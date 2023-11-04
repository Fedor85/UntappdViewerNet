using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;

namespace UntappdViewer.ValueConverters
{
   public class MouseWheelEventArgsToDeltaConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            MouseWheelEventArgs eventArgs = value as MouseWheelEventArgs;
            if (eventArgs == null)
                return null;

            return eventArgs.Delta;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
