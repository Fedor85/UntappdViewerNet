using System.Collections.Generic;

namespace UntappdViewer
{
    public static class Extensions
    {
        /// <summary>
        /// Import untappd data
        /// </summary>
        private static string CSV = "csv";

        /// <summary>
        /// Проект данных по Untappd
        /// </summary>
        private static string UNTP = "untp";

        public static List<string> GetSupportExtensions()
        {
            return new List<string> { CSV, UNTP };
        }
    }
}