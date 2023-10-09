using Newtonsoft.Json;
using QuickType.Common.WebModels;

/// <summary>
/// Сгенерировавано с помощью https://app.quicktype.io/
/// </summary>
namespace QuickType.Brewery.WebModels
{
    public partial class BreweryQuickType
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
        [JsonProperty("brewery")]
        public Brewery Brewery { get; set; }
    }

    public partial class Brewery : Common.WebModels.Brewery
    {
        [JsonProperty("brewery_label_hd")]
        public string BreweryLabelHd { get; set; }

        [JsonProperty("brewery_in_production")]
        public long BreweryInProduction { get; set; }

        [JsonProperty("is_independent")]
        public long IsIndependent { get; set; }

        [JsonProperty("claimed_status")]
        public ClaimedStatus ClaimedStatus { get; set; }

        [JsonProperty("beer_count")]
        public long BeerCount { get; set; }

        [JsonProperty("brewery_type_id")]
        public long BreweryTypeId { get; set; }

        [JsonProperty("rating")]
        public Rating Rating { get; set; }

        [JsonProperty("brewery_description")]
        public string BreweryDescription { get; set; }

        [JsonProperty("stats")]
        public Stats Stats { get; set; }
    }

    public partial class ClaimedStatus
    {
        [JsonProperty("is_claimed")]
        public bool IsClaimed { get; set; }

        [JsonProperty("claimed_slug")]
        public string ClaimedSlug { get; set; }

        [JsonProperty("follow_status")]
        public bool FollowStatus { get; set; }

        [JsonProperty("follower_count")]
        public long FollowerCount { get; set; }

        [JsonProperty("uid")]
        public long Uid { get; set; }

        [JsonProperty("mute_status")]
        public string MuteStatus { get; set; }
    }

    public partial class Rating
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("rating_score")]
        public double RatingScore { get; set; }
    }

    public partial class Stats
    {
        [JsonProperty("total_count")]
        public long TotalCount { get; set; }

        [JsonProperty("unique_count")]
        public long UniqueCount { get; set; }

        [JsonProperty("monthly_count")]
        public long MonthlyCount { get; set; }

        [JsonProperty("weekly_count")]
        public long WeeklyCount { get; set; }

        [JsonProperty("user_count")]
        public long UserCount { get; set; }

        [JsonProperty("age_on_service")]
        public double AgeOnService { get; set; }
    }
}
