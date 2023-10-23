using System;
using System.Globalization;
using System.Windows.Data;

namespace UntappdViewer.UI.Helpers
{
    public class PercentageChangeValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToDouble(value) * GetPercent(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToDouble(value) / GetPercent(parameter);
        }

        private double GetPercent(object parameter)
        {
            string[] parameters = parameter.ToString().Split('|');
            double percent = 1;
            int parametersLength = parameters.Length;

            if (parametersLength == 1 || Int32.Parse(parameters[0]) < 1)
                percent -= Double.Parse(parameters[parametersLength == 1 ? 0 : 1]) / 100;
            else
                percent += Double.Parse(parameters[1]) / 100;

            return percent;
        }
    }
}