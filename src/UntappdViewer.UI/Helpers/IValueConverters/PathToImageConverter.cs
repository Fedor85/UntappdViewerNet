using System;
using System.Globalization;
using System.Windows.Data;
using UntappdViewer.UI.Helpers;

namespace UntappdViewer.UI.ValueConverters
{
    public class PathToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string path = value as string;
            return  String.IsNullOrWhiteSpace(path) ? null : ImageConverter.GetBitmapSource(path);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
