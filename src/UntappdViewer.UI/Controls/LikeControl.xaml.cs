using System.Windows;
using System.Windows.Controls;
using UntappdViewer.UI.Helpers;

namespace UntappdViewer.UI.Controls
{
    /// <summary>
    /// Interaction logic for LikeControl.xaml
    /// </summary>
    public partial class LikeControl : UserControl
    {
        public static readonly DependencyProperty RoutedControlProperty = DependencyProperty.Register("RoutedControl", typeof(UIElement), typeof(LikeControl), new PropertyMetadata(InitializeRoutedControl));

        public UIElement RoutedControl
        {
            get { return (UIElement)GetValue(RoutedControlProperty); }
            set { SetValue(RoutedControlProperty, value); }
        }

        public LikeControl()
        {
            InitializeComponent();
        }

        private static void InitializeRoutedControl(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            UIElement routedControl = e.NewValue as UIElement;
            if (routedControl == null)
                return;

            LikeControl likeLottieAnimation = dependencyObject as LikeControl;
            HandlerRoutedEvent handlerRoutedEvent = new HandlerRoutedEvent(routedControl);
            handlerRoutedEvent.Initialize(likeLottieAnimation, "Mouse");
        }
    }
}