using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

/// <summary>
/// Сгенерировавано с помощью https://app.quicktype.io/
/// требуется доработка
/// </summary>
namespace QuickType.WebModels
{
    public partial class Temperatures
    {
        [JsonProperty("meta")]
        public Meta Meta { get; set; }

        [JsonProperty("notifications")]
        public Notifications Notifications { get; set; }

        [JsonProperty("response")]
        public Response Response { get; set; }
    }

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

    public partial class Response
    {
        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }

        [JsonProperty("checkins")]
        public Checkins Checkins { get; set; }
    }

    public partial class Checkins
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("items")]
        public CheckinsItem[] Items { get; set; }
    }

    public partial class CheckinsItem
    {
        [JsonProperty("checkin_id")]
        public long CheckinId { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("checkin_comment")]
        public string CheckinComment { get; set; }

        [JsonProperty("rating_score")]
        public double RatingScore { get; set; }

        [JsonProperty("user")]
        public CheckinUser User { get; set; }

        [JsonProperty("beer")]
        public Beer Beer { get; set; }

        [JsonProperty("brewery")]
        public Brewery Brewery { get; set; }

        [JsonProperty("venue")]
        [JsonConverter(typeof(ObjectToArrayConverter<Venue>))]
        public Venue[] Venue { get; set; }

        [JsonProperty("comments")]
        public Comments Comments { get; set; }

        [JsonProperty("toasts")]
        public Toasts Toasts { get; set; }

        [JsonProperty("media")]
        public Media Media { get; set; }

        [JsonProperty("source")]
        public Source Source { get; set; }

        [JsonProperty("badges")]
        public Badges Badges { get; set; }
    }

    public partial class Badges
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("items")]
        public BadgesItem[] Items { get; set; }

        [JsonProperty("retro_status", NullValueHandling = NullValueHandling.Ignore)]
        public bool? RetroStatus { get; set; }
    }

    public partial class BadgesItem
    {
        [JsonProperty("badge_id")]
        public long BadgeId { get; set; }

        [JsonProperty("user_badge_id")]
        public long UserBadgeId { get; set; }

        [JsonProperty("badge_name")]
        public string BadgeName { get; set; }

        [JsonProperty("badge_description")]
        public string BadgeDescription { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("badge_image")]
        public VenueIcon BadgeImage { get; set; }
    }

    public partial class VenueIcon
    {
        [JsonProperty("sm")]
        public Uri Sm { get; set; }

        [JsonProperty("md")]
        public Uri Md { get; set; }

        [JsonProperty("lg")]
        public Uri Lg { get; set; }
    }

    public partial class Beer
    {
        [JsonProperty("bid")]
        public long Bid { get; set; }

        [JsonProperty("beer_name")]
        public string BeerName { get; set; }

        [JsonProperty("beer_label")]
        public Uri BeerLabel { get; set; }

        [JsonProperty("beer_style")]
        public string BeerStyle { get; set; }

        [JsonProperty("beer_slug")]
        public string BeerSlug { get; set; }

        [JsonProperty("beer_abv")]
        public double BeerAbv { get; set; }

        [JsonProperty("beer_active")]
        public long BeerActive { get; set; }

        [JsonProperty("has_had")]
        public bool HasHad { get; set; }
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

        [JsonProperty("brewery_active")]
        public long BreweryActive { get; set; }
    }

    public partial class BreweryContact
    {
        [JsonProperty("twitter")]
        public string Twitter { get; set; }

        [JsonProperty("facebook")]
        public string Facebook { get; set; }

        [JsonProperty("instagram")]
        public string Instagram { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public partial class BreweryLocation
    {
        [JsonProperty("brewery_city")]
        public string BreweryCity { get; set; }

        [JsonProperty("brewery_state")]
        public string BreweryState { get; set; }

        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lng")]
        public double Lng { get; set; }
    }

    public partial class Comments
    {
        [JsonProperty("total_count")]
        public long TotalCount { get; set; }

        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("items")]
        public CommentsItem[] Items { get; set; }
    }

    public partial class CommentsItem
    {
        [JsonProperty("user")]
        public PurpleUser User { get; set; }

        [JsonProperty("checkin_id")]
        public long CheckinId { get; set; }

        [JsonProperty("comment_id")]
        public long CommentId { get; set; }

        [JsonProperty("comment_owner")]
        public bool CommentOwner { get; set; }

        [JsonProperty("comment_editor")]
        public bool CommentEditor { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("comment_source")]
        public string CommentSource { get; set; }
    }

    public partial class CheckinUser
    {
        [JsonProperty("uid")]
        public long Uid { get; set; }

        [JsonProperty("user_name")]
        public string UserName { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("is_supporter")]
        public long IsSupporter { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }

        [JsonProperty("bio")]
        public string Bio { get; set; }

        [JsonProperty("relationship")]
        public string Relationship { get; set; }

        [JsonProperty("user_avatar")]
        public Uri UserAvatar { get; set; }

        [JsonProperty("is_private", NullValueHandling = NullValueHandling.Ignore)]
        public long? IsPrivate { get; set; }

        [JsonProperty("contact", NullValueHandling = NullValueHandling.Ignore)]
        public object[] Contact { get; set; }
    }

    public partial class PurpleUser
    {
        [JsonProperty("uid")]
        public long Uid { get; set; }

        [JsonProperty("user_name")]
        public string UserName { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("bio")]
        public string Bio { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("relationship")]
        public string Relationship { get; set; }

        [JsonProperty("is_supporter")]
        public long IsSupporter { get; set; }

        [JsonProperty("user_avatar")]
        public Uri UserAvatar { get; set; }

        [JsonProperty("account_type", NullValueHandling = NullValueHandling.Ignore)]
        public string AccountType { get; set; }

        [JsonProperty("venue_details", NullValueHandling = NullValueHandling.Ignore)]
        public object[] VenueDetails { get; set; }

        [JsonProperty("brewery_details", NullValueHandling = NullValueHandling.Ignore)]
        public object[] BreweryDetails { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }

        [JsonProperty("is_private", NullValueHandling = NullValueHandling.Ignore)]
        public long? IsPrivate { get; set; }

        [JsonProperty("contact", NullValueHandling = NullValueHandling.Ignore)]
        public object[] Contact { get; set; }
    }

    public partial class Media
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("items")]
        public MediaItem[] Items { get; set; }
    }

    public partial class MediaItem
    {
        [JsonProperty("photo_id")]
        public long PhotoId { get; set; }

        [JsonProperty("photo")]
        public Photo Photo { get; set; }
    }

    public partial class Photo
    {
        [JsonProperty("photo_img_sm")]
        public Uri PhotoImgSm { get; set; }

        [JsonProperty("photo_img_md")]
        public Uri PhotoImgMd { get; set; }

        [JsonProperty("photo_img_lg")]
        public Uri PhotoImgLg { get; set; }

        [JsonProperty("photo_img_og")]
        public Uri PhotoImgOg { get; set; }
    }

    public partial class Source
    {
        [JsonProperty("app_name")]
        public string AppName { get; set; }

        [JsonProperty("app_website")]
        public Uri AppWebsite { get; set; }
    }

    public partial class Toasts
    {
        [JsonProperty("total_count")]
        public long TotalCount { get; set; }

        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("auth_toast")]
        public bool AuthToast { get; set; }

        [JsonProperty("items")]
        public ToastsItem[] Items { get; set; }
    }

    public partial class ToastsItem
    {
        [JsonProperty("uid")]
        public long Uid { get; set; }

        [JsonProperty("user")]
        public FluffyUser User { get; set; }

        [JsonProperty("like_id")]
        public long LikeId { get; set; }

        [JsonProperty("like_owner")]
        public bool LikeOwner { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }
    }

    public partial class FluffyUser
    {
        [JsonProperty("uid")]
        public long Uid { get; set; }

        [JsonProperty("user_name")]
        public string UserName { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("bio")]
        public string Bio { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("relationship")]
        public string Relationship { get; set; }

        [JsonProperty("user_avatar")]
        public Uri UserAvatar { get; set; }

        [JsonProperty("account_type")]
        public string AccountType { get; set; }

        [JsonProperty("venue_details")]
        public object[] VenueDetails { get; set; }
    }

    public partial class BreweryDetailsClass
    {
        [JsonProperty("brewery_id")]
        public long BreweryId { get; set; }
    }

    public partial class Venue
    {
        [JsonProperty("venue_id")]
        public long VenueId { get; set; }

        [JsonProperty("venue_name")]
        public string VenueName { get; set; }

        [JsonProperty("venue_slug")]
        public string VenueSlug { get; set; }

        [JsonProperty("primary_category_key")]
        public string PrimaryCategoryKey { get; set; }

        [JsonProperty("primary_category")]
        public string PrimaryCategory { get; set; }

        [JsonProperty("parent_category_id")]
        public string ParentCategoryId { get; set; }

        [JsonProperty("categories")]
        public Categories Categories { get; set; }

        [JsonProperty("location")]
        public VenueLocation Location { get; set; }

        [JsonProperty("contact")]
        public VenueContact Contact { get; set; }

        [JsonProperty("foursquare")]
        public Foursquare Foursquare { get; set; }

        [JsonProperty("venue_icon")]
        public VenueIcon VenueIcon { get; set; }

        [JsonProperty("is_verified")]
        public bool IsVerified { get; set; }
    }

    public partial class Categories
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("items")]
        public CategoriesItem[] Items { get; set; }
    }

    public partial class CategoriesItem
    {
        [JsonProperty("category_key")]
        public string CategoryKey { get; set; }

        [JsonProperty("category_name")]
        public string CategoryName { get; set; }

        [JsonProperty("category_id")]
        public string CategoryId { get; set; }

        [JsonProperty("is_primary")]
        public bool IsPrimary { get; set; }
    }

    public partial class VenueContact
    {
        [JsonProperty("twitter")]
        public string Twitter { get; set; }

        [JsonProperty("venue_url")]
        public string VenueUrl { get; set; }
    }

    public partial class Foursquare
    {
        [JsonProperty("foursquare_id")]
        public string FoursquareId { get; set; }

        [JsonProperty("foursquare_url")]
        public Uri FoursquareUrl { get; set; }
    }

    public partial class VenueLocation
    {
        [JsonProperty("venue_address")]
        public string VenueAddress { get; set; }

        [JsonProperty("venue_city")]
        public string VenueCity { get; set; }

        [JsonProperty("venue_state")]
        public string VenueState { get; set; }

        [JsonProperty("venue_country")]
        public string VenueCountry { get; set; }

        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lng")]
        public double Lng { get; set; }
    }

    public partial class Pagination
    {
        [JsonProperty("since_url")]
        public Uri SinceUrl { get; set; }

        [JsonProperty("next_url")]
        public Uri NextUrl { get; set; }

        [JsonProperty("max_id")]
        public long MaxId { get; set; }
    }

    public class ObjectToArrayConverter<T> : CustomCreationConverter<T[]>
    {
        public override T[] Create(Type objectType)
        {
            return new T[0];
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartArray)
            {
                return serializer.Deserialize(reader, objectType);
            }
            else
            {
                return new T[] { serializer.Deserialize<T>(reader) };
            }
        }
    }

}

