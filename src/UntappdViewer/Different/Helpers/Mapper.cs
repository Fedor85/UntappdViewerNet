using System;
using System.IO;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Models;
using UntappdViewer.Models.Different;
using UntappdViewer.UI.Controls.Maps.GMapNet.ViewModel;
using UntappdViewer.UI.Controls.ViewModel;
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
                if (IsValidVenue(checkin.Venue))
                    checkinViewModel.LocationItem = new LocationItem(checkin.Venue.Latitude.Value, checkin.Venue.Longitude.Value);
            }
            checkinViewModel.ServingType = ConverterHelper.GetServingTypeImagePath(checkin.ServingType);
            checkinViewModel.PhotoPath = GetCheckinPhoto(untappdService, checkin);
            FillBadges(untappdService, checkin, checkinViewModel);
            checkinViewModel.BeerViewModel = GetBeerViewModel(untappdService, checkin.Beer);
            return checkinViewModel;
        }

        private static BeerViewModel GetBeerViewModel(IUntappdService untappdService, Beer beer)
        {
            BeerViewModel beerViewModel = new BeerViewModel();
            beerViewModel.LabelPath = untappdService.GetBeerLabelFilePath(beer);
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
                BreweryViewModel breweryViewModel = GetBreweryViewModels(brewery);
                breweryViewModel.LabelPath = untappdService.GetBreweryLabelFilePath(brewery);
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
                if (IsValidVenue(brewery.Venue))
                    breweryViewModels.LocationItem = new LocationItem(brewery.Venue.Latitude.Value, brewery.Venue.Longitude.Value);

                breweryViewModels.IsNeedsUpdating = brewery.IsNeedsUpdating();
            }
            return breweryViewModels;
        }

        private static string GetCheckinHeader(DateTime? checkinCreatedDate)
        {
            return $"{Properties.Resources.Checkin}: {checkinCreatedDate}";
        }

        private static string GetCheckinPhoto(IUntappdService untappdService, Checkin checkin)
        {
            string photoPath = untappdService.GetCheckinPhotoFilePath(checkin);
            return !String.IsNullOrEmpty(photoPath) ? photoPath : DefaultValues.NoImageIconResources;
        }

        private static void FillBadges(IUntappdService untappdService, Checkin checkin, CheckinViewModel checkinViewModel)
        {
            foreach (Badge badge in checkin.Badges)
            {
                string badgeImagePath = untappdService.GetBadgeImageFilePath(badge);
                if (!String.IsNullOrEmpty(badgeImagePath) && File.Exists(badgeImagePath))
                {
                    ImageViewModel imageViewModel = new ImageViewModel();
                    imageViewModel.ImagePath = badgeImagePath;
                    imageViewModel.ToolTip = $"{badge.Name}\n{StringHelper.GetSplitByLength(badge.Description, DefaultValues.MaxToolTipLineLength)}";
                    checkinViewModel.Badges.Add(imageViewModel);
                }
            }
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
            return StringHelper.GetSplitByLength(description, DefaultValues.MaxToolTipLineLength);
        }

        private static bool IsValidVenue(Venue venue)
        {
            return venue.IsValidLocation() && !MathHelper.DoubleCompare(venue.Latitude.Value, 0) && !MathHelper.DoubleCompare(venue.Longitude.Value, 0);
        }
    }
}