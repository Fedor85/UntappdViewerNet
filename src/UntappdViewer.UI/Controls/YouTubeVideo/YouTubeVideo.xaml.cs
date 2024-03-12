using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UntappdViewer.UI.Helpers;

namespace UntappdViewer.UI.Controls.YouTubeVideo
{
    /// <summary>
    /// Interaction logic for YouTubeVideo.xaml
    /// </summary>
    public partial class YouTubeVideo : UserControl
    {
        private static readonly DependencyProperty YouTubeVideoIdProperty = DependencyProperty.Register("YouTubeVideoId", typeof(string), typeof(YouTubeVideo), new PropertyMetadata(SetYouTubeVideoId));
        
        public string YouTubeVideoId
        {
            get { return (string)GetValue(YouTubeVideoIdProperty); }
            set { SetValue(YouTubeVideoIdProperty, value); }
        }

        public bool IsVisibleCloseButton { get; set; }

        public bool IsCollapseByCLose { get; set; }


        public YouTubeVideo()
        {
            InitializeComponent();
            IsVisibleCloseButton = false;
            Environment.SetEnvironmentVariable("WEBVIEW2_ADDITIONAL_BROWSER_ARGUMENTS", "--autoplay-policy=no-user-gesture-required");
            Loaded += YouTubeVideoLoaded;
            Unloaded += YouTubeVideoUnloaded;
        }

        private void YouTubeVideoLoaded(object sender, RoutedEventArgs e)
        {
            if(IsVisibleCloseButton)
            {
                ButtonGrid.Visibility = Visibility.Visible;
                WebView.Margin = new Thickness(30, 0, 30, 0);
            }
        }
        private void YouTubeVideoUnloaded(object sender, RoutedEventArgs e)
        {
            SetSource(String.Empty);
        }

        private void SetSource(string source)
        {
            WebView.Source = String.IsNullOrEmpty(source) ? new Uri("about:blank") : new Uri(source);
            MainGrid.Visibility = String.IsNullOrEmpty(source) ? Visibility.Hidden : Visibility.Visible;
            Visibility = IsCollapseByCLose && String.IsNullOrEmpty(source) ? Visibility = Visibility.Collapsed : Visibility.Visible;
        }

        private void GridPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.UpdateSource(YouTubeVideoIdProperty, String.Empty);
        }

        private static void SetYouTubeVideoId(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            YouTubeVideo youTubeVideo = dependencyObject as YouTubeVideo;
            youTubeVideo.SetSource(String.IsNullOrEmpty(e.NewValue as String) ? String.Empty : $"http://www.youtube.com/embed/{e.NewValue}?autoplay=1");
        }
    }
}