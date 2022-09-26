using System.IO;
using UntappdViewer.Models;
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

        public static RatingViewModel GetCheckinViewModel(Checkin checkin, string photoPath)
        {
            RatingViewModel checkinViewModel = new RatingViewModel();
            checkinViewModel.Caption = checkin.Beer.Name;
            checkinViewModel.ImagePath = File.Exists(photoPath) ? photoPath : DefautlValues.NoImageIconResources;
            checkinViewModel.RatingScore = checkin.RatingScore ?? 0;
            return checkinViewModel;
        }

        public static RatingViewModel GetBeerViewModel(Beer beer, string labelPath)
        {
            RatingViewModel checkinViewModel = new RatingViewModel();
            checkinViewModel.Caption = beer.Name;
            checkinViewModel.ImagePath = labelPath;
            checkinViewModel.RatingScore = beer.GlobalRatingScore;
            return checkinViewModel;
        }
    }
}