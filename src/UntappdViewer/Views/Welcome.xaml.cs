using System.Windows;
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
                AttachHendlers(welcomeViewModel);
        }

        private void AttachHendlers(IWelcomeViewModel welcomeViewModel)
        {
            OpenFileButton.Click += welcomeViewModel.OpenFileButtonClick;
            Drop += welcomeViewModel.FileOnDrop;
        }
    }
}