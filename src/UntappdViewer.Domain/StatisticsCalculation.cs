using System;
using System.Collections.Generic;
using System.Linq;
using UntappdViewer.Models;
using UntappdViewer.Models.Different;
using UntappdViewer.Utils;

namespace UntappdViewer.Domain
{
    public static class StatisticsCalculation
    {
        private const int DefautDay = 1;

        public static int GetCountrysCount(List<Checkin> checkins)
        {
            return GetCountrys(checkins).Count;
        }

        public static List<KeyValue<double, int>> GetChekinsRatingByCount(List<Checkin> checkins)
        {
            List<KeyValue<double, int>> keyValues = new List<KeyValue<double, int>>();

            IEnumerable<Checkin> chekinRating = checkins.Where(item => item.RatingScore.HasValue);
            if (!chekinRating.Any())
                return keyValues;

            List<double> ratings = chekinRating.Select(item => item.RatingScore.Value).Distinct().ToList();
            ratings.Sort();

            foreach (double rating in ratings)
                keyValues.Add(new KeyValue<double, int>(rating, chekinRating.Count(item => MathHelper.DoubleCompare(item.RatingScore.Value, rating))));

            return keyValues;
        }

        public static List<KeyValue<double, int>> GetBeersRatingByCount(List<Beer> beers)
        {
            List<KeyValue<double, int>> keyValues = new List<KeyValue<double, int>>();
            if (!beers.Any())
                return keyValues;

            Dictionary<long, double> beerRating = GetBeerIdByRoundRating(beers);
            List<double> ratings = beerRating.Select(item => item.Value).Distinct().ToList();
            ratings.Sort();

            foreach (double rating in ratings)
                keyValues.Add(new KeyValue<double, int>(rating, beerRating.Count(item => MathHelper.DoubleCompare(item.Value, rating))));

            return keyValues;
        }

        public static List<KeyValue<string, int>> GetDateChekinsByCount(List<Checkin> checkins)
        {
            List<KeyValue<DateTime, int>> dates = new List<KeyValue<DateTime, int>>();
            if (!checkins.Any())
                return new List<KeyValue<string, int>>();

            foreach (Checkin checkin in checkins.OrderBy(item => item.CreatedDate).ToList())
            {
                DateTime date = new DateTime(checkin.CreatedDate.Year, checkin.CreatedDate.Month, 1);
                KeyValue<DateTime, int> dataChekinCount = dates.FirstOrDefault(item => item.Key.Equals(date));
                if (dataChekinCount == null)
                    dates.Add(new KeyValue<DateTime, int>(date, DefautDay));
                else
                    dataChekinCount.Value++;
            }

            dates = InsertEmptyMonth(dates);
            return dates.Select(keyValue => new KeyValue<string, int>($"{keyValue.Key:MM}.{keyValue.Key:yy}", keyValue.Value)).ToList();
        }

        public static List<KeyValue<string, List<long>>> GetBeerTypesByCheckinIdsGroupByCount(List<Checkin> checkins)
        {
            List<KeyValue<string, List<long>>> keyValues = new List<KeyValue<string, List<long>>>();
            if (!checkins.Any())
                return keyValues;

            List<string> types = checkins.Select(item => item.Beer.Type).Distinct().ToList();
            types.Sort();

            foreach (KeyValuePair<string, List<string>> keyValuePair in StringHelper.GetGroupByList(types, DefaultValues.SeparatorsName))
            {
                List<long> checkinIds = checkins.Where(item => keyValuePair.Value.Contains(item.Beer.Type)).Select(checkin => checkin.Id).ToList();
                keyValues.Add(new KeyValue<string, List<long>>(keyValuePair.Key, checkinIds));
            }

            KeyValue<string, List<long>> other = keyValues.FirstOrDefault(item => item.Key.Equals(DefaultValues.OtherNameGroupBeerType));
            if (other == null)
                other = new KeyValue<string, List<long>>(DefaultValues.OtherNameGroupBeerType, new List<long>());
            else
                keyValues.Remove(other);


            List<string> removeKeys = new List<string>();
            foreach (KeyValue<string, List<long>> keyValue in keyValues.Where(item => item.Value.Count <= DefaultValues.BeerTypeCountByOther))
            {
                removeKeys.Add(keyValue.Key);
                other.Value.AddRange(keyValue.Value);
            }

            keyValues.RemoveAll(item => removeKeys.Contains(item.Key));

            if (other.Value.Count > 0)
                keyValues.Add(other);

            keyValues.Reverse();
            return keyValues;
        }

