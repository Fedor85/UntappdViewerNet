using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UntappdViewer.Models;
using UntappdViewer.Utils;
using UntappdViewer.Views.Controls.ViewModel;

namespace UntappdViewer.Helpers
{
    public static class ConverterHelper
    {
        public static string GetServingTypeImagePath(string servingTypeName)
        {
            if (servingTypeName == null)
                return DefautlValues.EmptyImage;

            switch (servingTypeName.ToLower())
            {
                case "draft":
                    return @"..\Resources\ServingType\draft@3x.png";
                case "bottle":
                    return @"..\Resources\ServingType\bottle@3x.png";
                case "can":
                    return @"..\Resources\ServingType\can@3x.png";
                case "taster":
                    return @"..\Resources\ServingType\taster@3x.png";
                case "cask":
                    return @"..\Resources\ServingType\cask@3x.png";
                case "crowler":
                    return @"..\Resources\ServingType\crowler.png";
                case "growler":
                    return @"..\Resources\ServingType\growler.png";
                default:
                    return DefautlValues.EmptyImage;
            }
        }

        #region GalleryProject

        public static RatingViewModel GetCheckinViewModel(Checkin checkin, string photoPath)
        {
            RatingViewModel ratingViewModel = new RatingViewModel();
            ratingViewModel.Caption = checkin.Beer.Name;
            ratingViewModel.ImagePath = File.Exists(photoPath) ? photoPath : DefautlValues.NoImageIconResources;
            ratingViewModel.RatingScore = checkin.RatingScore ?? 0;
            return ratingViewModel;
        }

        public static RatingViewModel GetBeerViewModel(Beer beer, string labelPath)
        {
            RatingViewModel ratingViewModel = new RatingViewModel();
            ratingViewModel.Caption = beer.Name;
            if (File.Exists(labelPath))
                ratingViewModel.ImagePath = labelPath;

            ratingViewModel.RatingScore = beer.GlobalRatingScore;
            return ratingViewModel;
        }

        public static ImageViewModel GetBreweryViewModel(Brewery brewery, string labelPath)
        {
            ImageViewModel imageViewModel = new ImageViewModel();
            imageViewModel.Caption = brewery.Name;
            if (File.Exists(labelPath))
                imageViewModel.ImagePath = labelPath;

            if (brewery.Venue != null && !String.IsNullOrEmpty(brewery.Venue.Country))
                imageViewModel.Description = brewery.Venue.Country;

            return imageViewModel;
        }

        public static ImageViewModel GetBadgeViewModel(Badge badge, string imagePath)
        {
            ImageViewModel imageViewModel = new ImageViewModel();
            imageViewModel.Caption = badge.Name;
            if (File.Exists(imagePath))
                imageViewModel.ImagePath = imagePath;

            if (!String.IsNullOrEmpty(badge.Description))
                imageViewModel.Description = StringHelper.GetSplitByLength(badge.Description, 40);

            return imageViewModel;
        }

        #endregion

        #region StatisticsProject

        public static List<ChartViewModel<double, int>> GetChekinRatingScore(List<Checkin> checkins)
        {
            List<ChartViewModel<double, int>> ratingsViewModels = new List<ChartViewModel<double, int>>();

            IEnumerable<Checkin> chekinRating = checkins.Where(item => item.RatingScore.HasValue);
            if (!chekinRating.Any())
                return ratingsViewModels;

            List<double> ratings = chekinRating.Select(item => item.RatingScore.Value).Distinct().ToList();
            ratings.Sort();
            foreach (double rating in ratings)
                ratingsViewModels.Add(new ChartViewModel<double, int>(rating, chekinRating.Count(item => item.RatingScore.Value == rating)));

            return ratingsViewModels;
        }

        #endregion
    }
}