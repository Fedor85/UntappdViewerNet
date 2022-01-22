using System;
using System.Collections.Generic;
using QuickType.Checkins.WebModels;
using UntappdViewer.Models;
using Beer = UntappdViewer.Models.Beer;
using Venue = UntappdViewer.Models.Venue;
using VenueWeb = QuickType.Checkins.WebModels.Venue;
using BeerWeb = QuickType.Checkins.WebModels.Beer;
using Brewery = UntappdViewer.Models.Brewery;
using BreweryWeb = QuickType.Checkins.WebModels.Brewery;

namespace UntappdWebApiClient
{
    public static class CheckinMapper
    {
        public static List<Checkin> GetCheckins(Checkins checkinsWeb)
        {
            List<Checkin> checkins = new List<Checkin>();
            foreach (CheckinsItem checkinsItem in checkinsWeb.Items)
                checkins.Add(GetCheckin(checkinsItem));

            return checkins;
        }

        private static Checkin GetCheckin(CheckinsItem checkinsItem)
        {
            Checkin checkin = new Checkin();
            checkin.Id = checkinsItem.CheckinId;
            checkin.Url = $"{UriConstants.BaseUri}c/{checkinsItem.CheckinId}";
            checkin.CreatedDate = DateTime.Parse(checkinsItem.CreatedAt);
            checkin.Comment = checkinsItem.CheckinComment;
            checkin.RatingScore = checkinsItem.RatingScore;
            checkin.UrlPhoto = GetUrlPhoto(checkinsItem);
            checkin.TotalComments = Convert.ToInt32(checkinsItem.Comments.TotalCount);
            checkin.TotalToasts = Convert.ToInt32(checkinsItem.Toasts.TotalCount);

            if (checkinsItem.Venue.Length > 0)
                FillVenue(checkin.Venue, checkinsItem.Venue[0]);

            FillBeer(checkin.Beer, checkinsItem.Beer);
            FillBrewery(checkin.Beer.Brewery, checkinsItem.Brewery);
            FillBadges(checkin.Badges, checkinsItem.Badges);

            return checkin;
        }

        private static string GetUrlPhoto(CheckinsItem checkinsItem)
        {
            if (checkinsItem.Media.Count == 0)
                return String.Empty;

            return checkinsItem.Media.Items[0].Photo.PhotoImgOg.ToString();
        }

        private static void FillVenue(Venue venue, VenueWeb venueWeb)
        {
            venue.Id = venueWeb.VenueId;
            venue.Name = venueWeb.VenueName;
            venue.Country = venueWeb.Location.VenueCountry;
            venue.State = venueWeb.Location.VenueState;
            venue.City = venueWeb.Location.VenueCity;
            venue.Latitude = venueWeb.Location.Lat;
            venue.Longitude = venueWeb.Location.Lng;
        }

        private static void FillBeer(Beer beer, BeerWeb beerWeb)
        {
            beer.Id = beerWeb.Bid;
            beer.Name = beerWeb.BeerName;
            beer.Type = beerWeb.BeerStyle;
            beer.ABV = beerWeb.BeerAbv;
            beer.Url = $"{UriConstants.BaseUri}beer/{beerWeb.Bid}";
            beer.LabelUrl = beerWeb.BeerLabel.ToString();
        }

        private static void FillBrewery(Brewery brewery, BreweryWeb breweryWeb)
        {
            brewery.Id = breweryWeb.BreweryId;
            brewery.Name = breweryWeb.BreweryName;
            brewery.Url = $"{UriConstants.BaseUri}brewery/{breweryWeb.BreweryId}";
            brewery.Venue.Country = breweryWeb.CountryName;
            brewery.Venue.City = breweryWeb.Location.BreweryCity;
            brewery.Venue.State = breweryWeb.Location.BreweryState;
            brewery.Venue.Latitude = breweryWeb.Location.Lat;
            brewery.Venue.Longitude = breweryWeb.Location.Lng;
            brewery.LabelUrl = breweryWeb.BreweryLabel.ToString();
        }

        private static void FillBadges(List<Badge> checkinBadges, Badges webCheckinsBadges)
        {
            foreach (BadgesItem badgesItem in webCheckinsBadges.Items)
            {
                Badge badge = new Badge();
                badge.Id = badgesItem.BadgeId;
                badge.Name = badgesItem.BadgeName;
                badge.Description = badgesItem.BadgeDescription;
                badge.ImageUrl = badgesItem.BadgeImage.Lg.ToString();
                checkinBadges.Add(badge);
            }
        }
    }
}