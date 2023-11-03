using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using UntappdViewer.UI.Helpers;
using UntappdViewer.Utils;

namespace UntappdViewer.UI.Controls
{
    /// <summary>
    /// Interaction logic for ScrollTopButton.xaml
    /// </summary>
    public partial class ScrollTopButton : UserControl
    {

        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register("Size", typeof(double), typeof(ScrollTopButton), new PropertyMetadata(100d));

        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register("StrokeThickness", typeof(double), typeof(ScrollTopButton), new PropertyMetadata(0d));

        private static readonly DependencyProperty BackgroundEllipseProperty = DependencyProperty.Register("BackgroundEllipse", typeof(Brush), typeof(ScrollTopButton), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0, 0, 0))));

        private static readonly DependencyProperty BackgroundStrokeProperty = DependencyProperty.Register("BackgroundStroke", typeof(Brush), typeof(ScrollTopButton), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        private static readonly DependencyProperty BackgroundPointerProperty = DependencyProperty.Register("BackgroundPointer", typeof(Brush), typeof(ScrollTopButton), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(255, 255, 255)) { Opacity = 0.5 }));

        public static readonly DependencyProperty ScrollViewerProperty = DependencyProperty.Register("ScrollViewerConteiner", typeof(FrameworkElement), typeof(ScrollTopButton), new PropertyMetadata(InitializeScrollViewer));

        public double Size
        {
            get { return (double)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        public Brush BackgroundEllipse
        {
            get { return (Brush)GetValue(BackgroundEllipseProperty); }
            set { SetValue(BackgroundEllipseProperty, value); }
        }

        public Brush BackgroundStroke
        {
            get { return (Brush)GetValue(BackgroundStrokeProperty); }
            set { SetValue(BackgroundStrokeProperty, value); }
        }

        public Brush BackgroundPointer
        {
            get { return (Brush)GetValue(BackgroundPointerProperty); }
            set { SetValue(BackgroundPointerProperty, value); }
        }
        public FrameworkElement ScrollViewerConteiner
        {
            get { return (FrameworkElement)GetValue(ScrollViewerProperty); }
            set { SetValue(ScrollViewerProperty, value); }
        }

        [Obsolete("Use the property Size.", true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new double Width
        {
            get { return base.Width; }
        }

        [Obsolete("Use the property Size.", true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new double Height
        {
            get { return base.Height; }
        }

        private ScrollViewer ScrollViewer { get; set; }

        public ScrollTopButton()
        {
            InitializeComponent();

            Loaded += ThisLoaded;
            Unloaded += ThisUnloaded;
            MouseLeftButtonUp += ThisMouseLeftButtonUp;

            SetBinding(UserControl.WidthProperty, new Binding { Path = new PropertyPath(SizeProperty), Source = this });
            SetBinding(UserControl.HeightProperty, new Binding { Path = new PropertyPath(SizeProperty), Source = this });

            PathEllipse.SetBinding(UserControl.WidthProperty, new Binding { Path = new PropertyPath(SizeProperty), Source = this });
            PathEllipse.SetBinding(UserControl.HeightProperty, new Binding { Path = new PropertyPath(SizeProperty), Source = this });
            PathEllipse.SetBinding(Path.FillProperty, new Binding { Path = new PropertyPath(BackgroundEllipseProperty), Source = this });
            PathEllipse.SetBinding(Path.StrokeThicknessProperty, new Binding { Path = new PropertyPath(StrokeThicknessProperty), Source = this });
            PathEllipse.SetBinding(Path.StrokeProperty, new Binding { Path = new PropertyPath(BackgroundStrokeProperty), Source = this });

            PathPointer.SetBinding(UserControl.WidthProperty, new Binding { Path = new PropertyPath(SizeProperty), Source = this });
            PathPointer.SetBinding(UserControl.HeightProperty, new Binding { Path = new PropertyPath(SizeProperty), Source = this });
            PathPointer.SetBinding(Path.FillProperty, new Binding { Path = new PropertyPath(BackgroundPointerProperty), Source = this });
        }

        private void SetScrollViewer(ScrollViewer scrollViewer)
        {
            ScrollViewer = scrollViewer;

            if (ScrollViewer != null)
            {
                Visibility = ScrollViewer.IsVisible && !MathHelper.DoubleCompare(ScrollViewer.VerticalOffset, 0) ? Visibility.Visible : Visibility.Collapsed;
                ScrollViewer.ScrollChanged += ScrollChanged;
            }
            else
            {
                Visibility = Visibility.Collapsed;
            }
        }

        private void ThisUnloaded(object sender, RoutedEventArgs e)
        {
            if(ScrollViewer != null)
                ScrollViewer.ScrollChanged -= ScrollChanged;
        }

        private void ThisLoaded(object sender, RoutedEventArgs e)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(this);
            SetScrollViewer(UIHelper.FindFirstChild<ScrollViewer>(parent));
        }

        private void ThisMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ScrollViewer?.ScrollToVerticalOffset(0);
        }

        private void ScrollViewerConteinerLoaded(object sender, RoutedEventArgs e)
        {
            SetScrollViewer(UIHelper.FindFirstChild<ScrollViewer>(sender as DependencyObject));
        }

        private void ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (!MathHelper.DoubleCompare(e.VerticalChange, 0))
                Visibility = !MathHelper.DoubleCompare(e.VerticalOffset, 0) ? Visibility.Visible : Visibility.Collapsed;
        }

        private static void InitializeScrollViewer(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
                return;

            ScrollTopButton scrollTopButton = dependencyObject as ScrollTopButton;
            FrameworkElement frameworkElement = e.NewValue as FrameworkElement;
            ScrollViewer scrollViewer = frameworkElement as ScrollViewer;
            if (scrollViewer != null)
                scrollTopButton.SetScrollViewer(scrollViewer);
            else
                frameworkElement.Loaded += scrollTopButton.ScrollViewerConteinerLoaded;
        }
    }
}