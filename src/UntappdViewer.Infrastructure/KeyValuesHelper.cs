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

        public static List<KeyValue<T, int>> GetAccumulateValues<T>(List<KeyValue<T, int>> items)
        {
            List<KeyValue<T, int>> keyValues = new List<KeyValue<T, int>>();
            if (!items.Any())
                return keyValues;

            KeyValue<T, int> firstItem = items[0];
            keyValues.Add(new KeyValue<T, int>(firstItem.Key, firstItem.Value));
            for (int i = 1; i < items.Count; i++)
            {
                KeyValue<T, int> previousItem = keyValues[i - 1];
                KeyValue<T, int> currentItem = items[i];
                keyValues.Add(new KeyValue<T, int>(currentItem.Key, currentItem.Value + previousItem.Value));
            }
            return keyValues;
        }

        public static List<KeyValue<TK, int>> GetListCount<TK, TV>(List<KeyValue<TK, List<TV>>> items)
        {
            List<KeyValue<TK, int>> keyValues = new List<KeyValue<TK, int>>();

            foreach (KeyValue<TK, List<TV>> item in items)
                keyValues.Add(new KeyValue<TK, int>(item.Key, item.Value.Count));

            return keyValues;
        }
    }
}