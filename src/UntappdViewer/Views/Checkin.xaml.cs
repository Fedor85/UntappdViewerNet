using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace UntappdViewer.Views
{
    /// <summary>
    /// Interaction logic for Checkin.xaml
    /// </summary>
    public partial class Checkin : UserControl
    {
        public Checkin()
        {
            InitializeComponent();
        }

        private void HyperlinkOnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }
    }
}
