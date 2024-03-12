using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace UntappdViewer.UI.Controls.YouTubeVideo
{
    /// <summary>
    /// Interaction logic for YouTubeButton.xaml
    /// </summary>
    public partial class YouTubeButton : UserControl
    {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(YouTubeButton));

        public event RoutedEventHandler Click;

        public bool isRunPlaySatus { get; set; }

        public bool IsSimpleMode { get; set; }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public YouTubeButton()
        {
            InitializeComponent();
            isRunPlaySatus = true;
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