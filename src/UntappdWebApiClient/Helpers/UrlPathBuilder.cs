using System;

namespace UntappdWebApiClient
{
    public class UrlPathBuilder
    {
        private string access_token;

        public void InitializeAccessToken(string accessToken)
        {
            access_token = $"&access_token={accessToken}";
        }

        public void ResetAccessToken()
        {
            access_token = String.Empty;
        }

        public string GetAPIUrl(string methodName)
        {
            if (String.IsNullOrEmpty(access_token))
                throw new ArgumentException("accessToken is not specified");

            return String.Concat(UrlConstants.BaseAPIUrl, methodName, access_token);
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