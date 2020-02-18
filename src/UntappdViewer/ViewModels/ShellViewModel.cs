using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using UntappdViewer.Events;
using UntappdViewer.Helpers;
using UntappdViewer.Infrastructure;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Modules;
using UntappdViewer.Services;
using Untappd = UntappdViewer.Models.Untappd;

namespace UntappdViewer.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        private UntappdService untappdService;

        private ICommunicationService communicationService;

        private IRegionManager regionManager;

        private IEventAggregator eventAggregator;

        private ISettingService settingService;

        private IModuleManager moduleManager;

        private string title;

        public ICommand ClosingCommand { get; }

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged();
            }
        }

        public ShellViewModel(UntappdService untappdService, ICommunicationService communicationService, IRegionManager regionManager, IEventAggregator eventAggregator, ISettingService settingService, IModuleManager moduleManager)
        {
            this.untappdService = untappdService;
            this.communicationService = communicationService;
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;
            this.settingService = settingService;
            this.moduleManager = moduleManager;
            ClosingCommand = new DelegateCommand<CancelEventArgs>(Closing);
            eventAggregator.GetEvent<InitializeUntappdEvent>().Subscribe(UpdateTitle);
            eventAggregator.GetEvent<CleanUntappdEvent>().Subscribe(UpdateTitle);
            Title = CommunicationHelper.GetTitle();
            Activate();
        }

        private void Activate()
        {
            string filePath = FileHelper.GetFirstFileItemPath(settingService.GetRecentFilePaths());
            FileStatus fileStatus = FileHelper.Check(filePath, Extensions.GetSupportExtensions());
            if (EnumsHelper.IsValidFileStatus(fileStatus))
            {
                try
                {
                    untappdService.Initialize(String.Empty, filePath);
                    moduleManager.LoadModule(typeof(MainModule).Name);
                }
                catch (ArgumentException ex)
                {
                    communicationService.ShowError(Properties.Resources.Error, ex.Message);
                    moduleManager.LoadModule(typeof(WelcomeModule).Name);
                }
            }
            else
            {
                if (fileStatus != FileStatus.IsEmptyPath)
                    communicationService.ShowMessage(Properties.Resources.Warning, CommunicationHelper.GetFileStatusMessage(fileStatus, filePath));

                moduleManager.LoadModule(typeof(WelcomeModule).Name);
            }
        }

        private void UpdateTitle(Untappd untappd)
        {
            Title = CommunicationHelper.GetTitle(untappd.UserName);
        }

        private void UpdateTitle()
        {
            Title = CommunicationHelper.GetTitle();
        }

        private void Closing(CancelEventArgs e)
        {
            if (communicationService.Ask(Properties.Resources.Warning, Properties.Resources.AskCloseApp) == MessageBoxResult.OK)
                DeActivate();
            else
                e.Cancel = true;
        }

        private void DeActivate()
        {
            foreach (IRegion region in regionManager.Regions)
            {
                foreach (object view in region.Views)
                    region.Deactivate(view);
            }
        }
    }
}