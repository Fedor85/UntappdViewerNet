using System;
using System.Globalization;

namespace UntappdViewer.Utils
{
    public static class ParserAndConvertHelper
    {
        public static double? GetDoubleValue(string value)
        {
            if (String.IsNullOrEmpty(value))
                return null;

            return Convert.ToDouble(StringHelper.GetNormalizeDecimalSeparator(value));
        }

        public static T GetConvertValue<T>(object value)
        {
            if (value == null || value is string && String.IsNullOrEmpty((string)value))
            {
                if (typeof(T) == typeof(string))
                    value = String.Empty;
                else
                    return default(T);
            }
            //не менять порядок if-ов
            if (IsFloatingPointType(value.GetType()) && typeof(T) == typeof(string))
                value = GetFloatingPointObjectToString(value);

            if (value is string && IsFloatingPointType(typeof(T)))
                return GetValueTypeOfFloatingPoint<T>(value.ToString());

            return (T)Convert.ChangeType(value, typeof(T));
        }


        private static bool IsFloatingPointType(Type type)
        {
            return type == typeof(double) || type == typeof(float) || type == typeof(decimal);
        }

        private static string GetFloatingPointObjectToString(object value)
        {
            if (value == null)
                return String.Empty;

            string stringValue = value.ToString();
            if (value.GetType() == typeof(double) || value.GetType() == typeof(float))
                stringValue = GetFloatingPointTypeToStringInvariantCulture((IFormattable)value);
            else if (value.GetType() == typeof(string) && stringValue.Contains("E"))
                stringValue = GetConvertExponentialFormatStringInvariantCulture(stringValue);

            return StringHelper.GetNormalizeDecimalSeparator(stringValue);
        }

        private static string GetFloatingPointTypeToStringInvariantCulture(IFormattable value)
        {
            string R = value.ToString("R", CultureInfo.InvariantCulture);
            if (!R.Contains("E"))
                return R;

            string G17 = value.ToString("G17", CultureInfo.InvariantCulture);

            if (!G17.Contains("E"))
                return G17;

            return GetConvertExponentialFormatStringInvariantCulture(R);

        }

        private static string GetConvertExponentialFormatStringInvariantCulture(string value)
        {
            string valueString = StringHelper.GetNormalizeDecimalSeparator(value);
            int i = valueString.IndexOf('E');
            string beforeTheE = valueString.Substring(0, i);
            int E = Convert.ToInt32(valueString.Substring(i + 1));

            i = beforeTheE.IndexOf('.');

            if (i < 0)
                i = beforeTheE.Length;
            else
                beforeTheE = beforeTheE.Replace(".", String.Empty);

            i += E;

            while (i < 1)
            {
                beforeTheE = "0" + beforeTheE;
                i++;
            }

            while (i > beforeTheE.Length)
                beforeTheE += "0";

            if (i == beforeTheE.Length)
                return beforeTheE;

            return $"{beforeTheE.Substring(0, i)}.{beforeTheE.Substring(i)}";
        }

        private static T GetValueTypeOfFloatingPoint<T>(string text)
        {
            string normalizedStr = StringHelper.GetNormalizeDecimalSeparator(text);
            return (T)Convert.ChangeType(normalizedStr, typeof(T));
        }
    }
}
