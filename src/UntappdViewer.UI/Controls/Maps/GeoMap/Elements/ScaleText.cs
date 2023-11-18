using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace UntappdViewer.UI.Controls.Maps.GeoMap
{
    public class ScaleText : FrameworkElement
    {
        public static readonly DependencyProperty StartProperty = DependencyProperty.Register("Start", typeof(double?), typeof(ScaleText), new PropertyMetadata(null, ValueChanged));

        public static readonly DependencyProperty Endroperty = DependencyProperty.Register("End", typeof(double?), typeof(ScaleText), new PropertyMetadata(null, ValueChanged));

        public double? Start
        {
            get { return (double?)GetValue(StartProperty); }
            set { SetValue(StartProperty, value); }
        }

        public double? End
        {
            get { return (double?)GetValue(Endroperty); }
            set { SetValue(Endroperty, value); }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (!Start.HasValue || !End.HasValue)
            {
                Width = 5;
                return;
            }

            List<double> widths = new List<double>();

            double delta = 5;
            FormattedText formattedStart = GetFormattedText(Start.ToString());
            widths.Add(formattedStart.Width);
            drawingContext.DrawText(formattedStart, new Point(0, ActualHeight - GetOffSetY(formattedStart.Height)));
            double startMinY = ActualHeight - formattedStart.Height / 2 - delta;

            FormattedText formattedEnd = GetFormattedText(End.ToString());
            widths.Add(formattedEnd.Width);
            drawingContext.DrawText(formattedEnd, new Point(0, -GetOffSetY(formattedEnd.Height)));
            double endMaxY = formattedEnd.Height / 2 + delta;

            double count = Math.Round(ActualHeight / 30);
            if (count > 0)
            {
                double range = End.Value - Start.Value;
                double stepValue = GetStepValue(range, count);
                List<double> values = GetValues(Start.Value, End.Value, stepValue);

                foreach (double value in values)
                {
                    FormattedText formatted = GetFormattedText(value.ToString());
                    double percent = (value - Start.Value) / range * 100;
                    double y = ActualHeight - ActualHeight / 100 * percent;
                    if (y > startMinY || y < endMaxY)
                        continue;

                    widths.Add(formatted.Width);
                    drawingContext.DrawText(formatted, new Point(0, y - GetOffSetY(formatted.Height)));
                }
            }

            Width = widths.Max();
        }

        private double GetStepValue(double range, double count)
        {
            double minimum = range / count;
            double magnitude = Math.Pow(10, Math.Floor(Math.Log(minimum) / Math.Log(10)));
            double residual = minimum / magnitude;
            return magnitude * GetDelta(residual);
        }

        private double GetDelta(double residual)
        {
            if (residual > 5)
                return 10;

            if (residual > 2)
                return 5;

            return residual > 1 ? 2 : 1;
        }

        private List<double> GetValues(double start, double end, double step)
        {
            List<double> values = new List<double>();
            if (step == 0)
                return values;

            for (double i = GetCeilingByStep(start, step); i < end; i = i + step)
                values.Add(i);

            return values;
        }

        private static double GetCeilingByStep(double value, double step)
        {
            if (step == 0)
                return value;

            if (value == 0)
                return step;

            double delta = value / step;
            double ceilingValue = Math.Ceiling(delta);
            return ceilingValue * step;
        }

        private double GetOffSetY(double height)
        {
            return height / 2;
        }

        private FormattedText GetFormattedText(string text)
        {
            return new FormattedText(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                                            new Typeface(SystemFonts.MessageFontFamily.ToString()), 11, Brushes.White);
        }

        private static void ValueChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement frameworkElement = dependencyObject as FrameworkElement;
            frameworkElement.InvalidateVisual();
        }
    }
}
