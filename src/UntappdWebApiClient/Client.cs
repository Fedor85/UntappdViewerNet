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
            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage httpResponse = httpClient.GetAsync(urlPathBuilder.GetUrl("checkin/recent")).Result;
                message = httpResponse.ReasonPhrase;
                return httpResponse.IsSuccessStatusCode;
            }
        }
    }
}