using System;
using System.Globalization;
using System.Windows.Data;
using Microsoft.Maps.MapControl.WPF;
using UntappdViewer.UI.Helpers;
using LocationVM = UntappdViewer.UI.Controls.Maps.BingMap.ViewModel.Location;

namespace UntappdViewer.UI.IValueConverters
{
    public class LocationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            LocationVM location = value as LocationVM;
            return location?.GetMapLocation();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Location location = value as Location;
            return location?.GetWPFLocation();
        }
    }
}