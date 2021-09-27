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

        private void TextPasswordBoxPasswordChanged(object sender, RoutedEventArgs e)
        {
            AccessTokenButton.IsEnabled = TokenTextBox.TextPasswordBox.Password.Length > 0;
        }
    }
}