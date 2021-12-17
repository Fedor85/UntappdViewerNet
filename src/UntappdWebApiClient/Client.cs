using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using QuickType.WebModels;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Models;
using Beer = UntappdViewer.Models.Beer;

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

        public void FillFirstCheckins(List<Checkin> checkins, long endId)
        {
            FillCheckins(checkins, 0, endId);
        }

        public void FillToEndCheckins(List<Checkin> checkins, long startId)
        {
            FillCheckins(checkins, startId);
        }

        public void BeerUpdate(List<Beer> beers)
        {

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