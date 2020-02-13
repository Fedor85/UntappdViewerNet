using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace UntappdViewer.Utils
{
    public static class StringHelper
    {
        public static string Join<T>(List<T> parameters)
        {
            StringBuilder text = new StringBuilder();
            foreach (T parameter in parameters)
            {
                text.Append(parameter);
                text.Append(";");
            }
            return text.ToString();
        }

        public static string GetShortName(string name)
        {
            Regex regex = new Regex(@"\(.*\)");
            foreach (Match match in regex.Matches(name))
                name = name.Replace(match.Value, String.Empty);

            return name.Trim();
        }
    }
}