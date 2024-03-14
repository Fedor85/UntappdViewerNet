using System.Windows.Controls;
using System.Windows.Navigation;
using UntappdViewer.Infrastructure;

namespace UntappdViewer.Views.Controls
{
    /// <summary>
    /// Interaction logic for TitleControl.xaml
    /// </summary>
    public partial class TitleControl : UserControl
    {
        public TitleControl()
        {
            InitializeComponent();
        }

        private void HyperlinkOnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            ProcessStartHelper.ProcessStart(e.Uri.ToString());
        }
    }
}