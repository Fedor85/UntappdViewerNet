using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using UntappdViewer.UI.Helpers;

namespace UntappdViewer.Views
{
    /// <summary>
    /// Interaction logic for WebDownloadProject.xaml
    /// </summary>
    public partial class WebDownloadProject : UserControl
    {
        private Window window;

        private static readonly SolidColorBrush DefaultMarkFillFullFirstCheckinsBackground = Brushes.OrangeRed;

        public WebDownloadProject()
        {
            InitializeComponent();
            MarkFillFullFirstCheckins.Background = DefaultMarkFillFullFirstCheckinsBackground;
            Loaded += WebDownloadProjectLoaded;
            Unloaded += WebDownloadProjectUnloaded;    
        }

        private void WebDownloadProjectLoaded(object sender, RoutedEventArgs e)
        {
            window = UIHelper.GetWindow(this);
            if (window == null)
                return;

            window.KeyDown += WindowKeyStatesChanged;
            window.KeyUp += WindowKeyStatesChanged;
        }

        private void WebDownloadProjectUnloaded(object sender, RoutedEventArgs e)
        {
            if (window == null)
                return;

            window.KeyDown -= WindowKeyStatesChanged;
            window.KeyUp -= WindowKeyStatesChanged;
        }


        private void WindowKeyStatesChanged(object sender, KeyEventArgs e)
        {
            MarkFillFullFirstCheckins.Background = Keyboard.IsKeyDown(Key.LeftCtrl) ? Brushes.GreenYellow : DefaultMarkFillFullFirstCheckinsBackground;
        }
    }
}