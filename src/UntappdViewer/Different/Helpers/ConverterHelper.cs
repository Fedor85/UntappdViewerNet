using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UntappdViewer.Domain;
using UntappdViewer.Infrastructure;
using UntappdViewer.Models;
using UntappdViewer.Models.Different;
using UntappdViewer.UI.Controls.Maps.BingMap.ViewModel;
using UntappdViewer.UI.Controls.RecyclerView.ViewModel;
using UntappdViewer.UI.Controls.ViewModel;
using UntappdViewer.Utils;

namespace UntappdViewer.Helpers
{
    public static class ConverterHelper
    {
        public static string GetServingTypeImagePath(string servingTypeName)
        {
            if (servingTypeName == null)
                return DefaultValues.EmptyImage;

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
                    return DefaultValues.EmptyImage;
            }
        }

        #region GalleryProject

        public static LongImageViewModel GetCheckinViewModel(Checkin checkin, string photoPath)
        {
            LongImageViewModel imageViewModel = new LongImageViewModel();
            imageViewModel.Signature = checkin.Beer.Name;
            imageViewModel.ImagePath = File.Exists(photoPath) ? photoPath : DefaultValues.NoImageIconResources;
            imageViewModel.RatingScore = checkin.RatingScore ?? 0;
            imageViewModel.ToolTip = StringHelper.GetSplitByLength(checkin.Beer.Name, DefaultValues.MaxToolTipLineLength);
            return imageViewModel;
        }

        public static RatingImageViewModel GetBeerViewModel(Beer beer, string labelPath)
        {
            RatingImageViewModel imageViewModel = new RatingImageViewModel();
            imageViewModel.Signature = beer.Name;
            imageViewModel.ImagePath = labelPath;
            imageViewModel.RatingScore = Math.Round(beer.GlobalRatingScore, 2);
            imageViewModel.ToolTip = StringHelper.GetSplitByLength(beer.Name, DefaultValues.MaxToolTipLineLength);
            return imageViewModel;
        }

        public static ImageViewModel GetBreweryViewModel(Brewery brewery, string labelPath)
        {
            ImageViewModel imageViewModel = new ImageViewModel();
            imageViewModel.Signature = brewery.Name;
            if (File.Exists(labelPath))
                imageViewModel.ImagePath = labelPath;

            StringBuilder description = new StringBuilder();
            description.AppendLine(StringHelper.GetSplitByLength(brewery.Name, DefaultValues.MaxToolTipLineLength));
            if (brewery.Venue != null && !String.IsNullOrEmpty(brewery.Venue.Country))
                description.Append(brewery.Venue.Country);

            imageViewModel.ToolTip = description.ToString().Trim();
            return imageViewModel;
        }


        public static EllipseImageViewModel GetBadgeViewModel(Badge badge, string imagePath)
        {
            EllipseImageViewModel captionImageViewModel = new EllipseImageViewModel();
            captionImageViewModel.Signature = badge.Name;
            captionImageViewModel.ImagePath = imagePath;

            StringBuilder description = new StringBuilder();
            description.AppendLine(badge.Name);
            if (!String.IsNullOrEmpty(badge.Description))
                description.Append(badge.Description);

            captionImageViewModel.ToolTip = StringHelper.GetSplitByLength(description.ToString().Trim(), DefaultValues.MaxToolTipLineLength);

            return captionImageViewModel;
        }

        public static List<LocationItem> GetLocationItems(List<KeyValueParam<long, List<string>>> venues)
        {
            List<LocationItem> locationItems = new List<LocationItem>();
            foreach (KeyValueParam<long, List<string>> keyValueParam in venues)
            {
                LocationItem locationItem = new LocationItem(keyValueParam.Parameters.Get<double>(ParameterNames.Latitude),
                                                             keyValueParam.Parameters.Get<double>(ParameterNames.Longitude));

                StringBuilder toolTip = new StringBuilder();
                if (keyValueParam.Parameters.Contains(ParameterNames.Name))
                    toolTip.AppendLine(keyValueParam.Parameters.Get<string>(ParameterNames.Name));

                toolTip.Append(String.Join("; ", keyValueParam.Value));

                if (keyValueParam.Parameters.Contains(ParameterNames.Count))
                    toolTip.Append($" ({keyValueParam.Parameters.Get<int>(ParameterNames.Count)})");

                locationItem.ToolTip = toolTip.ToString();
                locationItems.Add(locationItem);
            }
            return locationItems;
        }

        public static Dictionary<T1, T3> KeyValueToDirectory<T1, T2, T3>(List<KeyValue<T1, T2>> values)
        {
            Dictionary<T1, T3> dictionary = new Dictionary<T1, T3>();
            foreach (KeyValue<T1, T2> value in values)
            {
                if (!dictionary.ContainsKey(value.Key))
                    dictionary.Add(value.Key, ParserAndConvertHelper.GetConvertValue<T3>(value.Value));
            }
            return dictionary;
        }

        #endregion

        #region CountryNameToCode

        public static Dictionary<string, T> ConvertCountryNameToCode<T>(Dictionary<string, T> countries)
        {
            Dictionary<string, T> countryCodes = new Dictionary<string, T>();

            foreach (KeyValuePair<string, T> country in countries)
                countryCodes.Add(GetCountryCode(country.Key), country.Value);

            return countryCodes;
        }

        public static Dictionary<string, string> GetCountryNameByCode(List<string> countryNames)
        {
            Dictionary<string, string> countryNameByCode = new Dictionary<string, string>();

            foreach (string countryName in countryNames)
                countryNameByCode.Add(GetCountryCode(countryName), countryName);

            return countryNameByCode;
        }

        private static string GetCountryCode(string countryName)
        {
            string code = CountryNameHelper.GetCountryCode(countryName);
            return String.IsNullOrEmpty(code) ? countryName : code;
        }

        #endregion
    }
}