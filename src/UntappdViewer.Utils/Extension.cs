using System;
using System.Collections.Generic;
using System.Linq;

namespace UntappdViewer.Utils
{
    public static class Extension
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> list)
        {
            Random random = new Random();
            return list.OrderBy(item => random.Next());
        }

        public static void MoveToBottom<T>(this List<T> list, T item)
        {
            int index =  list.IndexOf(item);
            if (index < 0)
                return ;

            list.RemoveAt(index);
            list.Add(item);
        }
    }
}