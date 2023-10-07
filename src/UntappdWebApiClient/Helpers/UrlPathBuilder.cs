using System;

namespace UntappdWebApiClient
{
    public class UrlPathBuilder
    {
        private Uri baseUrl;

        private string authenticationUri;

        public UrlPathBuilder(string baseUrl, string accessToken)
        {
            this.baseUrl = new Uri(baseUrl);
            authenticationUri = $"&access_token={accessToken}";
        }

        public string GetUrl(string methodName)
        {
            return String.Concat(baseUrl, methodName, authenticationUri);
        }

        public static string GetСheckinUrl(long checkinId)
        {
            return String.Concat(UriConstants.СheckinUrl, checkinId);
        }

        public static string GetBeerUrl(long beerId)
        {
            return String.Concat(UriConstants.BeerUrl, beerId);
        }
    }
}