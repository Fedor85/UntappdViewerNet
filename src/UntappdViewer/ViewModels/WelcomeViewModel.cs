using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Prism.Commands;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Infrastructure;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Modules;
using UntappdViewer.Services;
using UntappdViewer.Views;

namespace UntappdViewer.ViewModels
{
    public class WelcomeViewModel: RegionManagerBaseModel
    {
        private UntappdService untappdService;

        private ICommunicationService communicationService;

        private ISettingService settingService;

        private IModuleManager moduleManager;

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

        public WelcomeViewModel(UntappdService untappdService, ICommunicationService communicationService, ISettingService settingService, IModuleManager moduleManager, IRegionManager regionManager): base(regionManager)
        {
            this.untappdService = untappdService;
            this.communicationService = communicationService;
            this.settingService = settingService;
            this.moduleManager = moduleManager;
            OpenFileCommand = new DelegateCommand(OpenFile);
            DropFileCommand = new DelegateCommand<DragEventArgs>(DropFile);
        }

        protected override void Activate()
        {
            base.Activate();
            moduleManager.LoadModule(typeof(RecentFilesModule).Name);
            ActivateView(RegionNames.RecentFiles, typeof(RecentFiles));
        }

        protected override void DeActivate()
        {
            base.DeActivate();
            DeActivateAllViews(RegionNames.RecentFiles);
        }

        private void OpenFile()
        {
            string lastOpenFilePath = FileHelper.GetFirstFileItemPath(settingService.GetRecentFilePaths());
            string filePath = communicationService.OpenFile(String.IsNullOrEmpty(lastOpenFilePath) ? String.Empty : Path.GetDirectoryName(lastOpenFilePath), Extensions.GetSupportExtensions());
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

            settingService.SetRecentFilePaths(FileHelper.AddFilePath(settingService.GetRecentFilePaths(), filePath, settingService.GetMaxRecentFilePaths()));
            moduleManager.LoadModule(typeof(MainModule).Name);
            ActivateView(RegionNames.RootRegion, typeof(Main));
            untappdService.RunUpdateUntappd();
        }
    }
}