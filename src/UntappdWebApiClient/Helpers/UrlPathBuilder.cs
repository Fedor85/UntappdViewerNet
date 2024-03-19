using System;

namespace UntappdWebApiClient
{
    public class UrlPathBuilder
    {
        private Uri baseUrl;

        private string authenticationUri;

        public UrlPathBuilder(string baseUrl, string accessToken)
        {
            if (String.IsNullOrEmpty(baseUrl))
                throw new ArgumentException(Properties.Resources.EmptyUrl);

            this.baseUrl = new Uri(baseUrl);
            authenticationUri = $"&access_token={accessToken}";
        }

        public string GetUrl(string methodName)
        {
            return String.Concat(baseUrl, methodName, authenticationUri);
        }

        public static string GetСheckinUrl(long checkinId)
        {
            return String.Concat(UrlConstants.СheckinUrl, checkinId);
        }

        public static string GetBeerUrl(long beerId)
        {
            return String.Concat(UrlConstants.BeerUrl, beerId);
        }

        public static string GetBreweryUrl(long breweryId)
        {
            return String.Concat(UrlConstants.BreweryUrl, breweryId);
        }
    }
}