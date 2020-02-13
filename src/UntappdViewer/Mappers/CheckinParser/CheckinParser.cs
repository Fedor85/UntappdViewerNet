using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UntappdViewer.Utils;

namespace UntappdViewer.Mappers.CheckinParser
{
    public class CheckinParser
    {
        private List<ParameterNumber> parameterNumbers;

        private List<ParameterValue> parameterValues;

        public CheckinParser(List<ParameterNumber> parameterNumbers, List<ParameterValue> parameterValues)
        {
            if (parameterNumbers.Count != parameterValues.Count)
                throw new ArgumentException(String.Format(Properties.Resources.ArgumentExceptionOnCheckinParser,"\n", parameterNumbers.Count, parameterValues.Count,
                                                            StringHelper.Join(parameterNumbers), StringHelper.Join(parameterValues)));

            this.parameterNumbers = parameterNumbers;
            this.parameterValues = parameterValues;
        }

        public string GetBeerName()
        {
            return GetValue(ParameterNames.BeerName);
        }

        public string GetBreweryName()
        {
            return GetValue(ParameterNames.BreweryName);
        }

        public string GetBeerType()
        {
            return GetValue(ParameterNames.BeerType);
        }

        public double GetBeerABV()
        {
            return GetDoubleValue(ParameterNames.BeerABV).Value;
        }

        public double GetBeerIBU()
        {
            return GetDoubleValue(ParameterNames.BeerIBU).Value;
        }

        public string GetComment()
        {
            return GetValue(ParameterNames.Comment);
        }

        public string GetVenueName()
        {
            return GetValue(ParameterNames.VenueName);
        }

        public string GetVenueCity()
        {
            return GetValue(ParameterNames.VenueCity);
        }

        public string GeVenueState()
        {
            return GetValue(ParameterNames.VenueState);
        }

        public string GeVenueCountry()
        {
            return GetValue(ParameterNames.VenueCountry);
        }

        public string GeVenueLat()
        {
            return GetValue(ParameterNames.VenueLat);
        }

        public string GeVenueLng()
        {
            return GetValue(ParameterNames.VenueLng);
        }

        public double? GetRatingScore()
        {
            return GetDoubleValue(ParameterNames.RatingScore);
        }

        public DateTime GetCreatedData()
        {
            return DateTime.Parse(GetValue(ParameterNames.CreatedData));
        }

        public string GeCheckinURL()
        {
            return GetValue(ParameterNames.CheckinURL);
        }

        public string GeBeerURL()
        {
            return GetValue(ParameterNames.BeerURL);
        }

        public string GeBreweryURL()
        {
            return GetValue(ParameterNames.BreweryURL);
        }

        public string GeBreweryCountry()
        {
            return GetValue(ParameterNames.BreweryCountry);
        }
        public string GeBreweryCity()
        {
            return GetValue(ParameterNames.BreweryCity);
        }
        public string GeBreweryState()
        {
            return GetValue(ParameterNames.BreweryState);
        }
        public List<string> GeFlavorProfiles()
        {
            return GetValues(GetValue(ParameterNames.FlavorProfiles));
        }

        public string GePurchaseVenues()
        {
            return GetValue(ParameterNames.PurchaseVenue);
        }

        public string GeServingType()
        {
            return GetValue(ParameterNames.ServingType);
        }

        public long GetCheckinID()
        {
            return Convert.ToInt64(GetValue(ParameterNames.CheckinID));
        }

        public long GetBeerID()
        {
            return Convert.ToInt64(GetValue(ParameterNames.BeerID));
        }

        public long GetBreweryID()
        {
            return Convert.ToInt64(GetValue(ParameterNames.BreweryID));
        }

        public string GePhotoURL()
        {
            return GetValue(ParameterNames.PhotoURL);
        }

        private string GetValue(string name)
        {
            ParameterNumber parameterNumber = parameterNumbers.First(i => i.Name.Equals(name));
            if (parameterNumber == null)
                return String.Empty;

            return parameterValues.First(i => i.Number == parameterNumber.Number).Value.Trim(Convert.ToChar(13)).Trim('"');
        }

        private double? GetDoubleValue(string name)
        {
            string value = GetValue(name);
            if (String.IsNullOrEmpty(value))
                return null;

            return Convert.ToDouble(NormalizeDecimalSeparator(value, System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator, ",."));
        }

        private static string NormalizeDecimalSeparator(string str, string requiredSeparator, string possibleSeparators)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char ch in str)
                if (possibleSeparators.IndexOf(ch) != -1)
                    sb.Append(requiredSeparator);
                else
                    sb.Append(ch);

            return sb.ToString();
        }

        private List<string> GetValues(string valueLine)
        {
            if (valueLine == null || String.IsNullOrEmpty(valueLine.Trim()))
                return new List<string>();

            return valueLine.Split(',').Select(item => item.Trim()).ToList();
        }
    }
}
