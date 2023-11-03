using System.Windows;

namespace UntappdViewer.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Shell : Window
    {s
        public Shell()
        {
            InitializeComponent();
            SizeChanged += ShellSizeChanged;
            ContentRendered += ShellContentRendered;
         
        }

        private void ShellSizeChanged(object sender, SizeChangedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }


        private void ShellContentRendered(object sender, System.EventArgs e)
        {
            WindowState = WindowState.Normal;
        }
    }
}