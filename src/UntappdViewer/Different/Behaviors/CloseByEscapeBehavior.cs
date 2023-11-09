using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;
using UntappdViewer.UI.Helpers;

namespace UntappdViewer.Behaviors
{
    public class CloseByEscapeBehavior : Behavior<UserControl>
    {
        public static readonly DependencyProperty DependencyProperty = DependencyProperty.Register("Window", typeof(Window), typeof(CloseByEscapeBehavior));

        public Window Window
        {
            get { return (Window) GetValue(DependencyProperty); }
            set { SetValue(DependencyProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += AssociatedObjectLoaded;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Loaded -= AssociatedObjectLoaded;
            if (Window != null)
                Window.PreviewKeyDown -= WindowPreviewKeyDown;
        }

        private void AssociatedObjectLoaded(object sender, RoutedEventArgs e)
        {
            Window = UIHelper.GetWindow(AssociatedObject);
            if (Window != null)
                Window.PreviewKeyDown += WindowPreviewKeyDown;
        }

        private void WindowPreviewKeyDown(object sender, KeyEventArgs e)
        {
            Window window = sender as Window;
            if (window != null && e.Key == Key.Escape)
                window.Close();
        }
    }
}