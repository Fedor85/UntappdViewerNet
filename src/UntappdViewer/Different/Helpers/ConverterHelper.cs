using System;
using System.IO;
using System.Reflection;
using UntappdViewer.Models;
using UntappdViewer.Views.Controls.VewModel;

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

        public static CheckinViewModel GetCheckinViewModel(Checkin checkin, string photoPath)
        {
            CheckinViewModel checkinViewModel = new CheckinViewModel();
            checkinViewModel.BeerName = checkin.Beer.Name;
            checkinViewModel.PhotoPath = !String.IsNullOrEmpty(checkin.UrlPhoto)? photoPath : DefautlValues.NoImageIconResources;
            checkinViewModel.CheckinRating = checkin.RatingScore ?? 0;
            return checkinViewModel;
        }
    }
}