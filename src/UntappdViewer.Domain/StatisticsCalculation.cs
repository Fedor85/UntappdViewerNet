using System;
using System.Collections.Generic;
using System.Linq;
using UntappdViewer.Domain.Models;
using UntappdViewer.Models;
using UntappdViewer.Utils;

namespace UntappdViewer.Domain
{
    public static class StatisticsCalculation
    {
        private const int DefautDay = 1;

        public static List<KeyValue<double, int>> GetChekinRatingScore(List<Checkin> checkins)
        {
            List<KeyValue<double, int>> ratingsViewModels = new List<KeyValue<double, int>>();

            IEnumerable<Checkin> chekinRating = checkins.Where(item => item.RatingScore.HasValue);
            if (!chekinRating.Any())
                return ratingsViewModels;

            List<double> ratings = chekinRating.Select(item => item.RatingScore.Value).Distinct().ToList();
            ratings.Sort();
            foreach (double rating in ratings)
                ratingsViewModels.Add(new KeyValue<double, int>(rating, chekinRating.Count(item => MathHelper.DoubleCompare(item.RatingScore.Value, rating))));

            return ratingsViewModels;
        }

        public static List<KeyValue<double, int>> GetBeerRatingScore(List<Beer> beers)
        {
            List<KeyValue<double, int>> keyValues = new List<KeyValue<double, int>>();
            if (!beers.Any())
                return keyValues;

            Dictionary<long, double> dictionaryBeerRating = GetBeerByRoundRating(beers);
            List<double> ratings = dictionaryBeerRating.Select(item => item.Value).Distinct().ToList();
            ratings.Sort();
            foreach (double rating in ratings)
                keyValues.Add(new KeyValue<double, int>(rating, dictionaryBeerRating.Count(item => MathHelper.DoubleCompare(item.Value, rating))));

            return keyValues;
        }

        public static List<KeyValue<string, int>> GetListCount(List<KeyValue<string, List<long>>> beerTypeCheckinIds)
        {
            List<KeyValue<string, int>> keyValues = new List<KeyValue<string, int>>();

            foreach (KeyValue<string, List<long>> keyValue in beerTypeCheckinIds)
                keyValues.Add(new KeyValue<string, int>(keyValue.Key, keyValue.Value.Count));

            return keyValues;
        }

        public static List<KeyValue<string, double>> GetCheckinRatingByIds(List<Checkin> checkins, List<KeyValue<string, List<long>>> beerTypeCheckinIds)
        {
            List<KeyValue<string, double>> keyValues = new List<KeyValue<string, double>>();

            foreach (KeyValue<string, List<long>> keyValue in beerTypeCheckinIds)
            {
                int count = checkins.Count(item => keyValue.Value.Contains(item.Id) && item.RatingScore.HasValue);
                double sumRating = checkins.Where(item => keyValue.Value.Contains(item.Id) && item.RatingScore.HasValue).Sum(checkin => checkin.RatingScore.Value);
                double rating = sumRating / (count > 0 ? count : 1);
                keyValues.Add(new KeyValue<string, double>(keyValue.Key, Math.Round(rating, 2)));
            }

            return keyValues;
        }

        public static List<KeyValue<string, List<long>>> GetBeerTypeCheckinIdGroupByCount(List<Checkin> checkins)
        {
            List<KeyValue<string, List<long>>> dictionary = new List<KeyValue<string, List<long>>>();
            if (!checkins.Any())
                return dictionary;

            List<string> types = checkins.Select(item => item.Beer.Type).Distinct().ToList();
            types.Sort();

            Dictionary<string, List<string>> groupTypes = StringHelper.GetGroupByList(types, DefautlValues.SeparatorsName);

            foreach (KeyValuePair<string, List<string>> keyValuePair in groupTypes)
            {
                List<long> checkinIds = checkins.Where(item => keyValuePair.Value.Contains(item.Beer.Type)).Select(checkin => checkin.Id).ToList();
                dictionary.Add(new KeyValue<string, List<long>>(keyValuePair.Key, checkinIds));               
            }

            KeyValue<string, List<long>> other = dictionary.FirstOrDefault(item => item.Key.Equals(DefautlValues.OtherNameGroupBeerType));
            if (other == null)
                other = new KeyValue<string, List<long>>(DefautlValues.OtherNameGroupBeerType, new List<long>());
            else
                dictionary.Remove(other);


            List<string> removeKeys = new List<string>();
            foreach (KeyValue<string, List<long>> keyValue in dictionary.Where(item => item.Value.Count <= DefautlValues.BeerTypeCountByOther))
            {
                removeKeys.Add(keyValue.Key);
                other.Value.AddRange(keyValue.Value);
            }

            dictionary.RemoveAll(item => removeKeys.Contains(item.Key));

            if (other.Value.Count > 0)
                dictionary.Add(other);

            dictionary.Reverse();
            return dictionary;
        }

        public static List<KeyValue<string, List<long>>> GetCountryCheckinIds(List<Checkin> checkins)
        {
            List<KeyValue<string, List<long>>> dictionary = new List<KeyValue<string, List<long>>>();
            IEnumerable<Checkin> checkinCountry = checkins.Where(item => item.Beer.Brewery.Venue != null && !String.IsNullOrEmpty(item.Beer.Brewery.Venue.Country));
            if (!checkinCountry.Any())
                return dictionary;

            List<string> countrys = checkinCountry.Select(item => item.Beer.Brewery.Venue.Country).Distinct().ToList();
            countrys.Sort();
            countrys.Reverse();

            foreach (string country in countrys)
            {
                List<long> checkinIds = checkins.Where(item => item.Beer.Brewery.Venue.Country.Equals(country)).Select(checkin => checkin.Id).ToList();
                dictionary.Add(new KeyValue<string, List<long>>(StringHelper.GetCutByFirstChars(country, DefautlValues.SeparatorsName), checkinIds));
            }

            return dictionary;
        }

        public static int GetTotalDays(List<Checkin> checkins)
        {
            if (!checkins.Any())
                return 0;

            DateTime minData = checkins.Min(item => item.CreatedDate);
            return Convert.ToInt32(Math.Ceiling((DateTime.Now - minData).TotalDays));
        }

        public static double GetAverageQuantity(List<Checkin> checkins)
        {
            int totalDays = GetTotalDays(checkins);
            int count = checkins.Count;
            return totalDays == 0 ? count : count / Convert.ToDouble(totalDays);
        }

        public static List<KeyValue<string, int>> GetDateChekins(List<Checkin> checkins)
        {
            List<KeyValue<DateTime, int>> dates = new List<KeyValue<DateTime, int>>();
            if (!checkins.Any())
                return null;

            List<Checkin> newCheckins = new List<Checkin>(checkins);
            newCheckins.Reverse();
            foreach (Checkin checkin in newCheckins)
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
                    if (sortDates.Any(item => item.Key.Equals(currentDate)))
                        continue;

                    sortDates.Add(new KeyValue<DateTime, int>(currentDate, 0));
                }
            }
            return sortDates.OrderBy(item => item.Key.Year).ThenBy(item => item.Key.Month).ToList();
        }

        private static Dictionary<long, double> GetBeerByRoundRating(List<Beer> beers)
        {
            Dictionary<long, double> dictionary = new Dictionary<long, double>();
            foreach (Beer beer in beers)
                dictionary.Add(beer.Id, MathHelper.GetRoundByStep(beer.GlobalRatingScore, DefautlValues.StepRating));

            return dictionary;
        }
    }
}