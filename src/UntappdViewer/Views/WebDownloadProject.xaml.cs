using System.Windows;
using System.Windows.Controls;

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
            
        }

        private void TextPasswordBoxPasswordChanged(object sender, RoutedEventArgs e)
        {
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