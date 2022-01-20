using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using QuickType.WebModels;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Models;
using Beer = UntappdViewer.Models.Beer;
using Brewery = UntappdViewer.Models.Brewery;
using Venue = UntappdViewer.Models.Venue;

namespace UntappdWebApiClient
{
    public class Client : IWebApiClient
    {
        private UrlPathBuilder urlPathBuilder;

        public event Action<int> ChangeUploadedCountEvent;

        public Client()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        public void Initialize(string accessToken)
        {
            urlPathBuilder = new UrlPathBuilder(UriConstants.BaseAPIUrl, accessToken);
        }

        public bool Check()
        {
            HttpResponseMessage httpResponse = GetHttpResponse("checkin/recent/?");
            if ((long)httpResponse.StatusCode == 429)
                throw new ArgumentException(httpResponse.ReasonPhrase);

            return httpResponse.IsSuccessStatusCode;
        }

        public void FillFullCheckins(List<Checkin> checkins)
        {
            FillCheckins(checkins, 0);
        }

        public void FillFullCheckins(CheckinsContainer checkinsContainer)
        {
            FillCheckins(checkinsContainer, 0);
        }

        public void FillFirstCheckins(List<Checkin> checkins, long endId)
        {
            FillCheckins(checkins, 0, endId);
        }

        public void FillFirstCheckins(CheckinsContainer checkinsContainer, long endId)
        {
            FillCheckins(checkinsContainer, 0);
        }

        public void FillToEndCheckins(List<Checkin> checkins, long startId)
        {
            FillCheckins(checkins, startId);
        }

        public void FillToEndCheckins(CheckinsContainer checkinsContainer, long endId)
        {
            FillCheckins(checkinsContainer, 0);
        }

        public void BeerUpdate(List<Beer> beers)
        {

        }

        private void FillCheckins(CheckinsContainer checkinsContainer, long maxId, long? minId = null)
        {
            long currentId = maxId;
            bool isRun = true;
            while (isRun)
            {
                HttpResponseMessage httpResponse = GetHttpResponse($"user/checkins/?max_id={currentId}&limit=50");
                if ((long)httpResponse.StatusCode == 429)
                    throw new ArgumentException(httpResponse.ReasonPhrase);

                string responseBody = httpResponse.Content.ReadAsStringAsync().Result;
                Temperatures temperatures = Newtonsoft.Json.JsonConvert.DeserializeObject<Temperatures>(responseBody);
                if (temperatures.Response.Pagination.MaxId.HasValue)
                {
                    List<Checkin> currentCheckins = CheckinMapper.GetCheckins(temperatures.Response.Checkins);

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
                        }
                        UploadedCountInvoke(checkinsContainer.Checkins);
                    }
                    else
                    {
                        foreach (Checkin currentCheckin in currentCheckins)
                            AddCheckin(currentCheckin, checkinsContainer);

                        UploadedCountInvoke(checkinsContainer.Checkins);
                    }
                    currentId = temperatures.Response.Pagination.MaxId.Value;
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

        private void FillCheckins(List<Checkin> checkins, long maxId, long? minId = null)
        {
            long currentId = maxId;
            while (true)
            {
                HttpResponseMessage httpResponse = GetHttpResponse($"user/checkins/?max_id={currentId}&limit=50");
                if ((long)httpResponse.StatusCode == 429)
                    throw new ArgumentException(httpResponse.ReasonPhrase);

                string responseBody = httpResponse.Content.ReadAsStringAsync().Result;
                Temperatures temperatures = Newtonsoft.Json.JsonConvert.DeserializeObject<Temperatures>(responseBody);
                if (temperatures.Response.Pagination.MaxId.HasValue)
                {
                    List<Checkin> currentCheckins = CheckinMapper.GetCheckins(temperatures.Response.Checkins);

                    if (minId.HasValue)
                    {
                        foreach (Checkin currentCheckin in currentCheckins)
                        {
                            if (currentCheckin.Id == minId.Value)
                                return;

                            checkins.Add(currentCheckin);
                            UploadedCountInvoke(checkins);
                        }
                    }
                    else
                    {
                        checkins.AddRange(currentCheckins);
                        UploadedCountInvoke(checkins);
                    }
                    currentId = temperatures.Response.Pagination.MaxId.Value;
                }
                else
                {
                    break;
                }
            }
        }

        private HttpResponseMessage GetHttpResponse(string methodName)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string url = urlPathBuilder.GetUrl(methodName);
                return httpClient.GetAsync(url).Result;
            }
        }

        private void UploadedCountInvoke(List<Checkin> checkins)
        {
            if (ChangeUploadedCountEvent != null)
                ChangeUploadedCountEvent.Invoke(checkins.Count);
        }
    }
}