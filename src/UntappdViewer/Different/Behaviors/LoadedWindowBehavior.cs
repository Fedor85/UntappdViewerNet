using System.Windows;
using Microsoft.Xaml.Behaviors;

namespace UntappdViewer.Behaviors
{
    public class LoadedWindowBehavior : Behavior<Window>
    {
        public static readonly DependencyProperty DependencyProperty = DependencyProperty.Register("LoadedWindow", typeof(bool), typeof(LoadedWindowBehavior), new PropertyMetadata(null));

        public bool LoadedWindow
        {
            get { return (bool) GetValue(DependencyProperty); }
            set { SetValue(DependencyProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += UserControlLoadedHandler;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Loaded -= UserControlLoadedHandler;
        }

        private void UserControlLoadedHandler(object sender, RoutedEventArgs e)
        {
            LoadedWindow = true;
        }
    }
}