using System;
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
        
        private static readonly DependencyProperty VisibilityAnimationProperty = DependencyProperty.Register("VisibilityAnimation", typeof(bool), typeof(LikeControl), new PropertyMetadata(true, VisibilityAnimationChanged));

        public UIElement RoutedControl
        {
            get { return (UIElement)GetValue(RoutedControlProperty); }
            set { SetValue(RoutedControlProperty, value); }
        }
        
        public bool VisibilityAnimation
        {
            get { return (bool)GetValue(VisibilityAnimationProperty); }
            set { SetValue(VisibilityAnimationProperty, value); }
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

            LikeControl likeControl = dependencyObject as LikeControl;
            HandlerRoutedEvent handlerRoutedEvent = new HandlerRoutedEvent(routedControl);
            handlerRoutedEvent.Initialize(likeControl, "Mouse");
        }

        private static void VisibilityAnimationChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            LikeControl likeControl = dependencyObject as LikeControl;
            likeControl.LottieAnimation.Visibility = Convert.ToBoolean(e.NewValue) ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}