using System.Windows.Media.Imaging;
using UntappdViewer.Models;
using UntappdViewer.Views.Controls.ViewModels;

namespace UntappdViewer.Helpers
{
    public static class Mapper
    {
        public static BreweryViewModel GetBreweryViewModels(Brewery brewery, BitmapSource breweryLabel)
        {
            BreweryViewModel breweryViewModels = new BreweryViewModel();
            breweryViewModels.BreweryUrl = brewery.Url;
            breweryViewModels.BreweryName = brewery.Name;
            breweryViewModels.BreweryLabel = breweryLabel;
            if (brewery.Venue != null)
            {
                breweryViewModels.BreweryVenueCountryFlag = brewery.Venue.Country;
                breweryViewModels.AddVenue(brewery.Venue.Name);
                breweryViewModels.AddVenue(brewery.Venue.Country);
                breweryViewModels.AddVenue(brewery.Venue.State);
                breweryViewModels.AddVenue(brewery.Venue.City);
            }
            return breweryViewModels;
        }
    }
}