using System.Windows;
using System.Windows.Media;

namespace UntappdViewer.UI.Controls.Maps.GeoMap
{
    public class GridLines : FrameworkElement
    {
        public static readonly DependencyProperty BackgroundGridProperty = DependencyProperty.Register("BackgroundGrid", typeof(Brush), typeof(GridLines), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(165, 165, 165)) { Opacity = 0.5 }));

        public static readonly DependencyProperty ThicknessGridProperty = DependencyProperty.Register("ThicknessGrid", typeof(double), typeof(GridLines), new PropertyMetadata(0.5));

        public static readonly DependencyProperty StepGridProperty = DependencyProperty.Register("StepGrid", typeof(double), typeof(GridLines), new PropertyMetadata(20.0));

        private Brush BackgroundGrid
        {
            get { return (Brush)GetValue(BackgroundGridProperty); }
            set { SetValue(BackgroundGridProperty, value); }
        }

        private double ThicknessGrid
        {
            get { return (double)GetValue(ThicknessGridProperty); }
            set { SetValue(ThicknessGridProperty, value); }
        }

        private double StepGrid
        {
            get { return (double)GetValue(StepGridProperty); }
            set { SetValue(StepGridProperty, value); }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            Pen pen = new Pen(BackgroundGrid, ThicknessGrid);
            double step = StepGrid;

            double hrActualWidth = ActualWidth / 2;

            for (double i = hrActualWidth; i < ActualWidth; i = i + step)
                drawingContext.DrawLine(pen, new Point(i, 0), new Point(i, ActualHeight));

            for (double i = hrActualWidth - step; i > 0; i = i - step)
                drawingContext.DrawLine(pen, new Point(i, 0), new Point(i, ActualHeight));

            double hrActualHeight = ActualHeight / 2;

            for (double i = hrActualHeight; i < ActualHeight; i = i + step)
                drawingContext.DrawLine(pen, new Point(0, i), new Point(ActualWidth, i));

            for (double i = hrActualHeight - step; i > 0; i = i - step)
                drawingContext.DrawLine(pen, new Point(0, i), new Point(ActualWidth, i));
        }
    }
}