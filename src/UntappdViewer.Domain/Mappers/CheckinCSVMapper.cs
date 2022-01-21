using System;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using UntappdViewer.Domain.Mappers.CheckinParser;
using UntappdViewer.Models;

namespace UntappdViewer.Domain.Mappers
{
    public static class CheckinCSVMapper
    {
        public static void InitializeCheckinsContainer(CheckinsContainer checkinsContainer, Stream stream)
        {
            if (stream == null)
                return;

            using (TextFieldParser csvParser = new TextFieldParser(stream))
            {
                csvParser.SetDelimiters(",");
                csvParser.HasFieldsEnclosedInQuotes = true;
                string parametersNameLine = csvParser.ReadLine();
                CheckinParserFactory checkinParserFactory = new CheckinParserFactory(parametersNameLine);
                while (!csvParser.EndOfData)
                {
                    string[] parametersValue = csvParser.ReadFields();
                    FillCheckinsContainer(checkinsContainer, checkinParserFactory.GetCheckinParser(parametersValue));
                }
            }
        }

        private static void FillCheckinsContainer(CheckinsContainer checkinsContainer, CheckinParser.CheckinParser checkinParser)
        {
            Checkin checkin = GetCheckin(checkinParser);
            checkin.VenuePurchase = GetPurchaseVenue(checkinsContainer, checkinParser);
            checkin.Venue = GetCheckinVenue(checkinsContainer, checkinParser);
            checkinsContainer.AddCheckin(checkin);

            Beer beer = checkinsContainer.GetBeer(checkinParser.GetBeerID());
            if (beer == null)
            {
                beer = GetBeer(checkinParser);
                checkinsContainer.AddBeer(beer);

                Brewery brewery = checkinsContainer.GetBrewery(checkinParser.GetBreweryID());
                if (brewery == null)
                {
                    brewery = GetBrewery(checkinParser);
                    brewery.Venue = GetBreweryVenue(checkinsContainer, checkinParser);
                    checkinsContainer.AddBrewery(brewery);
                }
                beer.Brewery = brewery;
            }
            checkin.Beer = beer;
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
            checkin.ServingType = checkinParser.GetServingType();
            checkin.TaggedFriends = checkinParser.GetTaggedFriends();
            checkin.TotalToasts = checkinParser.GetTotalToasts();
            checkin.TotalComments = checkinParser.GetTotalComments();

            return checkin;
        }

        private static Beer GetBeer(CheckinParser.CheckinParser checkinParser)
        {
            Beer beer = new Beer();

            beer.Name = checkinParser.GetBeerName();
            beer.Id = checkinParser.GetBeerID();
            beer.GlobalRatingScore = checkinParser.GetGlobalRatingScore();
            beer.GlobalWeightedRatingScore = checkinParser.GetGlobalWeightedRatingScore();
            beer.Url = checkinParser.GetBeerURL();
            beer.Type = checkinParser.GetBeerType();
            beer.ABV = checkinParser.GetBeerABV();
            beer.IBU = checkinParser.GetBeerIBU();

            return beer;
        }

        private static Brewery GetBrewery(CheckinParser.CheckinParser checkinParser)
        {
            Brewery brewery = new Brewery();

            brewery.Id = checkinParser.GetBreweryID();
            brewery.Name = checkinParser.GetBreweryName();
            brewery.Url = checkinParser.GetBreweryURL();

            return brewery;
        }

        private static Venue GetBreweryVenue(CheckinsContainer checkinsContainer, CheckinParser.CheckinParser checkinParser)
        {
            Venue venue = new Venue();
            venue.Country = checkinParser.GetBreweryCountry();
            venue.State = checkinParser.GetBreweryState();
            venue.City = checkinParser.GetBreweryCity();
            return GetVenue(venue, checkinsContainer);
        }

        private static Venue GetPurchaseVenue(CheckinsContainer checkinsContainer, CheckinParser.CheckinParser checkinParser)
        {
            Venue venue = new Venue();
            venue.Name = checkinParser.GetPurchaseVenue();
            return GetVenue(venue, checkinsContainer);
        }

        private static Venue GetCheckinVenue(CheckinsContainer checkinsContainer, CheckinParser.CheckinParser checkinParser)
        {
            Venue venue = new Venue();
            venue.Name = checkinParser.GetVenueName();
            venue.Country = checkinParser.GetVenueCountry();
            venue.State = checkinParser.GetVenueState();
            venue.City = checkinParser.GetVenueCity();
            venue.Latitude = checkinParser.GetVenueLat();
            venue.Longitude = checkinParser.GetVenueLng();
            return GetVenue(venue, checkinsContainer);
        }

        private static Venue GetVenue(Venue venue, CheckinsContainer checkinsContainer)
        {
            Venue existVenue = checkinsContainer.GetVenue(venue);
            if (existVenue != null)
                venue = existVenue;
            else
                checkinsContainer.AddVenue(venue);

            return venue;
        }
    }
}