using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using UntappdViewer.UI.Controls.Maps.GeoMap;

namespace UntappdViewer.UI.Controls.YouTubeVideo
{
    /// <summary>
    /// Interaction logic for YouTubeButton.xaml
    /// </summary>
    public partial class YouTubeButton : UserControl
    {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(YouTubeButton));

        private static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register("Background", typeof(Brush), typeof(YouTubeButton), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(255, 255, 255)) { Opacity = 0.3 }));

        public event RoutedEventHandler Click;

        public bool isRunPlaySatus { get; set; }

        public bool IsSimpleMode { get; set; }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        public YouTubeButton()
        {
            InitializeComponent();
            isRunPlaySatus = true;
            BackgroundGrid.SetBinding(Grid.BackgroundProperty, new Binding { Path = new PropertyPath(BackgroundProperty), Source = this });
            MouseLeftButtonUp += YouTubeButtonMouseLeftButtonUp;
            Unloaded += YouTubeButtonUnloaded;
        }

        private void YouTubeButtonMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Click?.Invoke(sender, e);
            Command?.Execute(isRunPlaySatus);

            if (!IsSimpleMode)
            {
                CloseButton.Visibility = isRunPlaySatus ? Visibility.Visible : Visibility.Collapsed;
                PlayButton.Visibility = isRunPlaySatus ? Visibility.Collapsed : Visibility.Visible;
                isRunPlaySatus = !isRunPlaySatus;
            }
        }
        private void YouTubeButtonUnloaded(object sender, RoutedEventArgs e)
        {
            if (!isRunPlaySatus)
            {
                if(!IsSimpleMode)
                    CloseButton.Visibility = Visibility.Collapsed;

                PlayButton.Visibility = Visibility.Visible;
                isRunPlaySatus = !isRunPlaySatus;
            }
        }
    }
}