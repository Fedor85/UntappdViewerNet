using System;
using Newtonsoft.Json;


namespace QuickType.Common.WebModels
{
    public partial class Meta
    {
        [JsonProperty("code")]
        public long Code { get; set; }

        [JsonProperty("response_time")]
        public Time ResponseTime { get; set; }

        [JsonProperty("init_time")]
        public Time InitTime { get; set; }
    }

    public partial class Time
    {
        [JsonProperty("time")]
        public double TimeTime { get; set; }

        [JsonProperty("measure")]
        public string Measure { get; set; }
    }

    public partial class Notifications
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("unread_count")]
        public UnreadCount UnreadCount { get; set; }
    }

    public partial class UnreadCount
    {
        [JsonProperty("comments")]
        public long Comments { get; set; }

        [JsonProperty("toasts")]
        public long Toasts { get; set; }

        [JsonProperty("friends")]
        public long Friends { get; set; }

        [JsonProperty("messages")]
        public long Messages { get; set; }

        [JsonProperty("venues")]
        public long Venues { get; set; }

        [JsonProperty("veunes")]
        public long Veunes { get; set; }

        [JsonProperty("others")]
        public long Others { get; set; }

        [JsonProperty("news")]
        public long News { get; set; }
    }

    public partial class Brewery
    {
        [JsonProperty("brewery_id")]
        public long BreweryId { get; set; }

        [JsonProperty("brewery_name")]
        public string BreweryName { get; set; }

        [JsonProperty("brewery_slug")]
        public string BrewerySlug { get; set; }

        [JsonProperty("brewery_page_url")]
        public string BreweryPageUrl { get; set; }

        [JsonProperty("brewery_type")]
        public string BreweryType { get; set; }

        [JsonProperty("brewery_label")]
        public Uri BreweryLabel { get; set; }

        [JsonProperty("country_name")]
        public string CountryName { get; set; }

        [JsonProperty("contact")]
        public BreweryContact Contact { get; set; }

        [JsonProperty("location")]
        public BreweryLocation Location { get; set; }
    }

    public partial class BreweryContact
    {
        [JsonProperty("twitter")]
        public string Twitter { get; set; }

        [JsonProperty("facebook")]
        public Uri Facebook { get; set; }

        [JsonProperty("instagram", NullValueHandling = NullValueHandling.Ignore)]
        public string Instagram { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }
    }

    public partial class BreweryLocation
    {
        [JsonProperty("brewery_address")]
        public string BreweryAddress { get; set; }

        [JsonProperty("brewery_city")]
        public string BreweryCity { get; set; }

        [JsonProperty("brewery_state")]
        public string BreweryState { get; set; }

        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lng")]
        public double Lng { get; set; }
    }
}
