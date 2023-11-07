using System;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace UntappdViewer.UI.ValueConverters
{
    public class PathImageReSizeWidthConverter : IValueConverter
    {
        private const int defaultDecodeWidth = 100;

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string path = value as string;

            if (String.IsNullOrEmpty(path))
                return value;

            if(!Path.IsPathRooted(path))
                return value;

            FileInfo info = new FileInfo(path);
            if (!info.Exists || info.Length <= 0)
                return value;

            BitmapImage image = new BitmapImage();

            image.BeginInit();
            image.DecodePixelWidth = parameter == null ? defaultDecodeWidth : System.Convert.ToInt32(parameter);
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = new Uri(info.FullName);
            image.EndInit();

            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}