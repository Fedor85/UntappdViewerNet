using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using QuickType.WebModels;
using UntappdViewer.Models;

namespace UntappdWebApiClient
{
    public class Client
    {
        private const string baseUrl = @"https://api.untappd.com/v4/";

        private UrlPathBuilder urlPathBuilder;

        private Client()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        public Client(string accessToken) : this()
        {
            urlPathBuilder = new UrlPathBuilder(baseUrl, accessToken);
        }

        public bool Check(out string message)
        {
            HttpResponseMessage httpResponse = GetHttpResponse("checkin/recent/?");
            message = httpResponse.ReasonPhrase;
            return httpResponse.IsSuccessStatusCode;
        }

        public List<Checkin> GetCheckins()
        {
            List<Checkin> checkins = new List<Checkin>();
            long currentId = 0;
            while (true)
            {
                HttpResponseMessage httpResponse = GetHttpResponse($"user/checkins/?max_id={currentId}&limit=50");
                string responseBody = httpResponse.Content.ReadAsStringAsync().Result;
                Temperatures temperatures = Newtonsoft.Json.JsonConvert.DeserializeObject<Temperatures>(responseBody);
                if (temperatures.Response.Pagination.MaxId.HasValue)
                {
                    checkins.AddRange(CheckinMapper.GetCheckins(temperatures.Response.Checkins));
                    currentId = temperatures.Response.Pagination.MaxId.Value;
                }
                else
                {
                    break;
                }
            }
            return checkins;
        }

        private HttpResponseMessage GetHttpResponse(string methodName)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string url = urlPathBuilder.GetUrl(methodName);
                return httpClient.GetAsync(url).Result;
            }
        }
    }
}