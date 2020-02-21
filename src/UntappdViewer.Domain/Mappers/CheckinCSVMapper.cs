using System.Collections.Generic;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using UntappdViewer.Domain.Mappers.CheckinParser;
using UntappdViewer.Models;

namespace UntappdViewer.Domain.Mappers
{
    public static class CheckinCSVMapper
    {
        public static List<Checkin> GetCheckins(Stream stream)
        {
            List<Checkin> checkins = new List<Checkin>();
            if (stream == null)
                return checkins;

            using (TextFieldParser csvParser = new TextFieldParser(stream))
            {
                csvParser.SetDelimiters(",");
                csvParser.HasFieldsEnclosedInQuotes = true;
                string parametersNameLine = csvParser.ReadLine();
                CheckinParserFactory checkinParserFactory = new CheckinParserFactory(parametersNameLine);
                while (!csvParser.EndOfData)
                {
                    string[] parametersValue = csvParser.ReadFields();
                    checkins.Add(GetCheckin(checkinParserFactory.GetCheckinParser(parametersValue)));
                }
            }
            return checkins;
        }

        private static Checkin GetCheckin(CheckinParser.CheckinParser checkinParser)
        {
            Checkin checkin = new Checkin();

            checkin.Id = checkinParser.GetCheckinID();
            checkin.Url = checkinParser.GetCheckinURL();
            checkin.RatingScore = checkinParser.GetRatingScore();
            checkin.CreatedDate = checkinParser.GetCreatedData();
            checkin.Comment = checkinParser.GetComment();
            checkin.UrlPhoto = checkinParser.GetPhotoURL();
            checkin.FlavorPprofiles = checkinParser.GetFlavorProfiles();
            checkin.VenuePurchase.Name = checkinParser.GetPurchaseVenues();
            checkin.ServingType = checkinParser.GetServingType();
            checkin.TaggedFriends = checkinParser.GetTaggedFriends();
            checkin.TotalToasts = checkinParser.GetTotalToasts();
            checkin.TotalComments = checkinParser.GetTotalComments();

            checkin.Beer.Name = checkinParser.GetBeerName();
            checkin.Beer.Id = checkinParser.GetBeerID();
            checkin.Beer.GlobalRatingScore = checkinParser.GetGlobalRatingScore();
            checkin.Beer.GlobalWeightedRatingScore = checkinParser.GetGlobalWeightedRatingScore();
            checkin.Beer.Url = checkinParser.GetBeerURL();
            checkin.Beer.Type = checkinParser.GetBeerType();
            checkin.Beer.ABV = checkinParser.GetBeerABV();
            checkin.Beer.IBU = checkinParser.GetBeerIBU();

            checkin.Beer.Brewery.Id = checkinParser.GetBreweryID();
            checkin.Beer.Brewery.Name = checkinParser.GetBreweryName();
            checkin.Beer.Brewery.Url = checkinParser.GetBreweryURL();
            checkin.Beer.Brewery.Venue.Country = checkinParser.GetBreweryCountry();
            checkin.Beer.Brewery.Venue.State = checkinParser.GetBreweryState();
            checkin.Beer.Brewery.Venue.City = checkinParser.GetBreweryCity();

            checkin.Venue.Name = checkinParser.GetVenueName();
            checkin.Venue.Country = checkinParser.GetVenueCountry();
            checkin.Venue.State = checkinParser.GetVenueState();
            checkin.Venue.City = checkinParser.GetVenueCity();
            checkin.Venue.Latitude = checkinParser.GetVenueLat();
            checkin.Venue.Longitude = checkinParser.GetVenueLng();

            return checkin;
        }
    }
}