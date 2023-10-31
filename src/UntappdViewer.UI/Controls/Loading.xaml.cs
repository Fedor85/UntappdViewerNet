using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;
using FontAwesome.WPF;

namespace UntappdViewer.UI.Controls
{
    /// <summary>
    /// Interaction logic for Loading.xaml
    /// </summary>
    public partial class Loading : UserControl
    {
        private static readonly DependencyProperty VisibilityImageAwesomeProperty = DependencyProperty.Register("VisibilityImageAwesome", typeof(Visibility), typeof(Loading));

        private static readonly DependencyProperty BackgroundStyleProperty = DependencyProperty.Register("BackgroundStyle", typeof(Style), typeof(Loading), new PropertyMetadata(Application.Current.FindResource("StyleLoadingControl")));

        private static readonly DependencyProperty LoadedContainerProperty = DependencyProperty.Register("LoadedContainer", typeof(FrameworkElement), typeof(Loading), new PropertyMetadata(LoadedContainerСhanged));

        private Action loadingCollapsed;

        public bool AutoCollapsed { get; set; }

        public Visibility VisibilityImageAwesome
        {
            get { return (Visibility)GetValue(VisibilityImageAwesomeProperty); }
            set { SetValue(VisibilityImageAwesomeProperty, value); }
        }

        public Style BackgroundStyle
        {
            get { return (Style)GetValue(BackgroundStyleProperty); }
            set { SetValue(BackgroundStyleProperty, value); }
        }

        public FrameworkElement LoadedContainer
        {
            get { return (FrameworkElement)GetValue(LoadedContainerProperty); }
            set { SetValue(LoadedContainerProperty, value); }
        }


        public Loading()
        {
            InitializeComponent();
            Loaded += LoadedHandler;
            loadingCollapsed = () => { Visibility = Visibility.Collapsed; };

            ImageAwesome.SetBinding(ImageAwesome.VisibilityProperty, new Binding { Path = new PropertyPath(VisibilityImageAwesomeProperty), Source = this });
            BackgroundControl.SetBinding(Grid.StyleProperty, new Binding { Path = new PropertyPath(BackgroundStyleProperty), Source = this });
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            if (!AutoCollapsed || !IsVisible)
                return;

            Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, loadingCollapsed);
        }

        private static void LoadedContainerСhanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            Loading loading = dependencyObject as Loading;
            if (!loading.AutoCollapsed)
                return;

            loading.DetachingContainerEvents(e.OldValue as FrameworkElement);
            loading.AttachedContainerEvents(e.NewValue as FrameworkElement);
        }

        private void AttachedContainerEvents(FrameworkElement container)
        {
            if (container == null)
                return;

            container.Loaded += ContainerLoaded;
            container.Unloaded += ContainerUnloaded;
        }

        private void DetachingContainerEvents(FrameworkElement container)
        {
            if (container == null)
                return;

            container.Loaded -= ContainerLoaded;
            container.Unloaded -= ContainerUnloaded;
        }

        private void ContainerLoaded(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Visible;
        }

        private void ContainerUnloaded(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Collapsed;
        }
    }
}