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
            IClosable closable = DataContext as IClosable;
            if (closable != null)
                Closing += closable.Closing;
        }
    }
}