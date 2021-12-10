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
    }
}