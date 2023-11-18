using System;
using System.Globalization;
using System.Windows.Data;
using Microsoft.Maps.MapControl.WPF;

namespace UntappdViewer.UI.IValueConverters
{
    public class CredentialsProviderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new ApplicationIdCredentialsProvider(value?.ToString() ?? String.Empty);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ApplicationIdCredentialsProvider credentialsProvider = value as ApplicationIdCredentialsProvider;
            return credentialsProvider?.ApplicationId ?? String.Empty;
        }
    }
}