        public static List<KeyValue<string, List<long>>> GetCountrysByCheckinIds(List<Checkin> checkins)
        {
            List<KeyValue<string, List<long>>> keyValues = new List<KeyValue<string, List<long>>>();
            List<string> countrys = GetCountrys(checkins);
            if (!countrys.Any())
                return keyValues;

            countrys.Sort();
            countrys.Reverse();

            foreach (string country in countrys)
            {
                List<long> checkinIds = checkins.Where(item => item.Beer.Brewery.Venue.Country.Equals(country)).Select(checkin => checkin.Id).ToList();
                keyValues.Add(new KeyValue<string, List<long>>(StringHelper.GetCutByFirstChars(country, DefaultValues.SeparatorsName), checkinIds));
            }

            return keyValues;
        }

        public static List<KeyValue<string, List<long>>> GetServingTypeByCheckinIds(List<Checkin> checkins, string defaultServingType)
        {
            List<KeyValue<string, List<long>>> keyValues = new List<KeyValue<string, List<long>>>();
            if (!checkins.Any())
                return keyValues;

            KeyValue<string, List<long>> checkinDefaultServingType = new KeyValue<string, List<long>>(defaultServingType, 
                                                                        checkins.Where(item => String.IsNullOrEmpty(item.ServingType)).Select(item=> item.Id).ToList());

            IEnumerable<Checkin> validCheckins = checkins.Where(item => !String.IsNullOrEmpty(item.ServingType));
            List<string> types = validCheckins.Select(item => item.ServingType).Distinct().ToList();
            types.Sort();
            types.Reverse();

            foreach (string type in types)
            {
                List<long> checkinIds = validCheckins.Where(item => item.ServingType.Equals(type)).Select(checkin => checkin.Id).ToList();
                if (type.Equals(defaultServingType))
                    checkinDefaultServingType.Value.AddRange(checkinIds);
                else
                    keyValues.Add(new KeyValue<string, List<long>>(type, checkinIds));
            }

            if (checkinDefaultServingType.Value.Count > 0)
                keyValues.Insert(0, checkinDefaultServingType);

            return keyValues;
        }

        public static List<KeyValue<string, int>> GetListCount(List<KeyValue<string, List<long>>> items)
        {
            List<KeyValue<string, int>> keyValues = new List<KeyValue<string, int>>();

            foreach (KeyValue<string, List<long>> item in items)
                keyValues.Add(new KeyValue<string, int>(item.Key, item.Value.Count));

            return keyValues;
        }

        public static List<KeyValue<string, int>> GetAccumulateValues(List<KeyValue<string, int>> items)
        {
            List<KeyValue<string, int>> keyValues = new List<KeyValue<string, int>>();
            if (!items.Any())
                return keyValues;

            KeyValue<string, int> firstItem = items[0];
            keyValues.Add(new KeyValue<string, int>(firstItem.Key, firstItem.Value));
            for (int i = 1; i < items.Count; i++)
            {
                KeyValue<string, int> previousItem = keyValues[i -1];
                KeyValue<string, int> currentItem = items[i];
                keyValues.Add(new KeyValue<string, int>(currentItem.Key, currentItem.Value + previousItem.Value));
            }
            return keyValues;
        }

        public static List<KeyValue<string, double>> GetAverageRatingByCheckinIds(List<Checkin> checkins, List<KeyValue<string, List<long>>> checkinIds)
        {
            List<KeyValue<string, double>> keyValues = new List<KeyValue<string, double>>();

            foreach (KeyValue<string, List<long>> keyValue in checkinIds)
            {
                int count = checkins.Count(item => keyValue.Value.Contains(item.Id) && item.RatingScore.HasValue);
                double sumRating = checkins.Where(item => keyValue.Value.Contains(item.Id) && item.RatingScore.HasValue).Sum(checkin => checkin.RatingScore.Value);
                double rating = sumRating / (count > 0 ? count : 1);
                keyValues.Add(new KeyValue<string, double>(keyValue.Key, Math.Round(rating, 2)));
            }

            return keyValues;
        }

        public static List<KeyValue<double, double>> GetABVToIBU(List<Beer> beers)
        {
            List<KeyValue<double, double>> keyValues = new List<KeyValue<double, double>>();
            foreach (Beer beer in beers.Where(beer => !MathHelper.DoubleCompare(beer.ABV, 0) && beer.IBU.HasValue && !MathHelper.DoubleCompare(beer.IBU.Value, 0)))
            {
                KeyValue<double, double> keyValue = new KeyValue<double, double>(beer.ABV, beer.IBU.Value);
                if (!keyValues.Contains(keyValue))
                    keyValues.Add(keyValue);
            }
            return keyValues;
        }

        public static List<KeyValueParam<string, int>> GetRangeABVByCount(List<Beer> beers, double range, double maxValue)
        {
            if (!beers.Any())
                return new List<KeyValueParam<string, int>>();

            List<double> abvs = beers.Select(item => MathHelper.GetCeilingByStep(item.ABV, range)).Distinct().ToList();
            abvs.Sort();

            List<KeyValue<double, int>> abvCount = new List<KeyValue<double, int>>();
            foreach (double abv in abvs)
            {
                int currentABVCount = beers.Count(item => MathHelper.DoubleCompare(abv, MathHelper.GetCeilingByStep(item.ABV, range)));
                abvCount.Add(new KeyValue<double, int>(abv, currentABVCount));
            }

            MergeByMore(abvCount, maxValue);
            return GetRangeNameToCount(abvCount);
        }

