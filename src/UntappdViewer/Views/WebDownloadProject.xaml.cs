using System;
using System.Windows;
using System.Windows.Controls;
using UntappdViewer.UI.Helpers;

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
            TokenTextBox.TextChanged += TextChanged;
            Loaded += WebDownloadProjectLoaded;
            Unloaded += WebDownloadProjectUnloaded;
            TableCheckins.Items.CurrentChanged += TableCheckinsItemsCurrentChanged;
        }

        private void WebDownloadProjectLoaded(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(TokenTextBox.Text))
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
            FulllDownloadCheckinsButton.IsEnabled = false;
            FirstDownloadCheckinsButton.IsEnabled = false;
            ToEndDownloadCheckinsButton.IsEnabled = false;
            UpdateBeersPanelEnabled(false);
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
                FulllDownloadCheckinsButton.IsEnabled = false;
                FirstDownloadCheckinsButton.IsEnabled = false;
                ToEndDownloadCheckinsButton.IsEnabled = false;
                UpdateBeersPanelEnabled(false);
            }
        }

        private void TextChanged(string text)
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
            FulllDownloadCheckinsButton.IsEnabled = true;
            if (TableCheckins.Items.Count > 0)
            {
                FirstDownloadCheckinsButton.IsEnabled = true;
                ToEndDownloadCheckinsButton.IsEnabled = true;
                UpdateBeersPanelEnabled(true);
            }
        }

        private void UpdateBeersPanelEnabled(bool isEnabled)
        {
            UpdateBeersButton.IsEnabled = isEnabled;
            OffsetUpdateBeersTextBox.IsEnabled = isEnabled;
        }
    }
}