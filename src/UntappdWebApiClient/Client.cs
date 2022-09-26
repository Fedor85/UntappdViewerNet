using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Models;
using QuickType.Checkins.WebModels;
using QuickType.Beers.WebModels;
using Beer = UntappdViewer.Models.Beer;
using Brewery = UntappdViewer.Models.Brewery;
using Venue = UntappdViewer.Models.Venue;

namespace UntappdWebApiClient
{
    public class Client : IWebApiClient
    {
        private UrlPathBuilder urlPathBuilder;

        private JsonSerializerSettings jsonSerializerSettings;

        public event Action<string> UploadedProgress;

        public Client()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        public void Initialize(string accessToken)
        {
            urlPathBuilder = new UrlPathBuilder(UriConstants.BaseAPIUrl, accessToken);

            jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
        }

        public bool Check()
        {
            HttpResponseMessage httpResponse = GetHttpResponse("checkin/recent/?");
            if ((long)httpResponse.StatusCode == 429)
                throw new ArgumentException(httpResponse.ReasonPhrase);

            return httpResponse.IsSuccessStatusCode;
        }

        public void FillFullCheckins(CheckinsContainer checkinsContainer)
        {
            FillCheckins(checkinsContainer, 0);
        }

        public void FillFirstCheckins(CheckinsContainer checkinsContainer)
        {
            FillCheckins(checkinsContainer, 0, checkinsContainer.Checkins.Max(item => item.Id));
        }

        public void FillToEndCheckins(CheckinsContainer checkinsContainer)
        {
            FillCheckins(checkinsContainer, checkinsContainer.Checkins.Min(item => item.Id));
        }

        public void UpdateBeers(List<Beer> beers, Func<Beer, bool> predicate, ref long offset)
        {
            if (beers.Count == 0)
                return;

            long countChek = 0;
            long countUpdate = 0;
            bool isRun = true;
            while (isRun)
            {
                HttpResponseMessage httpResponse = GetHttpResponse($"user/beers/?offset={offset}&limit=50");
                if ((long)httpResponse.StatusCode == 429)
                    throw new ArgumentException(httpResponse.ReasonPhrase);

                string responseBody = httpResponse.Content.ReadAsStringAsync().Result;
                BeersQuickType beersQuickType = JsonConvert.DeserializeObject<BeersQuickType>(responseBody, jsonSerializerSettings);

                countChek += beersQuickType.Response.Beers.Count;
                int currentCountUpdate = UpdateBeersHelper.UpdateBeers(beers, beersQuickType);
                if (currentCountUpdate > 0)
                    countUpdate += currentCountUpdate;

                UploadedCountInvoke(GetUpdateBeersMessage(countChek, countUpdate));

                if (beersQuickType.Response.Pagination.Offset.HasValue)
                    offset = beersQuickType.Response.Pagination.Offset.Value;
                else
                    isRun = false;

                if (predicate != null && !beers.Any(predicate))
                    isRun = false;
            }
        }

        private void FillCheckins(CheckinsContainer checkinsContainer, long maxId, long? minId = null)
        {
            long currentId = maxId;
            int counter = 0;
            bool isRun = true;
            while (isRun)
            {
                HttpResponseMessage httpResponse = GetHttpResponse($"user/checkins/?max_id={currentId}&limit=50");
                if ((long)httpResponse.StatusCode == 429)
                    throw new ArgumentException(httpResponse.ReasonPhrase);

                string responseBody = httpResponse.Content.ReadAsStringAsync().Result;
                CheckinsQuickType checkinsQuickType = JsonConvert.DeserializeObject<CheckinsQuickType>(responseBody);
                if (checkinsQuickType.Response.Pagination.MaxId.HasValue)
                {
                    List<Checkin> currentCheckins = CheckinMapper.GetCheckins(checkinsQuickType.Response.Checkins);

                    if (minId.HasValue && currentCheckins.Any(item => item.Id == minId.Value))
                    {
                        foreach (Checkin currentCheckin in currentCheckins)
                        {
                            if (currentCheckin.Id == minId.Value)
                            {
                                isRun = false;
                                break;
                            }
                            AddCheckin(currentCheckin, checkinsContainer);
                            counter++;
                        }
                        UploadedCountInvoke(GetFillCheckinsMessage(counter));
                    }
                    else
                    {
                        foreach (Checkin currentCheckin in currentCheckins)
                        {
                            AddCheckin(currentCheckin, checkinsContainer);
                            counter++;
                        }
                        UploadedCountInvoke(GetFillCheckinsMessage(counter));
                    }
                    currentId = checkinsQuickType.Response.Pagination.MaxId.Value;
                }
                else
                {
                    isRun = false;
                }
            }
        }

        private void AddCheckin(Checkin checkin, CheckinsContainer checkinsContainer)
        {
            Beer beer = checkinsContainer.GetBeer(checkin.Beer.Id);
            if (beer == null)
            {
                beer = checkin.Beer;
                Brewery brewery = checkinsContainer.GetBrewery(beer.Brewery.Id);
                if (brewery == null)
                {
                    brewery = beer.Brewery;
                    Venue venue = brewery.Venue;
                    if (IsUpdateVenue(ref venue, checkinsContainer))
                        brewery.Venue = venue;

                    checkinsContainer.AddBrewery(brewery);
                }
                else
                {
                    beer.Brewery = brewery;
                }
                checkinsContainer.AddBeer(beer);
            }
            else
            {
                checkin.Beer = beer;
            }

            FillCheckinVenue(checkin, checkinsContainer);
            checkinsContainer.AddCheckin(checkin);
        }

        private static bool IsUpdateVenue(ref Venue venue, CheckinsContainer checkinsContainer)
        {
            Venue existVenue = checkinsContainer.GetVenue(venue);
            if (existVenue != null)
            {
                venue = existVenue;
                return true;
            }
            checkinsContainer.AddVenue(venue);
            return false;
        }

        private static void FillCheckinVenue(Checkin checkin, CheckinsContainer checkinsContainer)
        {
            Venue existVenue = checkinsContainer.GetVenue(checkin.Venue);
            if (existVenue != null)
                checkin.Venue = existVenue;
            else
                checkinsContainer.AddVenue(checkin.Venue);
        }

        private HttpResponseMessage GetHttpResponse(string methodName)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string url = urlPathBuilder.GetUrl(methodName);
                return httpClient.GetAsync(url).Result;
            }
        }

        private string GetFillCheckinsMessage(int count)
        {
            return $"{Properties.Resources.Uploaded}: {count}";
        }

        private string GetUpdateBeersMessage(long countChek, long countUpdate)
        {
            int percent = countChek > 0 ? (int) Math.Truncate((double) countUpdate / countChek * 100) : 0;
            return $"{Properties.Resources.Chek}:{countChek} / {Properties.Resources.Update}:{countUpdate} [{percent}%]";
        }

        private void UploadedCountInvoke(string message)
        {
            UploadedProgress?.Invoke(message);
        }
    }
}