using System.Windows.Controls;
using System.Windows.Navigation;
using UntappdViewer.Infrastructure;

namespace UntappdViewer.Views
{
    /// <summary>
    /// Interaction logic for Welcom.xaml
    /// </summary>
    public partial class Welcome : UserControl
    {
        public Welcome()
        {
            InitializeComponent();
        }

        private void HyperlinkOnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            ProcessStartHelper.ProcessStart(e.Uri.ToString());
        }
    }
}