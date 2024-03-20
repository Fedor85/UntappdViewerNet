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

        public static string GetAuthenticateUrl(string clientId, string redirectUrl)
        {
            return String.Concat(UrlConstants.AuthenticateUrl, $"?client_id={clientId}", "&response_type=code", $"&redirect_url={redirectUrl}");
        }

        public static string GetAuthorizecateUrl(string clientId,string clientSecret, string redirectUrl, string code)
        {
            return String.Concat(UrlConstants.AuthorizeUrl, $"?client_id={clientId}", $"&client_secret={clientSecret}", "&response_type=code", $"&redirect_url={redirectUrl}", $"&code={code}");
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