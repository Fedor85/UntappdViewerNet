using System.Windows;
using System.Windows.Controls;
using UntappdViewer.Helpers;

namespace UntappdViewer.Views
{
    /// <summary>
    /// Interaction logic for WebDownloadProject.xaml
    /// </summary>
    public partial class WebDownloadProject : UserControl
    {
        public WebDownloadProject()
        {
            InitializeComponent();
            TokenTextBox.TextPasswordBox.PasswordChanged += TextPasswordBoxPasswordChanged;
            Unloaded += WebDownloadProjectUnloaded;
            TableCheckins.Items.CurrentChanged += TableCheckinsItemsCurrentChanged;
        }

        private void TableCheckinsItemsCurrentChanged(object sender, System.EventArgs e)
        {
            TableCaption.Content = $"{Properties.Resources.Checkins} ({TableCheckins.Items.Count}):";
        }

        private void WebDownloadProjectUnloaded(object sender, RoutedEventArgs e)
        {
            TokenTextBox.Clear();
        }

        public void SetAccessToken(bool? isAccessToken)
        {
            AccessTokenButton.IsEnabled = false;
            if (isAccessToken.HasValue)
            {
                CheckStatusImg.Source = ImageConverter.GetBitmapSource(isAccessToken.Value ? Properties.Resources.green_checkmark : Properties.Resources.red_x);
                CheckStatusImg.Visibility = Visibility.Visible;
            }
            else
            {
                CheckStatusImg.Visibility = Visibility.Hidden;
            }
        }

        private void TextPasswordBoxPasswordChanged(object sender, RoutedEventArgs e)
        {
            CheckStatusImg.Visibility = Visibility.Hidden;

            if (TokenTextBox.TextPasswordBox.Password.Length > 0)
            {
                AccessTokenButton.IsEnabled = true;
            }
            else
            {
                AccessTokenButton.IsEnabled = false;
                CheckStatusImg.Visibility = Visibility.Hidden;
            }
        }
    }
}