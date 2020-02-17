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

        public string GetVenueState()
        {
            return GetValue(ParameterNames.VenueState);
        }

        public string GetVenueCountry()
        {
            return GetValue(ParameterNames.VenueCountry);
        }

        public double? GetVenueLat()
        {
            return GetDoubleValue(ParameterNames.VenueLat);
        }

        public double? GetVenueLng()
        {
            return GetDoubleValue(ParameterNames.VenueLng);
        }

        public double? GetRatingScore()
        {
            return GetDoubleValue(ParameterNames.RatingScore);
        }

        public DateTime GetCreatedData()
        {
            return DateTime.Parse(GetValue(ParameterNames.CreatedData));
        }

        public string GetCheckinURL()
        {
            return GetValue(ParameterNames.CheckinURL);
        }

        public string GetBeerURL()
        {
            return GetValue(ParameterNames.BeerURL);
        }

        public string GetBreweryURL()
        {
            return GetValue(ParameterNames.BreweryURL);
        }

        public string GetBreweryCountry()
        {
            return GetValue(ParameterNames.BreweryCountry);
        }
        public string GetBreweryCity()
        {
            return GetValue(ParameterNames.BreweryCity);
        }
        public string GetBreweryState()
        {
            return GetValue(ParameterNames.BreweryState);
        }
        public List<string> GetFlavorProfiles()
        {
            return GetValues(GetValue(ParameterNames.FlavorProfiles));
        }

        public string GetPurchaseVenues()
        {
            return GetValue(ParameterNames.PurchaseVenue);
        }

        public string GetServingType()
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

        public string GetPhotoURL()
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

            return Convert.ToDouble(StringHelper.GetNormalizeDecimalSeparator(value, System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator, ",."));
        }

        private List<string> GetValues(string valueLine)
        {
            if (valueLine == null || String.IsNullOrEmpty(valueLine.Trim()))
                return new List<string>();

            return valueLine.Split(',').Select(item => item.Trim()).ToList();
        }
    }
}
