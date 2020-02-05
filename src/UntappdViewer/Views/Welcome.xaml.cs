using System.Windows.Controls;
using UntappdViewer.Interfaces;

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
            IWelcomeViewModel welcomeViewModel = DataContext as IWelcomeViewModel;
            if (welcomeViewModel != null)
                OpenFileButton.Click += welcomeViewModel.OpenFileButtonClick;
        }
    }
}