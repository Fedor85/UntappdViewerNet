using System.Collections.Generic;

namespace UntappdViewer
{
    public static class Extensions
    {
        /// <summary>
        /// Import untappd data
        /// </summary>
        private static string CVS = "cvs";

        /// <summary>
        /// Проект данных по Untappd
        /// </summary>
        private static string UNTP = "untp";

        public static List<string> GetExtensions()
        {
            return new List<string> { CVS, UNTP };
        }
    }
}