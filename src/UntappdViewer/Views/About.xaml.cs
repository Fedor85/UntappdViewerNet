using System;
using System.Media;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Resources;
using UntappdViewer.Infrastructure;

namespace UntappdViewer.Views
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : Window
    {
        private SoundPlayer player;

        public About()
        {
            InitializeComponent();
            Owner = Application.Current.MainWindow;

            StreamResourceInfo streamResource = Application.GetResourceStream(new Uri(@"Resources/about_sound.wav", UriKind.Relative));
            player = new SoundPlayer(streamResource.Stream);

            PreviewKeyDown += WindowPreviewKeyDown;
            Loaded += WindowLoaded;
            Closed += WindowClosed;
        }

        private void WindowPreviewKeyDown(object sender, KeyEventArgs e)
        {
            Window window = sender as Window;
            if (window != null && e.Key == Key.Escape)
                window.Close();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            player.PlayLooping();
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            player.Stop();
        }

        private void HyperlinkOnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            ProcessStartHelper.ProcessStart(e.Uri.ToString());
        }

        private void YouTubeButtonOnClick(object sender, RoutedEventArgs e)
        {
            player.Stop();
        }

        private void YouTubeVideoOnCloseClick(object sender, RoutedEventArgs e)
        {
            player.PlayLooping();
        }
    }
}