using System.Collections;
using System.Windows;
using System.Windows.Controls.DataVisualization.Charting;

namespace UntappdViewer.Helpers
{
    //костыль для исправления ошибки кода загрузка графика происходит на другой вкладке,
    //и если потом перейти на вкладку с графиками, то точки смещают
    public static class DataPointSeriesExtensions
    {
        public static readonly DependencyProperty IsExternalProperty = DependencyProperty.RegisterAttached("Register", typeof(bool), typeof(DataPointSeriesExtensions), new PropertyMetadata(RegisterChanged));

        public static bool GetRegister(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsExternalProperty);
        }

        public static void SetRegister(DependencyObject obj, bool value)
        {
            obj.SetValue(IsExternalProperty, value);
        }

        private static void RegisterChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            DataPointSeries dataPointSeries = sender as DataPointSeries;

            if ((bool)args.NewValue)
                dataPointSeries.Loaded += LoadedPointSeries;
            else
                dataPointSeries.Loaded -= LoadedPointSeries;
        }

        private static void LoadedPointSeries(object sender, RoutedEventArgs e)
        {
            DataPointSeries dataPointSeries = sender as DataPointSeries;
            if (dataPointSeries.ItemsSource == null)
                return;

            IEnumerable itemsSource = dataPointSeries.ItemsSource;
            dataPointSeries.ItemsSource = null;
            dataPointSeries.ItemsSource = itemsSource;
            dataPointSeries.Loaded -= LoadedPointSeries;
        }
    }
}