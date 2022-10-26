using System.Collections.Generic;
using System.Linq;
using UntappdViewer.Models.Different;

namespace UntappdViewer.Infrastructure
{
    public static class KeyValuesHelper
    {
        public static Dictionary<double, int> KeyValuesToDictionary(List<KeyValue<double, int>> keyValues)
        {
            Dictionary<double, int> dictionary = new Dictionary<double, int>();
            foreach (KeyValue<double, int> keyValue in keyValues)
                dictionary.Add(keyValue.Key, keyValue.Value);

            return dictionary;
        }

        public static List<TK> GetDistinctKeys<TK, TV>(List<KeyValue<TK, TV>> items)
        {
            return items.Select(item => item.Key).Distinct().ToList();
        }

        public static List<KeyValue<TK, TV>> GetMerged<TK, TV>(params List<KeyValue<TK, TV>>[] items)
        {
            List<KeyValue<TK, TV>> list = new List<KeyValue<TK, TV>>();
            foreach (List<KeyValue<TK, TV>> item in items)
                list.AddRange(item);

            return list;
        }
    }
}