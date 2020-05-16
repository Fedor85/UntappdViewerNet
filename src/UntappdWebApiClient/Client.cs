using System;
using System.Net.Http;

namespace UntappdWebApiClient
{
    public class Client
    {
        private const string baseUrl = @"https://api.untappd.com/v4/";

        private UrlPathBuilder urlPathBuilder;

        public Client(string clinetId, string clientSecret)
        {
            urlPathBuilder = new UrlPathBuilder(baseUrl, clinetId, clientSecret);
        }

        public Client(string accessToken)
        {
            urlPathBuilder = new UrlPathBuilder(baseUrl, accessToken);
        }

        public bool Check(out string message)
        {
            return CheckSuccessResponse( "checkin/recent", out message);
        }

        public bool CheckUser(string userName, out string message)
        {
            return CheckSuccessResponse(String.Format("user/info/{0}", userName), out message);
        }

        private bool CheckSuccessResponse(string methodName, out string message)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage httpResponse = httpClient.GetAsync(urlPathBuilder.GetUrl(methodName)).Result;
                message = httpResponse.ReasonPhrase;
                return httpResponse.IsSuccessStatusCode;
            }
        }
    }
}