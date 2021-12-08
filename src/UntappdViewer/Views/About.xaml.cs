using System.Windows;
using System.Windows.Input;

namespace UntappdViewer.Views
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();
            Owner = Application.Current.MainWindow;
            PreviewKeyDown += WindowPreviewKeyDown;
        }

        private void WindowPreviewKeyDown(object sender, KeyEventArgs e)
        {
            Window window = sender as Window;
            if (window != null && e.Key == Key.Escape)
                window.Close();
        }
    }
}
