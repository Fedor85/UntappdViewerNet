using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Prism.Commands;
using UntappdViewer.Interfaces.Services;

namespace UntappdViewer.ViewModels
{
    public class WelcomeViewModel
    {
        private IDialogService dialogService;

        private ISettingService settingService;

        public ICommand OpenFileCommand { get; }

        public ICommand DropFileCommand { get; }

        public WelcomeViewModel(IDialogService dialogService, ISettingService settingService)
        {
            this.dialogService = dialogService;
            this.settingService = settingService;
            OpenFileCommand = new DelegateCommand(OpenFile);
            DropFileCommand = new DelegateCommand<DragEventArgs>(DropFile);
        }

        private void OpenFile()
        {
            string saveOpenFilePath = settingService.GetLastOpenedFilePath();
            string openFilePath = dialogService.OpenFile(String.IsNullOrEmpty(saveOpenFilePath) ? String.Empty : Path.GetDirectoryName(saveOpenFilePath), Extensions.GetExtensions());
            if (String.IsNullOrEmpty(openFilePath))
                return;

            settingService.SetLastOpenedFilePath(openFilePath);
        }

        private void DropFile(DragEventArgs e)
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

            settingService.SetLastOpenedFilePath(openFilePath);
        }

        public static string GetExtensionWihtoutPoint(string path)
        {
            string extension = Path.GetExtension(path);
            if (String.IsNullOrEmpty(extension))
                return extension;

            return extension.Replace(".", String.Empty).Trim().ToLower();
        }
    }
}