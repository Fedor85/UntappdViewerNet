using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using FamFamFam.Flags.Wpf;
using Nager.Country;

namespace UntappdViewer.Different.Helpers
{
    public class CountryNameToFlagConverter : IValueConverter
    {
        private ICountryProvider countryProvider;

        private CountryIdToFlagImageSourceConverter converter;

        private List<List<string>> alternativeNames;

        public CountryNameToFlagConverter()
        {
            countryProvider = new CountryProvider();
            converter = new CountryIdToFlagImageSourceConverter();
            alternativeNames = GetAlternativeNames();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return DefaultValues.DefaultFlag;

            string country = value.ToString();

            ICountryInfo countryInfo = GetCountryInfo(country);
            if (countryInfo != null)
                country = countryInfo.Alpha2Code.ToString();

            object flag = converter.Convert(country, targetType, parameter, culture);
            return flag ?? DefaultValues.DefaultFlag;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }

        private ICountryInfo GetCountryInfo(string country)
        {
            ICountryInfo countryInfo = countryProvider.GetCountryByName(country);
            if (countryInfo != null)
                return countryInfo;

            List<string> names = alternativeNames.FirstOrDefault(item => item.Contains(country.ToLower()));
            if (names != null)
            {
                foreach (string name in names.Where(item => !item.Equals(country.ToLower())))
                {
                    countryInfo = countryProvider.GetCountryByName(name);
                    if (countryInfo != null)
                        return countryInfo;
                }
            }
            return null;
        }

        private List<List<string>> GetAlternativeNames()
        {
            List<List<string>> alternativeNames = new List<List<string>>();

            List<string> sr = new List<string>();
            sr.Add("surinam");
            sr.Add("suriname");
            alternativeNames.Add(sr);

            List<string> gb = new List<string>();
            gb.Add("united kingdom");
            gb.Add("england");
            alternativeNames.Add(gb);

            return alternativeNames;
        }
    }
}