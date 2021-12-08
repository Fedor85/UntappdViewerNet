using System.Windows;

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
        }
    }
}
