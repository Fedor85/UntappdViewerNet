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
            Regex parenthesesRegex = new Regex(@"\(.*?\)");
            foreach (Match match in parenthesesRegex.Matches(name))
                name = name.Replace(match.Value, String.Empty);

            name = name.Replace("(", String.Empty).Replace(")", String.Empty);
            Regex cpaceRegex = new Regex(@"\s{2,}");
            foreach (Match match in cpaceRegex.Matches(name))
                name = name.Remove(match.Index, match.Length - 1);

            string[] nameItems = name.Split('/');
            name = nameItems[0];
            return name.Trim();
        }

        public static string GeBreakForLongName(string name, int maxLength, int insertSpaceCount)
        {
            int index = maxLength;
            if (name.Length > index)
            {
                index--;
                while (index != 0)
                {
                    if (name[index] == Convert.ToChar(ControlChar.Space))
                    {
                        bool isInsertEmpty = insertSpaceCount == 0;
                        if (isInsertEmpty)
                            name = name.Remove(index, 1);

                        name =name.Insert(index, String.Format("{0}{1}", Convert.ToChar(ControlChar.NewLine), isInsertEmpty ? String.Empty : 
                                                                                                                new string(Convert.ToChar(ControlChar.Space), insertSpaceCount - 1)));
                        break;;
                    }
                    index--;
                }
            }
            return name;
        }
    }
}