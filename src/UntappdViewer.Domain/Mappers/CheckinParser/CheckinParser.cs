using System;
using System.Collections.Generic;
using System.Linq;
using UntappdViewer.Utils;

namespace UntappdViewer.Domain.Mappers.CheckinParser
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
            return GetValue<double>(ParameterNames.BeerABV);
        }

        public double? GetBeerIBU()
        {
            double ibu = GetValue<double>(ParameterNames.BeerIBU);
            return MathHelper.Doublecompare(ibu, 0) ? (double?) null : ibu;
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
            return GetDoubleNullableValue(ParameterNames.VenueLat);
        }

        public double? GetVenueLng()
        {
            return GetDoubleNullableValue(ParameterNames.VenueLng);
        }

        public double? GetRatingScore()
        {
            return GetDoubleNullableValue(ParameterNames.RatingScore);
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
            return StringHelper.GetValues(GetValue(ParameterNames.FlavorProfiles));
        }

        public string GetPurchaseVenue()
        {
            return GetValue(ParameterNames.PurchaseVenue);
        }

        public string GetServingType()
        {
            return GetValue(ParameterNames.ServingType);
        }

        public long GetCheckinID()
        {
            return GetValue<long>(ParameterNames.CheckinID);
        }

        public long GetBeerID()
        {
            return GetValue<long>(ParameterNames.BeerID);
        }

        public long GetBreweryID()
        {
            return GetValue<long>(ParameterNames.BreweryID);
        }

        public string GetPhotoURL()
        {
            return GetValue(ParameterNames.PhotoURL);
        }

        public double GetGlobalRatingScore()
        {
            return GetValue<double>(ParameterNames.GlobalRatingScore);
        }

        public double GetGlobalWeightedRatingScore()
        {
            return GetValue<double>(ParameterNames.GlobalWeightedRatingScore);
        }

        public string GetTaggedFriends()
        {
            return GetValue(ParameterNames.TaggedFriends);
        }

        public int GetTotalToasts()
        {
            return GetValue<int>(ParameterNames.TotalToasts);
        }

        public int GetTotalComments()
        {
            return GetValue<int>(ParameterNames.TotalComments);
        }

        private string GetValue(string name)
        {
            ParameterNumber parameterNumber = parameterNumbers.FirstOrDefault(i => i.Name.Equals(name));
            if (parameterNumber == null)
                return String.Empty;

            return parameterValues.First(i => i.Number == parameterNumber.Number).Value.Trim(Convert.ToChar(13)).Trim('"');
        }

        private T GetValue<T>(string name)
        {
            return ParserAndConvertHelper.GetConvertValue <T>(GetValue(name));
        }

        private double? GetDoubleNullableValue(string name)
        {
            string value = GetValue(name);
            return ParserAndConvertHelper.GetDoubleValue(value);
        }
    }
}