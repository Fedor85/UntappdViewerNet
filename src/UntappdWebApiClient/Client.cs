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
            return CheckSuccessResponse( "checkin/recent/?", out message);
        }

        public List<Checkin> GetCheckins()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string url = urlPathBuilder.GetUrl("user/checkins/?limit=50");
                HttpResponseMessage httpResponse = httpClient.GetAsync(url).Result;
                string responseBody = httpResponse.Content.ReadAsStringAsync().Result;
                Temperatures temperatures = Newtonsoft.Json.JsonConvert.DeserializeObject<Temperatures>(responseBody);
                return CheckinMapper.GetCheckins(temperatures.Response.Checkins);
            }
        }

        private bool CheckSuccessResponse(string methodName, out string message)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string url = urlPathBuilder.GetUrl(methodName);
                HttpResponseMessage httpResponse = httpClient.GetAsync(url).Result;
                message = httpResponse.ReasonPhrase;
                return httpResponse.IsSuccessStatusCode;
            }
        }
    }
}