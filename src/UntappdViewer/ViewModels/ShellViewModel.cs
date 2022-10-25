using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using Prism.Commands;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using UntappdViewer.Domain;
using UntappdViewer.Helpers;
using UntappdViewer.Infrastructure;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Interfaces.Services.DataBase;
using UntappdViewer.Modules;
using UntappdViewer.Utils;

namespace UntappdViewer.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        private IUntappdService untappdService;

        private IInteractionRequestService interactionRequestService;

        private IRegionManager regionManager;

        private ISettingService settingService;

        private IModuleManager moduleManager;

        private IArgumentsProvider argumentsProvider;

        private IDevEntityDbService devEntityDbService;

        private IWebApiClient webApiClient;

        private IWebDownloader webDownloader;

        private string title;

        private bool loadedWindow;

        public ICommand ClosingCommand { get; }

        public string Title
        {
            get { return title; }
            set
            {
                SetProperty(ref title, value);
            }
        }

        public bool LoadedWindow
        {
            get { return loadedWindow; }
            set
            {
                SetProperty(ref loadedWindow, value);
                if (value)
                    Activate();
            }
        }

        public ShellViewModel(IUntappdService untappdService, IInteractionRequestService interactionRequestService,
                                                              IRegionManager regionManager,
                                                              ISettingService settingService,
                                                              IModuleManager moduleManager,
                                                              IArgumentsProvider argumentsProvider,
                                                              IDevEntityDbService devEntityDbService,
                                                              IWebApiClient webApiClient,
                                                              IWebDownloader webDownloader)
        {
            this.untappdService = untappdService;
            this.interactionRequestService = interactionRequestService;
            this.regionManager = regionManager;
            this.settingService = settingService;
            this.moduleManager = moduleManager;
            this.argumentsProvider = argumentsProvider;
            this.devEntityDbService = devEntityDbService;
            this.webApiClient = webApiClient;
            this.webDownloader = webDownloader;
            ClosingCommand = new DelegateCommand<CancelEventArgs>(Closing);
            untappdService.UpdateUntappdUserNameEvent += UpdateTitle;
            untappdService.CleanUntappdEvent += UpdateTitle;
        }

        private void Activate()
        {
            UpdateTitle();
            UpdateDevSetting();
            Run();
        }

        private void DeActivate()
        {
            foreach (IRegion region in regionManager.Regions)
            {
                foreach (object view in region.Views)
                    region.Deactivate(view);
            }
        }

        private void UpdateDevSetting()
        {
            UpdateDevAvatarImage();
            UpdateDevProfileHeaderImage();
        }

        private void UpdateDevAvatarImage()
        {
            string avatarImageUrl = webApiClient.GetDevAvatarImageUrl();
            if (String.IsNullOrEmpty(avatarImageUrl))
                return;

            string avatarImageUrlDb = devEntityDbService.GetValue<string>("avatarImageUrl");
            if(avatarImageUrl.Equals(avatarImageUrlDb))
                return;

            Stream stream = webDownloader.DownloadToStream(avatarImageUrl);
            if (stream != null)
            {
                devEntityDbService.Add("avatarImageUrl", avatarImageUrl);
                devEntityDbService.AddFile("avatarImage", stream);
            }
        }

        private void UpdateDevProfileHeaderImage()
        {
            string profileHeaderImage = webApiClient.GetDevProfileHeaderImageUrl();
            if (String.IsNullOrEmpty(profileHeaderImage))
                return;

            string profileHeaderImageDb = devEntityDbService.GetValue<string>("profileHeaderImageUrl");
            if (profileHeaderImage.Equals(profileHeaderImageDb))
                return;

            Stream stream = webDownloader.DownloadToStream(profileHeaderImage);
            if (stream != null)
            {
                devEntityDbService.Add("profileHeaderImageUrl", profileHeaderImage);
                devEntityDbService.AddFile("profileHeaderImage", stream);
            }
        }

        private void Run()
        {
            bool isUsedArgument = false;
            string filePath;
            if (argumentsProvider.Arguments.Count > 0)
            {
                filePath = argumentsProvider.Arguments[0];
                isUsedArgument = true;
            }
            else
            {
                filePath = FileHelper.GetFirstFileItemPath(settingService.GetRecentFilePaths());
            }

            FileStatus fileStatus = FileHelper.Check(filePath, Extensions.GetSupportExtensions());
            if (EnumsHelper.IsValidFileStatus(fileStatus))
            {
                try
                {
                    untappdService.Initialize(filePath);
                    if (isUsedArgument)
                        settingService.SetRecentFilePaths(FileHelper.AddFilePath(settingService.GetRecentFilePaths(), filePath, settingService.GetMaxRecentFilePaths()));

                    moduleManager.LoadModule(typeof(MainModule).Name);
                }
                catch (ArgumentException ex)
                {
                    interactionRequestService.ShowError(Properties.Resources.Error, StringHelper.GetFullExceptionMessage(ex));
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
            {
                DeActivate();
                FileHelper.DeleteTempDirectory();
            }
            else
            {
                e.Cancel = true;
            }
        }
    }
}