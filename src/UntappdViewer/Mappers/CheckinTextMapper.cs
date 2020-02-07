using System;
using System.Collections.Generic;
using System.Linq;
using UntappdViewer.Mappers.CheckinParser;
using UntappdViewer.Models;

namespace UntappdViewer.Mappers
{
    public static class CheckinTextMapper
    {
        public static List<Checkin> GetCheckins(string text)
        {
            List<Checkin> checkins = new List<Checkin>();
            List<string> lines = text.Split(Convert.ToChar(10)).ToList();
            if (lines.Count < 2)
                return checkins;

            CheckinParserFactory checkinParserFactory = new CheckinParserFactory(lines[0]);
            lines.RemoveAt(0);
            foreach (string line in lines)
                checkins.Add(GetCheckin(checkinParserFactory.GetCheckinParser(line)));

            return checkins;
        }

        private static Checkin GetCheckin(CheckinParser.CheckinParser checkinParser)
        {
            Checkin checkin = new Checkin();

            checkin.Id = checkinParser.GetCheckinID();
            checkin.Url = checkinParser.GeCheckinURL();
            checkin.RatingScore = checkinParser.GetRatingScore();
            checkin.CreatedDate = checkinParser.GetCreatedData();
            checkin.Comment = checkinParser.GetComment();
            checkin.UrlPhoto = checkinParser.GePhotoURL();
            checkin.FlavorPprofiles = checkinParser.GeFlavorProfiles();
            checkin.VenuePurchase.Name = checkinParser.GePurchaseVenues();
            checkin.ServingType = checkinParser.GeServingType();

            checkin.Beer.Name = checkinParser.GetBeerName();
            checkin.Beer.Id = checkinParser.GetBeerID();
            checkin.Beer.Url = checkinParser.GeBeerURL();
            checkin.Beer.Type = checkinParser.GetBeerType();
            checkin.Beer.ABV = checkinParser.GetBeerABV();
            checkin.Beer.IBU = checkinParser.GetBeerIBU();

            checkin.Beer.Brewery.Id = checkinParser.GetBreweryID();
            checkin.Beer.Brewery.Name = checkinParser.GetBreweryName();
            checkin.Beer.Brewery.Url = checkinParser.GeBreweryURL();
            checkin.Beer.Brewery.Venue.Country = checkinParser.GeBreweryCountry();
            checkin.Beer.Brewery.Venue.State = checkinParser.GeBreweryState();
            checkin.Beer.Brewery.Venue.City = checkinParser.GeBreweryCity();

            checkin.Venue.Name = checkinParser.GetVenueName();
            checkin.Venue.Country = checkinParser.GeVenueCountry();
            checkin.Venue.State = checkinParser.GeVenueState();
            checkin.Venue.City = checkinParser.GetVenueCity();

            return checkin;
        }
    }
}