using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

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
        }

        public void SetAccessToken(bool? isAccessToken)
        {
            AccessTokenButton.IsEnabled = false;
            if (isAccessToken.HasValue)
            {
                if (isAccessToken.Value)
                {
                    CheckStatusImg.Source = Imaging.CreateBitmapSourceFromHBitmap(Properties.Resources.green_checkmark.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                }
                else
                {
                    CheckStatusImg.Source = Imaging.CreateBitmapSourceFromHBitmap(Properties.Resources.red_x.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                }
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