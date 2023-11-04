using System;
using System.Globalization;
using System.Windows.Data;
using UntappdViewer.Helpers;

namespace UntappdViewer.ValueConverters
{
    public class ServingTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null ? ConverterHelper.GetServingTypeImagePath(value.ToString()) : String.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }
    }
}