        public static List<KeyValueParam<string, int>> GetRangeIBUByCount(List<Beer> beers, double range, double maxValue)
        {
            IEnumerable<Beer> currentBeers = beers.Where(item => item.IBU.HasValue);
            if (!currentBeers.Any())
                return new List<KeyValueParam<string, int>>(); ;

            List<double> ibus = currentBeers.Select(item => MathHelper.GetCeilingByStep(item.IBU.Value, range)).Distinct().ToList();
            ibus.Sort();

            List<KeyValue<double, int>> ibuCount = new List<KeyValue<double, int>>();
            foreach (double ibu in ibus)
            {
                int currentIBUCount = currentBeers.Count(item => MathHelper.DoubleCompare(ibu, MathHelper.GetCeilingByStep(item.IBU.Value, range)));
                ibuCount.Add(new KeyValue<double, int>(ibu, currentIBUCount));
            }

            MergeByMore(ibuCount, maxValue);
            return GetRangeNameToCount(ibuCount);
        }
        private static void MergeByMore(List<KeyValue<double, int>> keyValues, double maxValue)
        {
            List<KeyValue<double, int>> moreCount = keyValues.Where(item => item.Key > maxValue).ToList();
            if (moreCount.Count != keyValues.Count)
            {
                int index = keyValues.Count - moreCount.Count - 1;
                keyValues[index].Value += moreCount.Select(item => item.Value).Sum();
                keyValues.RemoveRange(index + 1, moreCount.Count());
            }
        }

        private static List<KeyValueParam<string, int>> GetRangeNameToCount(List<KeyValue<double, int>> items)
        {
            List<KeyValueParam<string, int>> keyValues = new List<KeyValueParam<string, int>>();
            int count = items.Count;
            for (int i = 0; i < count; i++)
            {
                string prefix = i == items.Count - 1 ? ">" : String.Empty;
                string start = i == 0 ? "0" : items[i - 1].Key.ToString();
                string end = i != items.Count - 1 ? $"-{items[i].Key}" : String.Empty;
                KeyValueParam<string, int> keyValue = new KeyValueParam<string, int>($"{prefix}{start}{end}", items[i].Value);
                keyValue.Parameters.Add(ParameterNames.Index, i);
                keyValue.Parameters.Add(ParameterNames.Count, count);
                keyValues.Add(keyValue);
            }
            return keyValues;
        }

        private static List<KeyValue<DateTime, int>> InsertEmptyMonth(List<KeyValue<DateTime, int>> dates)
        {
            if (!dates.Any())
                return dates;

            List<KeyValue<DateTime, int>> sortDates = dates.OrderBy(item => item.Key.Year).ThenBy(item => item.Key.Month).ToList();
            DateTime nowDate = DateTime.Today;
            IEnumerable<IGrouping<int, KeyValue<DateTime, int>>> dateByYear = sortDates.GroupBy(item => item.Key.Year);
            int minYear = dateByYear.Min(item => item.Key);
            foreach (IGrouping<int, KeyValue<DateTime, int>> keyValues in dateByYear)
            {
                int firstMonth = keyValues.Key == minYear ? keyValues.Min(item => item.Key.Month) : 1;
                int lastMonth = keyValues.Key == nowDate.Year ? nowDate.Month : 12;
                for (int month = firstMonth; month <= lastMonth; month++)
                {
                    DateTime currentDate = new DateTime(keyValues.Key, month, DefautDay);
                    if (!sortDates.Any(item => item.Key.Equals(currentDate)))
                        sortDates.Add(new KeyValue<DateTime, int>(currentDate, 0));
                }
            }
            return sortDates.OrderBy(item => item.Key.Year).ThenBy(item => item.Key.Month).ToList();
        }

        private static Dictionary<long, double> GetBeerIdByRoundRating(List<Beer> beers)
        {
            Dictionary<long, double> dictionary = new Dictionary<long, double>();
            foreach (Beer beer in beers)
                dictionary.Add(beer.Id, MathHelper.GetRoundByStep(beer.GlobalRatingScore, DefaultValues.StepRating));

            return dictionary;
        }

        private static List<string> GetCountrys(List<Checkin> checkins)
        {
            IEnumerable<Checkin> checkinCountry = checkins.Where(item => item.Beer.Brewery.Venue != null && !String.IsNullOrEmpty(item.Beer.Brewery.Venue.Country));
            if (!checkinCountry.Any())
                return new List<string>();

            return checkinCountry.Select(item => item.Beer.Brewery.Venue.Country).Distinct().ToList();
        }
    }
}