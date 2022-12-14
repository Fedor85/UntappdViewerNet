using System.Windows.Controls;
using System.Windows.Media;

namespace UntappdViewer.UI.Controls.StarRating
{
    /// <summary>
    /// Interaction logic for StarProgressBar.xaml
    /// </summary>
    public partial class StarProgressBar : UserControl
    {
        public Color BackgroundStarColor
        {
            set
            {
                Brush brush = GetBrush(BackgroundStar.Fill);
                ((SolidColorBrush)brush).Color = value;
                BackgroundStar.Fill = brush;
            }
        }

        public double BackgroundStarOpacity
        {
            set
            {
                Brush brush = GetBrush(BackgroundStar.Fill);
                brush.Opacity = value;
                BackgroundStar.Fill = brush;
            }
        }

        public Color ForegroundStarColor
        {
            set
            {
                Brush brush = GetBrush(ForegroundStar.Foreground);
                ((SolidColorBrush)brush).Color = value;
                ForegroundStar.Foreground = brush;
            }
        }

        public double ForegroundStarOpacity
        {
            set
            {
                Brush brush = GetBrush(ForegroundStar.Foreground);
                brush.Opacity = value;
                ForegroundStar.Foreground = brush;
            }
        }

        public Color BorderStarColor
        {
            set
            {
                Brush brush = GetBrush(BorderStar.Stroke);
                ((SolidColorBrush)brush).Color = value;
                BorderStar.Stroke = brush;
            }
        }

        public double BorderStarOpacity
        {
            set
            {
                Brush brush = GetBrush(BorderStar.Stroke);
                brush.Opacity = value;
                BorderStar.Stroke = brush;
            }
        }

        public double BorderStarThickness
        {
            set { BorderStar.StrokeThickness = value; }
        }

        public double Value
        {
            set { ForegroundStar.Value = value; }
        }

        public StarProgressBar()
        {
            InitializeComponent();
        }

        private Brush GetBrush(Brush brush)
        {
            if (brush.IsFrozen)
                brush = brush.Clone();

            return brush;
        }
    }
}