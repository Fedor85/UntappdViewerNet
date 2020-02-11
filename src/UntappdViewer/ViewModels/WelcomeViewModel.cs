using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Prism.Commands;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using UntappdViewer.Infrastructure;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Modules;
using UntappdViewer.Services;
using UntappdViewer.Views;

namespace UntappdViewer.ViewModels
{
    public class WelcomeViewModel: BindableBase
    {
        private UntappdService untappdService;

        private ICommunicationService communicationService;

        private ISettingService settingService;

        private IModuleManager moduleManager;

        private IRegionManager regionManager;

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

        public WelcomeViewModel(UntappdService untappdService, ICommunicationService communicationService, ISettingService settingService, IModuleManager moduleManager, IRegionManager regionManager)
        {
            this.untappdService = untappdService;
            this.communicationService = communicationService;
            this.settingService = settingService;
            this.moduleManager = moduleManager;
            this.regionManager = regionManager;
            OpenFileCommand = new DelegateCommand(OpenFile);
            DropFileCommand = new DelegateCommand<DragEventArgs>(DropFile);
        }

        private void OpenFile()
        {
            string saveOpenFilePath = settingService.GetLastOpenedFilePath();
            string filePath = communicationService.OpenFile(String.IsNullOrEmpty(saveOpenFilePath) ? String.Empty : Path.GetDirectoryName(saveOpenFilePath), Extensions.GetSupportExtensions());
            if (String.IsNullOrEmpty(filePath))
                return;

            RunUntappd(filePath);
        }

        private void DropFile(DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                return;

            string[] filesPaths = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (filesPaths.Length == 0)
                return;

            RunUntappd(filesPaths[0]);
        }

        private void RunUntappd(string filePath)
        {
            FileStatus fileStatus = FileHelper.Check(filePath, Extensions.GetSupportExtensions());
            if (!EnumsHelper.IsValidFileStatus(fileStatus))
            {
                communicationService.ShowMessage(Properties.Resources.Warning, CommunicationHelper.GetFileStatusMessage(fileStatus, filePath));
                return;
            }

            try
            {
                untappdService.Initialize(UntappdUserName, filePath);
            }
            catch (ArgumentException ex)
            {
                communicationService.ShowError(Properties.Resources.Error, ex.Message);
                return;
            }

            settingService.SetLastOpenedFilePath(filePath);
            moduleManager.LoadModule(typeof(MainModule).Name);
            IRegion requestInfoRegion = regionManager.Regions[RegionNames.RootRegion];
            object newView = requestInfoRegion.Views.First(i => i.GetType().Equals(typeof(Main)));
            requestInfoRegion.Activate(newView);
        }
    }
}