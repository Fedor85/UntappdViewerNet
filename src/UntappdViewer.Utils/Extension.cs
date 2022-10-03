using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace UntappdViewer.Utils
{
    public static class Extension
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> list)
        {
            Random random = new Random();
            return list.OrderBy(item => random.Next());
        }

        public static List<T> MoveToBottom<T>(this List<T> list, T item)
        {
            int index =  list.IndexOf(item);
            if (index < 0)
                return list;

            list.RemoveAt(index);
            list.Add(item);
            return list;
        }
    }
}