using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace UntappdViewer.UI.Controls
{
    /// <summary>
    /// Interaction logic for ImageEllipseClip.xaml
    /// </summary>
    public partial class ImageEllipseClip : UserControl
    {
        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register("ImageSource", typeof(BitmapSource), typeof(ImageEllipseClip));

        public BitmapSource ImageSource
        {
            get { return (BitmapSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }


        public ImageEllipseClip()
        {
            InitializeComponent();
            SizeChanged += ImageEllipseClipSizeChanged;
            UImage.Loaded += ImageLoaded;
            UImage.SetBinding(Image.SourceProperty, new Binding { Path = new PropertyPath(ImageSourceProperty), Source = this });
        }

        private void ImageEllipseClipSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateClipEllipseGeometry();
        }

        private void ImageLoaded(object sender, RoutedEventArgs e)
        {
            UpdateClipEllipseGeometry();
            UImage.Loaded -= ImageLoaded;
        }

        private void UpdateClipEllipseGeometry()
        {
            ClipEllipseGeometry.Center = new Point(ActualWidth / 2, ActualHeight / 2); ;
            ClipEllipseGeometry.RadiusX = UImage.ActualWidth / 2;
            ClipEllipseGeometry.RadiusY = UImage.ActualHeight / 2;
        }
    }
}