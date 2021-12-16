using System;
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
        private bool? isAccessToken;

        public WebDownloadProject()
        {
            InitializeComponent();
            isAccessToken = false;
            TokenTextBox.PasswordChanged += PasswordChanged;
            Loaded += WebDownloadProjectLoaded;
            Unloaded += WebDownloadProjectUnloaded;
            TableCheckins.Items.CurrentChanged += TableCheckinsItemsCurrentChanged;
        }

        private void WebDownloadProjectLoaded(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(TokenTextBox.Password))
                AccessTokenButton.IsEnabled = true;
        }

        private void TableCheckinsItemsCurrentChanged(object sender, System.EventArgs e)
        {
            TableCaption.Content = $"{Properties.Resources.Checkins} ({TableCheckins.Items.Count}):";
            if (isAccessToken.HasValue && isAccessToken.Value)
                SetEnabledDownloadButtons();
        }

        private void WebDownloadProjectUnloaded(object sender, RoutedEventArgs e)
        {
            TokenTextBox.Clear();
            FulllDownloadButton.IsEnabled = false;
            FirstDownloadButton.IsEnabled = false;
            ToEndDownloadButton.IsEnabled = false;
        }

        public void SetAccessToken(bool? isAccessToken)
        {
            this.isAccessToken = isAccessToken;
            AccessTokenButton.IsEnabled = false;
            if (isAccessToken.HasValue)
            {
                CheckStatusImg.Source = ImageConverter.GetBitmapSource(isAccessToken.Value ? Properties.Resources.green_checkmark : Properties.Resources.red_x);
                CheckStatusImg.Visibility = Visibility.Visible;
                if (isAccessToken.Value)
                    SetEnabledDownloadButtons();
            }
            else
            {
                CheckStatusImg.Visibility = Visibility.Hidden;
                FulllDownloadButton.IsEnabled = false;
                FirstDownloadButton.IsEnabled = false;
                ToEndDownloadButton.IsEnabled = false;
            }
        }

        private void PasswordChanged(string text)
        {
            CheckStatusImg.Visibility = Visibility.Hidden;

            if (text.Length > 0)
            {
                AccessTokenButton.IsEnabled = true;
            }
            else
            {
                AccessTokenButton.IsEnabled = false;
                CheckStatusImg.Visibility = Visibility.Hidden;
            }
        }

        private void SetEnabledDownloadButtons()
        {
            FulllDownloadButton.IsEnabled = true;
            if (TableCheckins.Items.Count > 0)
            {
                FirstDownloadButton.IsEnabled = true;
                ToEndDownloadButton.IsEnabled = true;
            }
        }
    }
}