using System.Windows.Controls;
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
            if (!e.Uri.AbsoluteUri.Equals(DefautlValues.DefaultUrl))
                System.Diagnostics.Process.Start(e.Uri.AbsoluteUri);
        }
    }
}
