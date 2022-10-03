using System.Collections.Generic;
using System.Linq;
using UntappdViewer.Domain.Models;
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
            List<KeyValue<double, int>> ratingsViewModels = new List<KeyValue<double, int>>();
            if (!beers.Any())
                return ratingsViewModels;

            Dictionary<long, double> dictionaryBeerRating = GetBeerByRoundRating(beers);
            List<double> ratings = dictionaryBeerRating.Select(item => item.Value).Distinct().ToList();
            ratings.Sort();
            foreach (double rating in ratings)
                ratingsViewModels.Add(new KeyValue<double, int>(rating, dictionaryBeerRating.Count(item => MathHelper.DoubleCompare(item.Value, rating))));

            return ratingsViewModels;
        }

        public static List<KeyValue<string, int>> GetBeerTypeCount(List<Checkin> checkins)
        {
            List<KeyValue<string, int>> typeCountViewModels = new List<KeyValue<string, int>>();
            if (!checkins.Any())
                return typeCountViewModels;

            List<string> types = checkins.Select(item => item.Beer.Type).Distinct().ToList();
            types.Sort();
            types.Reverse();
            Dictionary<string, List<string>> groupTypes = StringHelper.GetGroupByList(types, DefautlValues.SeparatorsBeerTypeName);
            foreach (KeyValuePair<string, List<string>> keyValuePair in groupTypes)
            {
                int sum = 0;
                foreach (string type in keyValuePair.Value)
                    sum += checkins.Count(item => type.Equals(item.Beer.Type));

                typeCountViewModels.Add(new KeyValue<string, int>(keyValuePair.Key, sum));
            }

            KeyValue<string, int> other = typeCountViewModels.FirstOrDefault(item => item.Key.Equals("Other"));
            List<string> remove = new List<string>();
            foreach (KeyValue<string, int> keyValuePair in typeCountViewModels.Where(item => item.Value < 15))
            {
                other.Value += keyValuePair.Value;
                remove.Add(keyValuePair.Key);
            }
            typeCountViewModels.RemoveAll(item => remove.Contains(item.Key));
            return typeCountViewModels;
        }


        private static Dictionary<long, double> GetBeerByRoundRating(List<Beer> beers)
        {
            Dictionary<long, double> dictionary = new Dictionary<long, double>();
            foreach (Beer beer in beers)
                dictionary.Add(beer.Id, MathHelper.GetRoundByStep(beer.GlobalRatingScore, 0.25));

            return dictionary;
        }
    }
}
