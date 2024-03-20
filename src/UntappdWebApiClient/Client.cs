using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using HtmlAgilityPack;
using Newtonsoft.Json;
using QuickType.Beer.WebModels.QuickType;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Models;
using QuickType.Checkins.WebModels;
using QuickType.Beers.WebModels;
using QuickType.Brewery.WebModels;
using UntappdViewer.Interfaces;
using UntappdViewer.Models.Different;
using BeerWeb = QuickType.Beers.WebModels.Beer;
using Beer = UntappdViewer.Models.Beer;
using Brewery = UntappdViewer.Models.Brewery;
using BreweryWeb = QuickType.Common.WebModels.Brewery;
using Venue = UntappdViewer.Models.Venue;
using System.Text.Json;
using UntappdViewer.Utils;
using System.Text.Json.Nodes;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace UntappdWebApiClient
{
    public class Client : IWebApiClient
    {
        private const int CountAttemptsToGetHtmlDocument = 5;

        private UrlPathBuilder urlPathBuilder;

        private JsonSerializerSettings jsonSerializerSettings;

        public event Action<string> UploadedProgress;

        public bool IsLogOn { get; private set; }

        public Client()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
            urlPathBuilder = new UrlPathBuilder();
        }

        public IResponseMessage CheckAuthenticateUrl(string clientId, string redirectUrl)
        {
            string authenticateUrl = UrlPathBuilder.GetAuthenticateUrl(clientId, redirectUrl);
            HttpResponseMessage responseMessage = GetHttpResponse(authenticateUrl);
            string responseBody = responseMessage.Content.ReadAsStringAsync().Result;
            if (!responseBody.TryParseJson(out JsonDocument jsonDocument))
                return new ResponseMessage((int)responseMessage.StatusCode) { Message = authenticateUrl };

            JsonNode data = JsonSerializer.Deserialize<JsonNode>(responseBody);
            int httpCode = data["meta"]["http_code"].GetValue<int>();
            string message = data["meta"]["error_detail"].GetValue<string>();
            return new ResponseMessage(httpCode) {Message = message};
        }

        public IResponseMessage GetAccessToken(string clientId, string clientSecret, string redirectUrl, string code)
        {
            string authorizecateUrl = UrlPathBuilder.GetAuthorizecateUrl(clientId, clientSecret, redirectUrl, code);
            HttpResponseMessage responseMessage = GetHttpResponse(authorizecateUrl);
            string responseBody = responseMessage.Content.ReadAsStringAsync().Result;
            JsonNode data = JsonSerializer.Deserialize<JsonNode>(responseBody);
            int httpCode = data["meta"]["http_code"].GetValue<int>();
            string message = httpCode == (int)HttpStatusCode.OK ? data["response"]["access_token"].GetValue<string>()
                                                                : data["meta"]["error_detail"].GetValue<string>();
            return new ResponseMessage(httpCode) { Message = message };
        }

        public void LogOff()
        {
            IsLogOn = false;
            urlPathBuilder.ResetAccessToken();
        }

        public bool LogOn(string accessToken)
        {
            urlPathBuilder.InitializeAccessToken(accessToken);

            HttpResponseMessage httpResponse = GetHttpApiResponse("checkin/recent/?", true);
            if ((long)httpResponse.StatusCode == 429)
                throw new ArgumentException(httpResponse.ReasonPhrase);

            IsLogOn = httpResponse.IsSuccessStatusCode;
            return IsLogOn;
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

        public void UpdateBeers(List<Beer> beers, Func<Beer, bool> predicate, ref long offset, IBaseCancellationToken cancellation = null)
        {
            if (beers.Count == 0)
                return;

            List<long> beersCheck = new List<long>();
            long countUpdate = 0;
            bool isRun = true;
            while (isRun)
            {
                HttpResponseMessage httpResponse = GetHttpApiResponse($"user/beers/?offset={offset}&limit=50");
                if ((long)httpResponse.StatusCode == 429)
                    throw new ArgumentException(httpResponse.ReasonPhrase);

                string responseBody = httpResponse.Content.ReadAsStringAsync().Result;
                BeersQuickType beersQuickType = JsonConvert.DeserializeObject<BeersQuickType>(responseBody, jsonSerializerSettings);

                List<BeerWeb> beersWeb = beersQuickType.Response.Beers.Items.Select(item => item.Beer).ToList();
                beersCheck.AddRange(beersWeb.Select(item => item.Bid));
                int currentCountUpdate = UpdateBeersHelper.UpdateBeers(beers, beersWeb);
                countUpdate += currentCountUpdate;

                UploadedProgressByMessage(Properties.Resources.UpdateBeers + GetChekUpdateMessage(beersCheck.Count, countUpdate));

                if (beersQuickType.Response.Pagination.Offset.HasValue)
                    offset = beersQuickType.Response.Pagination.Offset.Value;
                else
                    isRun = false;

                if (beersCheck.Count >= beers.Count)
                {
                    List<Beer> notCheckBeers = beers.Where(item => !beersCheck.Contains(item.Id)).ToList();
                    if (notCheckBeers.Count > 0)
                    {
                        beersCheck.AddRange(notCheckBeers.Select(item => item.Id));

                        currentCountUpdate = UpdateBeers(notCheckBeers);
                        countUpdate += currentCountUpdate;

                        UploadedProgressByMessage(Properties.Resources.UpdateBeers + GetChekUpdateMessage(beersCheck.Count, countUpdate));
                    }
                    isRun = false;
                }

                if (predicate != null && !beers.Any(predicate))
                    isRun = false;

                if (cancellation != null && cancellation.Cancel)
                    isRun = false;
            }
        }

        public void UpdateBreweries(List<Brewery> breweries, IBaseCancellationToken cancellation = null)
        {
            foreach (Brewery brewery in breweries)
            {
                BreweryWeb breweryWeb = GetBreweryByApi(brewery.Id);
                if (breweryWeb != null)
                    BreweryMapper.FillBrewery(brewery, breweryWeb);

                if (cancellation != null && cancellation.Cancel)
                    return;
            }
        }

        public void FillServingType(List<Checkin> checkins, string defaultServingType, IBaseCancellationToken cancellation = null)
        {
            long countTotal = checkins.Count;
            long countUpdate = 0;
            long errorCount = 0;
            foreach (Checkin checkin in checkins)
            {
                if (cancellation != null && cancellation.Cancel)
                    return;

                string checkinUrl = UrlPathBuilder.GetСheckinUrl(checkin.Id);
                if (!TryServingType(checkinUrl, defaultServingType, out string servingType))
                {
                    UploadedProgressByMessage(Properties.Resources.ErrorUpdate);
                    errorCount++;
                    if (errorCount == CountAttemptsToGetHtmlDocument)
                        return;

                    continue;
                }
                errorCount = 0;
                if (!servingType.Equals(checkin.ServingType))
                {
                    checkin.ServingType = servingType;
                    countUpdate++;
                }
                UploadedProgressByMessage(Properties.Resources.FillServingType + GetTotalUpdateMessage(countTotal, countUpdate));
            }
        }

        public void FillCollaboration(List<Beer> beers, List<Brewery> breweries, IBaseCancellationToken cancellation = null)
        {
            long countTotal = beers.Count;
            long countCheck = 0;
            long countUpdate = 0;
            long errorCount = 0;

            foreach (Beer beer in beers)
            {
                if (cancellation != null && cancellation.Cancel)
                    return;

                string beerUrl = UrlPathBuilder.GetBeerUrl(beer.Id);
                if (!TryCollaborationBreweryIds(beerUrl, out List<long> breweryIds))
                {
                    UploadedProgressByMessage(Properties.Resources.ErrorUpdate);
                    errorCount++;
                    if (errorCount == CountAttemptsToGetHtmlDocument)
                        return;

                    continue;
                }
                countCheck++;
                if (breweryIds.Count == 0)
                {
                    beer.Collaboration.SetDefined();
                }
                else
                {
                    foreach (long breweryId in breweryIds)
                    {
                        Brewery brewery = breweries.FirstOrDefault(item => item.Id == breweryId);
                        if (brewery == null)
                        {
                            brewery = GetBrewery(breweryId);
                            if (brewery == null)
                                continue;

                            breweries.Add(brewery);
                        }
                        beer.Collaboration.AddBrewery(brewery);
                    }

                    if (beer.Collaboration.State == CollaborationState.Undefined)
                        beer.Collaboration.SetDefined();
                    else
                        countUpdate++;
                }

                UploadedProgressByMessage(Properties.Resources.FillCollaboration + GetTotaAndCheklUpdateMessage(countTotal, countCheck, countUpdate));
            }
        }

        public string GetDevAvatarImageUrl()
        {
            HtmlDocument htmlDoc = GetHtmlDocument(UrlConstants.DeveloperProfileUrl);
            if (htmlDoc == null)
                return String.Empty;

            HtmlNode avatarNodea = htmlDoc.DocumentNode.Descendants("div").FirstOrDefault(node => node.GetAttributeValue("class", String.Empty).Contains("avatar-holder"));
            return avatarNodea == null ? String.Empty : avatarNodea.Descendants("img").Select(item => item.GetAttributeValue("src", String.Empty)).FirstOrDefault();
        }

        public string GetDevProfileHeaderImageUrl()
        {
            HtmlDocument htmlDoc = GetHtmlDocument(UrlConstants.DeveloperProfileUrl);
            if (htmlDoc == null)
                return String.Empty;

            HtmlNode coverNodes = htmlDoc.DocumentNode.Descendants("div").FirstOrDefault(node => node.GetAttributeValue("class", String.Empty).Contains("profile_header"));
            return coverNodes == null ? String.Empty : coverNodes.GetAttributeValue("data-image-url", String.Empty);
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
                HttpResponseMessage httpResponse = GetHttpApiResponse($"user/checkins/?max_id={currentId}&limit=50");
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
                            cancellation?.Items.Add(currentCheckin);
                            counter++;
                        }
                        UploadedProgressByMessage(Properties.Resources.FillCheckins + GetFillCountMessage(counter));
                    }
                    else
                    {
                        foreach (Checkin currentCheckin in currentCheckins)
                        {
                            AddCheckin(currentCheckin, checkinsContainer);
                            cancellation?.Items.Add(currentCheckin);
                            counter++;
                        }
                        UploadedProgressByMessage(Properties.Resources.FillCheckins + GetFillCountMessage(counter));
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

        private int UpdateBeers(List<Beer> beers)
        {
            int counter = 0;
            foreach (Beer beer in beers)
            {
                HttpResponseMessage httpResponse = GetHttpApiResponse($"beer/info/{beer.Id}/?");
                if ((long)httpResponse.StatusCode == 429)
                    throw new ArgumentException(httpResponse.ReasonPhrase);

                string responseBody = httpResponse.Content.ReadAsStringAsync().Result;
                BeerQuickType beerQuickType = JsonConvert.DeserializeObject<BeerQuickType>(responseBody);
                if (UpdateBeersHelper.UpdateBeer(beer, beerQuickType.Response.Beer))
                    counter++;
            }
            return counter;
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

        private HttpResponseMessage GetHttpApiResponse(string methodName, bool isLogOnRequest = false)
        {
            if(!isLogOnRequest && !IsLogOn)
                throw new ArgumentException(Properties.Resources.ServiceIsNotAuthorized);

            return GetHttpResponse(urlPathBuilder.GetAPIUrl(methodName));
        }

        private HttpResponseMessage GetHttpResponse(string url)
        {
            using HttpClient httpClient = new HttpClient();
            return httpClient.GetAsync(url).Result;
        }

        private bool TryServingType(string checkinUrl, string defaultServingType, out string servingType)
        {
            servingType = String.Empty;
            HtmlDocument htmlDoc = GetHtmlDocument(checkinUrl);
            if (htmlDoc == null)
                return false;

            List<HtmlNode> servingNode = htmlDoc.DocumentNode.Descendants("p").Where(node => node.GetAttributeValue("class", String.Empty).Contains("serving")).ToList();
            servingType = servingNode.Count > 0 ? servingNode[0].InnerText.Trim() : defaultServingType;
            return true;
        }

        private bool TryCollaborationBreweryIds(string beerUrl, out List<long> breweryIds)
        {
            breweryIds = new List<long>();
            HtmlDocument htmlDoc = GetHtmlDocument(beerUrl);
            if (htmlDoc == null)
                return false;

            HtmlNode collaboration = htmlDoc.DocumentNode.Descendants("p").FirstOrDefault(node => node.InnerText.Trim().ToLower().Contains("collaboration with"));
            if (collaboration != null)
            {
                IEnumerable<HtmlNode> htmlNodes  = collaboration.Descendants("a").Where(item => item.GetAttributeValue("href", String.Empty).ToLower().Contains("/brewery/"));
                foreach (string breweryNode in htmlNodes.Select(item => item.GetAttributeValue("href", String.Empty)))
                {
                    string breweryId = String.Concat(breweryNode.Where(Char.IsDigit).ToArray());
                    if(!String.IsNullOrEmpty(breweryId))
                        breweryIds.Add(Convert.ToInt64(breweryId));
                }
            }
            return true;
        }

        private Brewery GetBrewery(long breweryId)
        {
            if(!IsLogOn)
                return new Brewery { Id = breweryId };

            BreweryWeb breweryWeb =  GetBreweryByApi(breweryId);
            if (breweryWeb == null)
                return null;

            Brewery brewery = new Brewery();
            BreweryMapper.FillBrewery(brewery, breweryWeb);
            return brewery;
        }

        private BreweryWeb GetBreweryByApi(long breweryId)
        {
            HttpResponseMessage httpResponse = GetHttpApiResponse($"brewery/info/{breweryId}/?");
            long statusCode = (long) httpResponse.StatusCode;
            if (statusCode == 429)
                throw new ArgumentException(httpResponse.ReasonPhrase);

            if (statusCode == 404)
                return null;

            string responseBody = httpResponse.Content.ReadAsStringAsync().Result;
            BreweryQuickType checkinsQuickType = JsonConvert.DeserializeObject<BreweryQuickType>(responseBody);

            return checkinsQuickType.Response.Brewery;
        }

        private HtmlDocument GetHtmlDocument(string url)
        {
            using WebClient client = new WebClient();
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

        #region Message

        private string GetFillCountMessage(int count)
        {
            return $"{Properties.Resources.Uploaded}: {count}";
        }

        private string GetChekUpdateMessage(long countChek, long countUpdate)
        {
            int percent = countChek > 0 ? (int)Math.Truncate((double)countUpdate / countChek * 100) : 0;
            return $"{Properties.Resources.Chek}:{countChek} / {Properties.Resources.Update}:{countUpdate} [{percent}%]";
        }

        private string GetTotalUpdateMessage(long total, long countUpdate)
        {
            int percent = total > 0 ? (int)Math.Truncate((double)countUpdate / total * 100) : 0;
            return $"{Properties.Resources.Total}:{total} / {Properties.Resources.Update}:{countUpdate} [{percent}%]";
        }

        private string GetTotaAndCheklUpdateMessage(long total, long countChek, long countUpdate)
        {
            int percentChek = total > 0 ? (int)Math.Truncate((double)countChek / total * 100) : 0;
            int percentUpdate = countChek > 0 ? (int)Math.Truncate((double)countUpdate / countChek * 100) : 0;
            return $"{Properties.Resources.Total}:{total} / {Properties.Resources.Chek}:{countChek} [{percentChek}%] / {Properties.Resources.Update}:{countUpdate} [{percentUpdate}%]";
        }

        private void UploadedProgressByMessage(string message)
        {
            UploadedProgress?.Invoke(message);
        }

        #endregion
    }
}