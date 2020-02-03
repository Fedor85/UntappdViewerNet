using System.Windows;
using UntappdViewer.Interfaces;

namespace UntappdViewer.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Shell : Window
    {
        public Shell()
        {
            InitializeComponent();
            ICloseable closeable = DataContext as ICloseable;
            if (closeable != null)
                Closing += closeable.Closing;
        }
    }
}