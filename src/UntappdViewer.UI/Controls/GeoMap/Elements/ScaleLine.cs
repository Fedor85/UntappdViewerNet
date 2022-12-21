using System;
using System.Windows;
using System.Windows.Media;

namespace UntappdViewer.UI.Controls.GeoMap
{
    public class ScaleLine : FrameworkElement
    {
        protected override void OnRender(DrawingContext drawingContext)
        {
            Pen pen = new Pen(Brushes.White, 1);
            double hfPenThickness = pen.Thickness / 2;

            double mainLine = Width;
            double smallLine = mainLine / 2;
            int step = 10;

            drawingContext.DrawLine(pen, new Point(0, hfPenThickness), new Point(mainLine, hfPenThickness));

            int height = Convert.ToInt32(ActualHeight);
            for (int i = height; i > 0; i = i - step)
            {
                double y = i - hfPenThickness;
                drawingContext.DrawLine(pen, new Point(0, y), new Point(mainLine, y));
            }

            for (int i = height - step / 2; i > 0; i = i - step)
            {
                double y = i - hfPenThickness;
                drawingContext.DrawLine(pen, new Point(0, y), new Point(smallLine, y));
            }
        }
    }
}