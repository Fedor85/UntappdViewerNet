using System;
using Newtonsoft.Json;
using QuickType.Common.WebModels;

/// <summary>
/// Сгенерировавано с помощью https://app.quicktype.io/
/// </summary>
namespace QuickType.Beers.WebModels
{
    public partial class BeersQuickType
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
        [JsonProperty("total_count")]
        public long TotalCount { get; set; }

        [JsonProperty("dates")]
        public Dates Dates { get; set; }

        [JsonProperty("is_search")]
        public bool IsSearch { get; set; }

        [JsonProperty("sort")]
        public bool Sort { get; set; }

        [JsonProperty("type_id")]
        public bool TypeId { get; set; }

        [JsonProperty("country_id")]
        public bool CountryId { get; set; }

        [JsonProperty("brewery_id")]
        public bool BreweryId { get; set; }

        [JsonProperty("rating_score")]
        public bool RatingScore { get; set; }

        [JsonProperty("region_id")]
        public bool RegionId { get; set; }

        [JsonProperty("container_id")]
        public bool ContainerId { get; set; }

        [JsonProperty("is_multi_type")]
        public bool IsMultiType { get; set; }

        [JsonProperty("beers")]
        public Beers Beers { get; set; }

        [JsonProperty("sort_key")]
        public string SortKey { get; set; }

        [JsonProperty("sort_name")]
        public string SortName { get; set; }

        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }
    }

    public partial class Dates
    {
        [JsonProperty("first_checkin_date")]
        public string FirstCheckinDate { get; set; }

        [JsonProperty("start_date")]
        public bool StartDate { get; set; }

        [JsonProperty("end_date")]
        public bool EndDate { get; set; }
    }

    public partial class Pagination
    {
        [JsonProperty("next_url")]
        public Uri NextUrl { get; set; }

        [JsonProperty("offset")]
        public long? Offset { get; set; }

        [JsonProperty("max_id")]
        public bool MaxId { get; set; }
    }

    public partial class Beers
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("items")]
        public BeersItem[] Items { get; set; }

        [JsonProperty("sort_english")]
        public string SortEnglish { get; set; }

        [JsonProperty("sort_name")]
        public string SortName { get; set; }
    }

    public partial class BeersItem
    {
        [JsonProperty("first_checkin_id")]
        public long FirstCheckinId { get; set; }

        [JsonProperty("first_created_at")]
        public string FirstCreatedAt { get; set; }

        [JsonProperty("recent_checkin_id")]
        public long RecentCheckinId { get; set; }

        [JsonProperty("recent_created_at")]
        public string RecentCreatedAt { get; set; }

        [JsonProperty("rating_score")]
        public double RatingScore { get; set; }

        [JsonProperty("user_auth_rating_score")]
        public double UserAuthRatingScore { get; set; }

        [JsonProperty("first_had")]
        public string FirstHad { get; set; }

        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("beer")]
        public Beer Beer { get; set; }
    }

    public partial class Beer
    {
        [JsonProperty("bid")]
        public long Bid { get; set; }

        [JsonProperty("beer_name")]
        public string BeerName { get; set; }

        [JsonProperty("beer_label")]
        public Uri BeerLabel { get; set; }

        [JsonProperty("beer_abv")]
        public double BeerAbv { get; set; }

        [JsonProperty("beer_ibu")]
        public long BeerIbu { get; set; }

        [JsonProperty("beer_slug")]
        public string BeerSlug { get; set; }

        [JsonProperty("beer_style")]
        public string BeerStyle { get; set; }

        [JsonProperty("beer_description")]
        public string BeerDescription { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("rating_score")]
        public double RatingScore { get; set; }

        [JsonProperty("rating_count")]
        public long RatingCount { get; set; }
    }
}