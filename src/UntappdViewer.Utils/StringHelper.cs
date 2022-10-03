using System;
using System.Collections.Generic;
using System.Linq;
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

        public static string GetNormalizeDecimalSeparator(string str)
        {
            return GetNormalizeDecimalSeparator(str, System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
        }

        public static string GetNormalizedJPGPath(string path)
        {
            Regex regex = new Regex(@".*jp(e?)g");
            MatchCollection matches = regex.Matches(path);
            return matches.Count > 0 ? matches[0].Value : path;
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

                        name =name.Insert(index, $"{Convert.ToChar(ControlChar.NewLine)}{(isInsertEmpty ? String.Empty : new string(Convert.ToChar(ControlChar.Space), insertSpaceCount - 1))}");
                        break;
                    }
                    index--;
                }
            }
            return name;
        }

        public static string GetSplitByLength(string text, int length)
        {
            if (String.IsNullOrEmpty(text))
                return String.Empty;

            List<string> lines = new List<string>();
            foreach (string newLine in text.Split(Convert.ToChar(ControlChar.NewLine)).ToList())
            {
                if (newLine.Length > length)
                    lines.AddRange(GetSplitWordsByLength(newLine, length));
                else
                    lines.Add(newLine.Trim());
            }
            return GetMergedLines(lines);
        }

        public static string GetRemoveEmptyLines(string text)
        {
            if (String.IsNullOrEmpty(text))
                return text;

            List<string> lines = new List<string>();
            foreach (string newLine in text.Split(Convert.ToChar(ControlChar.NewLine)))
            {
                string line = newLine.Trim();
                if (String.IsNullOrEmpty(line))
                    continue;

                lines.Add(line);
            }
            return GetMergedLines(lines);
        }

        public static List<string> GetValues(string valueLine)
        {
            if (valueLine == null || String.IsNullOrEmpty(valueLine.Trim()))
                return new List<string>();

            return valueLine.Split(',').Select(item => item.Trim()).ToList();
        }

        public static Dictionary<string, List<string>> GetGroupByList(List<string> values)
        {
            Dictionary<string, List<string>> group = new Dictionary<string, List<string>>();
            int count = values.Count;
            if (count == 0)
                return group;

            foreach (IGrouping<string, string> grouping in values.GroupBy(i => i.Split()[0]))
            {
                List<string> currentValues = grouping.ToList();
                group.Add(GetKey(currentValues), currentValues);
            }

            return group;
        }

        public static string GetFullExceptionMessage(Exception ex)
        {
            string message = String.Empty;
            if (ex == null)
                return message;

            message = ex.Message;
            string innerMessage = GetFullExceptionMessage(ex.InnerException);
            if (!String.IsNullOrEmpty(innerMessage))
                message += $"\n{innerMessage}";

            return message;
        }

        public static string GetEmailUrl(string email)
        {
            return $"mailto:{email}";
        }

        private static string GetNormalizeDecimalSeparator(string str, string requiredSeparator)
        {
            return GetNormalizeDecimalSeparator(str, requiredSeparator, ",.");
        }

        private static string GetNormalizeDecimalSeparator(string str, string requiredSeparator, string possibleSeparators)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char ch in str)
            {
                if (possibleSeparators.IndexOf(ch) != -1)
                    sb.Append(requiredSeparator);
                else
                    sb.Append(ch); 
            }

            return sb.ToString();
        }

        private static List<string> GetSplitWordsByLength(string text, int length)
        {
            List<string> lines = text.Split(Convert.ToChar(ControlChar.Space))
                                        .Aggregate(new[] { String.Empty }.ToList(), (result, nextItem) =>
                                        {
                                            int lastIndex = result.Count - 1;
                                            string last = result[lastIndex];
                                            string mergeItem = $"{last} {nextItem}".Trim();

                                            if (mergeItem.Length > length)
                                                result.Add(nextItem);
                                            else
                                                result[lastIndex] = mergeItem;

                                            return result;
                                        });

            return lines;
        }

        private static string GetMergedLines(List<string> lines)
        {
            int linesCount = lines.Count;
            if (linesCount == 0)
                return String.Empty;

            if (linesCount == 1)
                return lines[0];

            StringBuilder result = new StringBuilder();
            for (int i = 0; i < linesCount; i++)
            {
                result.Append(lines[i]);
                if (i != linesCount - 1)
                    result.AppendLine();
            }
            return result.ToString();
        }

        private static string GetKey(List<string> values)
        {
            int count = values.Count;
            string firstValue= values[0];
            string key = String.Empty;
            foreach (string words in firstValue.Split())
            {
                if (words.Equals("-") || words.Equals("/"))
                    break;

                string currentKey = $"{key} {words}".Trim();
                if (values.Count(item => item.StartsWith(currentKey)) != count)
                    break;

                key = currentKey;
            }
            return key;
        }
    }
}