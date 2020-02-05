using System;
using System.IO;
using System.Linq;
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
            string openFilePath = dialogService.OpenFile(Settings.Default.OpenFileInitialDirectory, Extensions.GetExtensions());
            if (String.IsNullOrEmpty(openFilePath))
                return;

            SaveSettings(openFilePath);
        }

        public void FileOnDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                return;

            string[] filesPaths = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (filesPaths.Length == 0)
                return;

            string openFilePath = filesPaths[0];
            if (String.IsNullOrEmpty(openFilePath))
                return;

            if (!Extensions.GetExtensions().Contains(GetExtensionWihtoutPoint(openFilePath)))
                return;

            SaveSettings(openFilePath);
        }

        public static string GetExtensionWihtoutPoint(string path)
        {
            string extension = Path.GetExtension(path);
            if (String.IsNullOrEmpty(extension))
                return extension;

            return extension.Replace(".", String.Empty).Trim().ToLower();
        }

        private void SaveSettings(string filePath)
        {
            Settings.Default.OpenFileInitialDirectory = Path.GetDirectoryName(filePath);
            Settings.Default.Save();
        }
    }
}