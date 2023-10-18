using System;
using System.Windows.Media.Imaging;
using UntappdViewer.Models;
using UntappdViewer.Utils;
using UntappdViewer.Views.Controls.ViewModels;

namespace UntappdViewer.Helpers
{
    public static class Mapper
    {
        public static BreweryViewModel GetBreweryViewModels(Brewery brewery, BitmapSource breweryLabel)
        {
            BreweryViewModel breweryViewModels = new BreweryViewModel();

            breweryViewModels.BreweryLabel = breweryLabel;
            breweryViewModels.BreweryUrl = brewery.Url;
            breweryViewModels.BreweryName = brewery.Name;
            
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

        public static BeerViewModel GetBeerViewModel(Beer beer, BitmapSource beerLabel)
        {
            BeerViewModel beerViewModel = new BeerViewModel();
            beerViewModel.BeerLabel = beerLabel;
            beerViewModel.BeerUrl = beer.Url;
            beerViewModel.BeerName = beer.Name;    
            beerViewModel.BeerType = beer.Type;
            beerViewModel.BeerABV = beer.ABV.ToString();
            beerViewModel.BeerIBU = beer.IBU?.ToString() ?? DefaultValues.NoIBU;
            beerViewModel.BeerRating = Math.Round(beer.GlobalRatingScore, 2);
            beerViewModel.BeerDescription = GetBeerDescription(beer.Description);
            return beerViewModel;
        }

        /// <summary>
        /// Если ToolTip String.Empty то отображается пустой
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static string GetBeerDescription(string text)
        {
            if (String.IsNullOrEmpty(text))
                return null;

            string description = StringHelper.GetRemoveEmptyLines(text);
            return StringHelper.GetSplitByLength(description, 50);
        }
    }
}