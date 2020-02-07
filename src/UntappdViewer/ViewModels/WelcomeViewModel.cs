using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using UntappdViewer.Infrastructure;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Services;

namespace UntappdViewer.ViewModels
{
    public class WelcomeViewModel: BindableBase
    {
        private UntappdService untappdService;

        private IDialogService dialogService;

        private ISettingService settingService;

        private string untappdUserName;

        public ICommand OpenFileCommand { get; }

        public ICommand DropFileCommand { get; }

        public string UntappdUserName
        {
            get { return untappdUserName; }
            set
            {
                untappdUserName = value;
                OnPropertyChanged();
            }
        }

        public WelcomeViewModel(UntappdService untappdService, IDialogService dialogService, ISettingService settingService)
        {
            this.untappdService = untappdService;
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
            untappdService.Initialize(UntappdUserName, openFilePath);
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

            if (!Extensions.GetExtensions().Contains(FileHelper.GetExtensionWihtoutPoint(openFilePath)))
                return;

            settingService.SetLastOpenedFilePath(openFilePath);
            OnPropertyChanged("UntappdUserName");
            untappdService.Initialize(UntappdUserName, openFilePath);
        }
    }
}