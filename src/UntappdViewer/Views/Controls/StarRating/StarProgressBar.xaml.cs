using System.Windows.Controls;
using System.Windows.Media;

namespace UntappdViewer.Views.Controls.StarRating
{
    /// <summary>
    /// Interaction logic for StarProgressBar.xaml
    /// </summary>
    public partial class StarProgressBar : UserControl
    {
        public Color BackgroundStarColor
        {
            set { ((SolidColorBrush)BackgroundStar.Fill).Color = value; }
        }

        public double BackgroundStarOpacity
        {
            set { ((SolidColorBrush)BackgroundStar.Fill).Opacity = value; }
        }

        public Color ForegroundStarColor
        {
            set { ((SolidColorBrush)ForegroundStar.Foreground).Color = value; }
        }

        public double ForegroundStarOpacity
        {
            set { ((SolidColorBrush)ForegroundStar.Foreground).Opacity = value; }
        }

        public Color BorderStarColor
        {
            set { ((SolidColorBrush)BorderStar.Stroke).Color = value; }
        }

        public double BorderStarOpacity
        {
            set { ((SolidColorBrush)BorderStar.Stroke).Opacity = value; }
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
    }
}
