using System;

namespace UntappdWebApiClient
{
    public class UrlPathBuilder
    {
        private Uri baseUrl;

        private string authenticationUri;

        public UrlPathBuilder(string baseUrl, string clinetId, string clientSecretId)
        {
            this.baseUrl = new Uri(baseUrl);
            authenticationUri = String.Format("?client_id={0}&client_secret={1}", clinetId, clientSecretId);
        }

        public UrlPathBuilder(string baseUrl, string accessToken)
        {
            this.baseUrl = new Uri(baseUrl);
            authenticationUri = String.Format("?access_token={0}", accessToken);
        }

        public string GetUrl(string methodName)
        {
            return String.Concat(baseUrl, methodName, authenticationUri);
        }
    }
}