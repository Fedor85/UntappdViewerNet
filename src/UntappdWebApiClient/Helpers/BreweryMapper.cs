using Brewery = UntappdViewer.Models.Brewery;
using BreweryWeb = QuickType.Common.WebModels.Brewery;

namespace UntappdWebApiClient
{
    public static class BreweryMapper
    {
        public static void FillBrewery(Brewery brewery, BreweryWeb breweryWeb)
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
    }
}