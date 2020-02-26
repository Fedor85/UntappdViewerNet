using System.Collections.Generic;

namespace UntappdViewer.Domain
{
    public static class Extensions
    {
        /// <summary>
        /// Import untappd data
        /// </summary>
        public const string CSV = "csv";

        /// <summary>
        /// Проект данных по Untappd
        /// </summary>
        public const string UNTP = "untp";

        public static List<string> GetSupportExtensions()
        {
            return new List<string> { CSV, UNTP };
        }

        public static List<string> GetSaveExtensions()
        {
            return new List<string> { UNTP };
        }
    }
}