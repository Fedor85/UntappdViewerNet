using System;
using System.Collections.Generic;
using System.Linq;
using Nager.Country;

namespace UntappdViewer.Infrastructure
{
    public static class CountryNameHelper
    {
        private static readonly ICountryProvider countryProvider = new CountryProvider();

        private static readonly List<List<string>> alternativeNames = GetAlternativeNames();

        public static string GetCountryCode(string countryName)
        {
            if (String.IsNullOrEmpty(countryName))
                return String.Empty;

            ICountryInfo countryInfo = GetCountryInfo(countryName);
           return countryInfo != null ? countryInfo.Alpha2Code.ToString() : countryName;

        }

        private static ICountryInfo GetCountryInfo(string country)
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

        private static List<List<string>> GetAlternativeNames()
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
