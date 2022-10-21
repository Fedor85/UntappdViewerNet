using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;
using UntappdViewer.Domain.Models;
using UntappdViewer.Utils;

namespace UntappdViewer.Different.Helpers
{
    public class PieSeriesBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ParametersContainer parametersContainer = value as ParametersContainer;
            int index = parametersContainer.Get<int>("index");
            int count = parametersContainer.Get<int>("count");
            double offset = GetOffSet(index, count);
            GradientStopCollection gradientStopCollection = parameter as GradientStopCollection;
            Color color = GetRelativeColor(gradientStopCollection, offset);
            return new SolidColorBrush(color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        private double GetOffSet(int index, int count)
        {
            if (count == 1)
                return 0;

            double step = 1 / ((double)count - 1);
            return index * step;
        }

        private Color GetRelativeColor(GradientStopCollection gsc, double offset)
        {
            var point = gsc.SingleOrDefault(f => f.Offset == offset);
            if (point != null) return point.Color;

            GradientStop before = gsc.First(w => MathHelper.DoubleCompare(w.Offset, gsc.Min(m => m.Offset)));
            GradientStop after = gsc.First(w => MathHelper.DoubleCompare(w.Offset, gsc.Max(m => m.Offset)));

            foreach (var gs in gsc)
            {
                if (gs.Offset < offset && gs.Offset > before.Offset)
                    before = gs;

                if (gs.Offset > offset && gs.Offset < after.Offset)
                    after = gs;
            }

            var color = new Color();

            color.ScA = (float)((offset - before.Offset) * (after.Color.ScA - before.Color.ScA) / (after.Offset - before.Offset) + before.Color.ScA);
            color.ScR = (float)((offset - before.Offset) * (after.Color.ScR - before.Color.ScR) / (after.Offset - before.Offset) + before.Color.ScR);
            color.ScG = (float)((offset - before.Offset) * (after.Color.ScG - before.Color.ScG) / (after.Offset - before.Offset) + before.Color.ScG);
            color.ScB = (float)((offset - before.Offset) * (after.Color.ScB - before.Color.ScB) / (after.Offset - before.Offset) + before.Color.ScB);

            return color;
        }
    }
}