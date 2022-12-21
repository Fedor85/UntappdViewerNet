using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace UntappdViewer.UI.Controls.GeoMap
{
    /// <summary>
    /// Interaction logic for GradientScale.xaml
    /// </summary>
    public partial class GradientScale : UserControl
    {
        public static readonly DependencyProperty GradientStopCollectionProperty = DependencyProperty.Register("GradientStopCollection", typeof(GradientStopCollection), typeof(GradientScale), new PropertyMetadata(default(GradientStopCollection)));

        public static readonly DependencyProperty StartProperty = DependencyProperty.Register("Start", typeof(double?), typeof(GradientScale), new PropertyMetadata(null));

        public static readonly DependencyProperty EndProperty = DependencyProperty.Register("End", typeof(double?), typeof(GradientScale), new PropertyMetadata(null));

        public GradientStopCollection GradientStopCollection
        {
            get { return (GradientStopCollection)GetValue(GradientStopCollectionProperty); }
            set { SetValue(GradientStopCollectionProperty, value); }
        }

        public double? Start
        {
            get { return (double?)GetValue(StartProperty); }
            set { SetValue(StartProperty, value); }
        }

        public double? End
        {
            get { return (double?)GetValue(EndProperty); }
            set { SetValue(EndProperty, value); }
        }

        public GradientScale()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}