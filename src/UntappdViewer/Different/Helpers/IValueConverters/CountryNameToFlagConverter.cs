using System;
using System.Globalization;
using System.Windows.Data;
using FamFamFam.Flags.Wpf;
using UntappdViewer.Infrastructure;

namespace UntappdViewer.Different.Helpers
{
    public class CountryNameToFlagConverter : IValueConverter
    {
        private CountryIdToFlagImageSourceConverter converter;

        public CountryNameToFlagConverter()
        {
            converter = new CountryIdToFlagImageSourceConverter();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            string countryCode = CountryNameHelper.GetCountryCode(value.ToString());
            object flag = converter.Convert(countryCode, targetType, parameter, culture);
            return flag ?? DefaultValues.DefaultFlag;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }
    }
}