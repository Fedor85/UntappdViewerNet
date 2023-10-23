using System;
using System.IO;
using System.Windows.Media.Imaging;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Models;
using UntappdViewer.UI.Controls.ViewModel;
using UntappdViewer.UI.Helpers;
using UntappdViewer.Utils;
using UntappdViewer.ViewModels.Controls;

namespace UntappdViewer.Helpers
{
    public static class Mapper
    {

        public static CheckinViewModel GetCheckinViewModel(IUntappdService untappdService, Checkin checkin)
        {
            CheckinViewModel checkinViewModel = new CheckinViewModel();
            checkinViewModel.Header = GetCheckinHeader(checkin.CreatedDate);
            checkinViewModel.Url = checkin.Url;
            checkinViewModel.Rating = checkin.RatingScore;
            if(checkin.Venue != null)
            {
                checkinViewModel.AddVenue(checkin.Venue.Name);
                checkinViewModel.AddVenue(checkin.Venue.Country);
                checkinViewModel.AddVenue(checkin.Venue.State);
                checkinViewModel.AddVenue(checkin.Venue.City);
            }
            checkinViewModel.ServingType = ConverterHelper.GetServingTypeImagePath(checkin.ServingType);
            checkinViewModel.Photo = GetCheckinPhoto(untappdService, checkin);
            FillBadges(untappdService, checkin, checkinViewModel);
            checkinViewModel.BeerViewModel = GetBeerViewModel(untappdService, checkin.Beer);
            return checkinViewModel;
        }

        private static BeerViewModel GetBeerViewModel(IUntappdService untappdService, Beer beer)
        {
            BeerViewModel beerViewModel = new BeerViewModel();
            beerViewModel.Label = GetBeerLabel(untappdService, beer);
            beerViewModel.Url = beer.Url;
            beerViewModel.Name = beer.Name;
            beerViewModel.Type = beer.Type;
            beerViewModel.ABV = beer.ABV.ToString();
            beerViewModel.IBU = beer.IBU?.ToString() ?? DefaultValues.NoIBU;
            beerViewModel.Rating = Math.Round(beer.GlobalRatingScore, 2);
            beerViewModel.Description = GetBeerDescription(beer.Description);
            FillBreweryViewModels(untappdService, beer, beerViewModel);
            return beerViewModel;
        }

        private static void FillBreweryViewModels(IUntappdService untappdService, Beer beer, BeerViewModel beerViewModel)
        {
            foreach (Brewery brewery in beer.GetFullBreweries())
            {
                BreweryViewModel breweryViewModel = Mapper.GetBreweryViewModels(brewery);
                breweryViewModel.Label = GetBreweryLabel(untappdService, brewery);
                beerViewModel.BreweryViewModels.Add(breweryViewModel);
            }
        }

        private static BreweryViewModel GetBreweryViewModels(Brewery brewery)
        {
            BreweryViewModel breweryViewModels = new BreweryViewModel();
            breweryViewModels.Url = brewery.Url;
            breweryViewModels.Name = brewery.Name;
            
            if (brewery.Venue != null)
            {
                breweryViewModels.VenueCountryFlag = brewery.Venue.Country;
                breweryViewModels.AddVenue(brewery.Venue.Name);
                breweryViewModels.AddVenue(brewery.Venue.Country);
                breweryViewModels.AddVenue(brewery.Venue.State);
                breweryViewModels.AddVenue(brewery.Venue.City);
            }
            return breweryViewModels;
        }

        private static string GetCheckinHeader(DateTime? checkinCreatedDate)
        {
            return $"{Properties.Resources.Checkin}: {checkinCreatedDate}";
        }

        private static BitmapSource GetCheckinPhoto(IUntappdService untappdService, Checkin checkin)
        {
            string photoPath = untappdService.GetCheckinPhotoFilePath(checkin);

            return String.IsNullOrEmpty(photoPath) ? ImageConverter.GetBitmapSource(Properties.Resources.no_image_icon) :
                                                     ImageConverter.GetBitmapSource(photoPath);
        }
        private static void FillBadges(IUntappdService untappdService, Checkin checkin, CheckinViewModel checkinViewModel)
        {
            foreach (Badge badge in checkin.Badges)
            {
                string badgeImagePath = untappdService.GetBadgeImageFilePath(badge);
                if (!String.IsNullOrEmpty(badgeImagePath))
                    checkinViewModel.Badges.Add(new ImageItemViewModel(badgeImagePath, $"{badge.Name}\n{badge.Description}"));
            }
        }

        private static BitmapSource GetBeerLabel(IUntappdService untappdService, Beer beer)
        {
            string labelPath = untappdService.GetBeerLabelFilePath(beer);
            return !String.IsNullOrEmpty(labelPath) && !Path.GetFileNameWithoutExtension(labelPath).Equals(DefaultValues.DefaultBeerLabelName) ?
                    ImageConverter.GetBitmapSource(labelPath) : null;
        }

        private static BitmapSource GetBreweryLabel(IUntappdService untappdService, Brewery brewery)
        {
            string labelPath = untappdService.GetBreweryLabelFilePath(brewery);
            return !String.IsNullOrEmpty(labelPath) && !Path.GetFileNameWithoutExtension(labelPath).Equals(DefaultValues.DefaultBreweryLabelName) ?
                ImageConverter.GetBitmapSource(labelPath) : null;
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