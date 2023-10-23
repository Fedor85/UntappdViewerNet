using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace UntappdViewer.UI.Controls.StarRating
{
    /// <summary>
    /// Interaction logic for StarProgressBar.xaml
    /// </summary>
    public partial class StarProgressBar : UserControl
    {
        public static readonly DependencyProperty BorderStarThicknessProperty = DependencyProperty.Register("BorderStarThickness", typeof(double), typeof(StarProgressBar));

        public static readonly DependencyProperty BackgroundStarColorProperty = DependencyProperty.Register("BackgroundStarColor", typeof(Brush), typeof(StarProgressBar));

        public static readonly DependencyProperty BackgroundStarOpacityProperty = DependencyProperty.Register("BackgroundStarOpacity", typeof(double), typeof(StarProgressBar));

        public static readonly DependencyProperty ForegroundStarColorProperty = DependencyProperty.Register("ForegroundStarColor", typeof(Brush), typeof(StarProgressBar));

        public static readonly DependencyProperty ForegroundStarOpacityProperty = DependencyProperty.Register("ForegroundStarOpacity", typeof(double), typeof(StarProgressBar));

        public static readonly DependencyProperty BorderStarColorProperty = DependencyProperty.Register("BorderStarColor", typeof(Brush), typeof(StarProgressBar));

        public static readonly DependencyProperty BorderStarOpacityProperty = DependencyProperty.Register("BorderStarOpacity", typeof(double), typeof(StarProgressBar));

        public double Value
        {
            set { ForegroundStar.Value = value; }
        }

        public StarProgressBar()
        {
            InitializeComponent();
            BackgroundStar.SetBinding(Path.FillProperty, new Binding { Path = new PropertyPath(BackgroundStarColorProperty), Source = this });
            BackgroundStar.SetBinding(Path.OpacityProperty, new Binding { Path = new PropertyPath(BackgroundStarOpacityProperty), Source = this });

            ForegroundStar.SetBinding(ProgressBar.ForegroundProperty, new Binding { Path = new PropertyPath(ForegroundStarColorProperty), Source = this });
            ForegroundStar.SetBinding(ProgressBar.OpacityProperty, new Binding { Path = new PropertyPath(ForegroundStarOpacityProperty), Source = this });

            BorderStar.SetBinding(Path.StrokeThicknessProperty, new Binding { Path = new PropertyPath(BorderStarThicknessProperty), Source = this });
            BorderStar.SetBinding(Path.StrokeProperty, new Binding { Path = new PropertyPath(BorderStarColorProperty), Source = this });
            BorderStar.SetBinding(Path.OpacityProperty, new Binding { Path = new PropertyPath(BorderStarOpacityProperty), Source = this });
        }
    }
}