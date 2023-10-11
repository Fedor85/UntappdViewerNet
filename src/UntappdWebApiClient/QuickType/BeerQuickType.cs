using Newtonsoft.Json;
using QuickType.Common.WebModels;
using WebBeer = QuickType.Beers.WebModels.Beer;

namespace QuickType.Beer.WebModels
{
    namespace QuickType
    {
        public partial class BeerQuickType
        {
            [JsonProperty("meta")]
            public Meta Meta { get; set; }

            [JsonProperty("notifications")]
            public Notifications Notifications { get; set; }

            [JsonProperty("response")]
            public Response Response { get; set; }
        }

        public partial class Response
        {
            [JsonProperty("beer")]
            public WebBeer Beer { get; set; }
        }
    }
}