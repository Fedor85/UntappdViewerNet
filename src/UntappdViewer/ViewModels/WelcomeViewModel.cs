using System;
using System.IO;
using System.Windows;
using Prism.Mvvm;
using UntappdViewer.Interfaces;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Properties;

namespace UntappdViewer.ViewModels
{
    public class WelcomeViewModel : BindableBase, IWelcomeViewModel
    {
        private IDialogService dialogService;

        public WelcomeViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;
        }

        public void OpenFileButtonClick(object sender, RoutedEventArgs e)
        {
            string filePath = dialogService.OpenFile(Settings.Default.OpenFileInitialDirectory, Extensions.CVS, Extensions.UNTP);
            if (String.IsNullOrEmpty(filePath))
                return;

            Settings.Default.OpenFileInitialDirectory = Path.GetDirectoryName(filePath);
            Settings.Default.Save();
        }
    }
}