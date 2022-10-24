using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using HtmlAgilityPack;
using Newtonsoft.Json;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Models;
using QuickType.Checkins.WebModels;
using QuickType.Beers.WebModels;
using UntappdViewer.Interfaces;
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

        public void FillFullCheckins(CheckinsContainer checkinsContainer, ICancellationToken<Checkin> cancellation = null)
        {
            FillCheckins(checkinsContainer, 0, null, cancellation);
        }

        public void FillFirstCheckins(CheckinsContainer checkinsContainer, ICancellationToken<Checkin> cancellation = null)
        {
            FillCheckins(checkinsContainer, 0, checkinsContainer.Checkins.Max(item => item.Id), cancellation);
        }

        public void FillToEndCheckins(CheckinsContainer checkinsContainer, ICancellationToken<Checkin> cancellation = null)
        {
            FillCheckins(checkinsContainer, checkinsContainer.Checkins.Min(item => item.Id), null, cancellation);
        }

        public void UpdateBeers(List<Beer> beers, Func<Beer, bool> predicate, ref long offset, ICancellationToken<Checkin> cancellation = null)
        {
            if (beers.Count == 0)
                return;

            long countCheck = 0;
            long countUpdate = 0;
            bool isRun = true;
            while (isRun)
            {

                HttpResponseMessage httpResponse = GetHttpResponse($"user/beers/?offset={offset}&limit=50");
                if ((long)httpResponse.StatusCode == 429)
                    throw new ArgumentException(httpResponse.ReasonPhrase);

                string responseBody = httpResponse.Content.ReadAsStringAsync().Result;
                BeersQuickType beersQuickType = JsonConvert.DeserializeObject<BeersQuickType>(responseBody, jsonSerializerSettings);

                countCheck += beersQuickType.Response.Beers.Count;
                int currentCountUpdate = UpdateBeersHelper.UpdateBeers(beers, beersQuickType);
                if (currentCountUpdate > 0)
                    countUpdate += currentCountUpdate;

                UploadedCountInvoke(GetUpdateBeersMessage(countCheck, countUpdate));

                if (beersQuickType.Response.Pagination.Offset.HasValue)
                    offset = beersQuickType.Response.Pagination.Offset.Value;
                else
                    isRun = false;

                if (predicate != null && !beers.Any(predicate))
                    isRun = false;

                if (cancellation != null && cancellation.Cancel)
                    isRun = false;
            }
        }

        public void FillServingType(List<Checkin> checkins, string defaultServingType, ICancellationToken<Checkin> cancellation = null)
        {
            long countTotal = checkins.Count;
            long countUpdate = 0;
            long errorCount = 0;
            foreach (Checkin checkin in checkins)
            {
                if (cancellation != null && cancellation.Cancel)
                    return;

                string checkinUrl = UrlPathBuilder.GetСheckinUrl(checkin.Id);
                string servingType = GetServingType(checkinUrl, defaultServingType);
                if (String.IsNullOrEmpty(servingType))
                {
                    UploadedCountInvoke(Properties.Resources.ErrorUpdate);
                    errorCount++;
                    if (errorCount == 5)
                        return;

                    continue;
                }
                errorCount = 0;
                if (!servingType.Equals(checkin.ServingType))
                {
                    checkin.ServingType = servingType;
                    cancellation?.Items.Add(checkin);
                    countUpdate++;
                }
                UploadedCountInvoke(GetServingTypeMessage(countTotal, countUpdate));
            }
        }

        public string GetDevAvatarImageUrl()
        {
            HtmlDocument htmlDoc = GetHtmlDocument(UriConstants.DeveloperProfileUrl);
            if (htmlDoc == null)
                return String.Empty;

            List<HtmlNode> avatarNodea = htmlDoc.DocumentNode.Descendants("div").Where(node => node.GetAttributeValue("class", String.Empty).Contains("avatar-holder")).ToList();
            foreach (HtmlNode htmlNode in avatarNodea)
            {
                List<string> avatarUrl = htmlNode.Descendants("img").Select(item => item.GetAttributeValue("src", String.Empty)).Where(item => !String.IsNullOrEmpty(item)).ToList();
                if (avatarUrl.Count > 0)
                    return avatarUrl[0];
            }
            return String.Empty;
        }

        public string GetDevProfileHeaderImageUrl()
        {
            HtmlDocument htmlDoc = GetHtmlDocument(UriConstants.DeveloperProfileUrl);
            if (htmlDoc == null)
                return String.Empty;

            List<HtmlNode> coverNodes = htmlDoc.DocumentNode.Descendants("div").Where(node => node.GetAttributeValue("class", String.Empty).Contains("profile_header")).ToList();
            return coverNodes.Count == 0 ? String.Empty : coverNodes[0].GetAttributeValue("data-image-url", "");
        }

        public ICancellationToken<T> GetCancellationToken<T>()
        {
            return new CancellationToken<T>();
        }

        private void FillCheckins(CheckinsContainer checkinsContainer, long maxId, long? minId = null, ICancellationToken<Checkin> cancellation = null)
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


                if (cancellation != null && cancellation.Cancel)
                    isRun = false;
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

        private string GetServingTypeMessage(long total, long countUpdate)
        {
            int percent = total > 0 ? (int)Math.Truncate((double)countUpdate / total * 100) : 0;
            return $"{Properties.Resources.Total}:{total} / {Properties.Resources.Update}:{countUpdate} [{percent}%]";
        }

        private void UploadedCountInvoke(string message)
        {
            UploadedProgress?.Invoke(message);
        }

        private string GetServingType(string checkinUrl, string defaultServingType)
        {
            HtmlDocument htmlDoc = GetHtmlDocument(checkinUrl);
            if (htmlDoc == null)
                return String.Empty;

            List<HtmlNode> servingNode = htmlDoc.DocumentNode.Descendants("p").Where(node => node.GetAttributeValue("class", String.Empty).Contains("serving")).ToList();
            return servingNode.Count > 0 ? servingNode[0].InnerText.Trim() : defaultServingType;
        }

        private HtmlDocument GetHtmlDocument(string url)
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    string htmlPage = client.DownloadString(url);
                    HtmlDocument htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(htmlPage);
                    return htmlDoc;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
    }
}