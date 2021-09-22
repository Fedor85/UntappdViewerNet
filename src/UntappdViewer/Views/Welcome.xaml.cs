using System.Windows.Controls;
using System.Windows.Navigation;

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
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }
    }
}