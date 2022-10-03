using System;
using System.Collections.Generic;
using System.Linq;
using UntappdViewer.Domain.Models;
using UntappdViewer.Infrastructure;
using UntappdViewer.Models;
using UntappdViewer.Utils;

namespace UntappdViewer.Domain
{
    public static class StatisticsCalculation
    {
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

        public static List<KeyValue<string, int>> GetBeerTypeCount(List<Checkin> checkins, List<KeyValue<string, List<long>>> beerTypeCheckinIds)
        {
            List<KeyValue<string, int>> keyValues = new List<KeyValue<string, int>>();

            foreach (KeyValue<string, List<long>> keyValue in beerTypeCheckinIds)
                keyValues.Add(new KeyValue<string, int>(keyValue.Key, checkins.Count(item => keyValue.Value.Contains(item.Id))));

            return keyValues;
        }

        public static List<KeyValue<string, double>> GetBeerTypeRating(List<Checkin> checkins, List<KeyValue<string, List<long>>> beerTypeCheckinIds)
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

            Dictionary<string, List<string>> groupTypes = StringHelper.GetGroupByList(types, DefautlValues.SeparatorsBeerTypeName);

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
            foreach (KeyValue<string, List<long>> keyValue in dictionary.Where(item => item.Value.Count < DefautlValues.BeerTypeCountByOther))
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


        private static Dictionary<long, double> GetBeerByRoundRating(List<Beer> beers)
        {
            Dictionary<long, double> dictionary = new Dictionary<long, double>();
            foreach (Beer beer in beers)
                dictionary.Add(beer.Id, MathHelper.GetRoundByStep(beer.GlobalRatingScore, DefautlValues.StepRating));

            return dictionary;
        }
    }
}
