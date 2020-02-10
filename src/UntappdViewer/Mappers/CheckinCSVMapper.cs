using System.Collections.Generic;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using UntappdViewer.Mappers.CheckinParser;
using UntappdViewer.Models;

namespace UntappdViewer.Mappers
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