using System;
using System.ComponentModel;
using System.Windows.Input;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using UntappdViewer.Domain.Services;
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

        private InteractionRequestService interactionRequestService;

        private IRegionManager regionManager;

        private ISettingService settingService;

        private IModuleManager moduleManager;

        public InteractionRequest<IConfirmation> ConfirmationRequest { get; private set; }

        public InteractionRequest<INotification> NotificationRequest { get; private set; }

        private string title;

        private bool loadedWindow;

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

        public bool LoadedWindow
        {
            get { return loadedWindow; }
            set
            {
                loadedWindow = value;
                OnPropertyChanged();
                if(value)
                    Activate();
            }
        }

        public ShellViewModel(UntappdService untappdService, InteractionRequestService interactionRequestService,
                                                                IRegionManager regionManager,
                                                                ISettingService settingService,
                                                                IModuleManager moduleManager)
        {
            this.untappdService = untappdService;
            this.interactionRequestService = interactionRequestService;
            this.regionManager = regionManager;
            this.settingService = settingService;
            this.moduleManager = moduleManager;

            ConfirmationRequest = interactionRequestService.ConfirmationRequest;
            NotificationRequest = interactionRequestService.NotificationRequest;

            ClosingCommand = new DelegateCommand<CancelEventArgs>(Closing);
            untappdService.UpdateUntappdUserNameEvent += UpdateTitle;
            untappdService.CleanUntappdEvent += UpdateTitle;
        }

        private void Activate()
        {
            Title = CommunicationHelper.GetTitle();
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
                    interactionRequestService.ShowError(Properties.Resources.Error, ex.Message);
                    moduleManager.LoadModule(typeof(WelcomeModule).Name);
                }
            }
            else
            {
                if (fileStatus != FileStatus.IsEmptyPath)
                    interactionRequestService.ShowMessage(Properties.Resources.Warning, CommunicationHelper.GetFileStatusMessage(fileStatus, filePath));

                moduleManager.LoadModule(typeof(WelcomeModule).Name);
            }
        }

        private void UpdateTitle(string untappdUserName)
        {
            Title = CommunicationHelper.GetTitle(untappdUserName);
        }

        private void UpdateTitle()
        {
            Title = CommunicationHelper.GetTitle();
        }

        private void Closing(CancelEventArgs e)
        {
            if (interactionRequestService.Ask(Properties.Resources.Warning, Properties.Resources.AskCloseApp))
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