using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using UntappdViewer.Domain;
using UntappdViewer.Helpers;
using UntappdViewer.Models.Different;

namespace UntappdViewer.ValueConverters
{
    public class PieSeriesBackgroundConverter : IValueConverter
    {
        private GradientHelper gradientHelper;

        public PieSeriesBackgroundConverter()
        {
            gradientHelper = new GradientHelper(DefaultValues.MainGradient3);
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ParametersContainer parametersContainer = value as ParametersContainer;
            int index = parametersContainer.Get<int>(ParameterNames.Index);
            int count = parametersContainer.Get<int>(ParameterNames.Count);
            return new SolidColorBrush(gradientHelper.GetRelativeColor(index, count));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}