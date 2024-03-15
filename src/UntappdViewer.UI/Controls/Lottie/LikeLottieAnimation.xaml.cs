using System.Windows;
using System.Windows.Controls;
using UntappdViewer.UI.Helpers;

namespace UntappdViewer.UI.Controls.Lottie
{
    /// <summary>
    /// Interaction logic for LikeLottieAnimation.xaml
    /// </summary>
    public partial class LikeLottieAnimation : UserControl
    {
        public static readonly DependencyProperty RoutedControlProperty = DependencyProperty.Register("RoutedControl", typeof(UIElement), typeof(LikeLottieAnimation), new PropertyMetadata(InitializeRoutedControl));

        public UIElement RoutedControl
        {
            get { return (UIElement)GetValue(RoutedControlProperty); }
            set { SetValue(RoutedControlProperty, value); }
        }

        public LikeLottieAnimation()
        {
            InitializeComponent();
        }

        private static void InitializeRoutedControl(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            UIElement routedControl = e.NewValue as UIElement;
            if (routedControl == null)
                return;

            LikeLottieAnimation likeLottieAnimation = dependencyObject as LikeLottieAnimation;
            HandlerRoutedEvent handlerRoutedEvent = new HandlerRoutedEvent(routedControl);
            handlerRoutedEvent.Initialize(likeLottieAnimation, "Mouse");
        }
    }
}