using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Prism.Commands;
using Prism.Events;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Domain;
using UntappdViewer.Events;
using UntappdViewer.Helpers;
using UntappdViewer.Infrastructure;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Interfaces.Services.DataBase;
using UntappdViewer.Modules;
using UntappdViewer.Utils;
using UntappdViewer.Views;

namespace UntappdViewer.ViewModels
{
    public class WelcomeViewModel: RegionManagerBaseModel
    {
        private IUntappdService untappdService;

        private IInteractionRequestService interactionRequestService;

        private ISettingService settingService;

        private IModuleManager moduleManager;

        private IEventAggregator eventAggregator;

        private IDevEntityDbService devEntityDbService;

        private IWebApiClient webApiClient;

        private IWebDownloader webDownloader;

        private BitmapSource avatarImage;

        private BitmapSource profileHeaderImage;

        public ICommand OpenFileCommand { get; }

        public ICommand DropFileCommand { get; }

        public ICommand CreateProjectCommand { get; }

        public string EmailUrl
        {
            get { return StringHelper.GetEmailUrl(Properties.Resources.Email); }
        }

        public BitmapSource AvatarImage
        {
            get { return avatarImage; }
            set
            {
                SetProperty(ref avatarImage, value);
            }
        }

        public BitmapSource ProfileHeaderImage
        {
            get { return profileHeaderImage; }
            set
            {
                SetProperty(ref profileHeaderImage, value);
            }
        }

        public WelcomeViewModel(IUntappdService untappdService, IInteractionRequestService interactionRequestService,
                                                                ISettingService settingService,
                                                                IModuleManager moduleManager,
                                                                IRegionManager regionManager,
                                                                IEventAggregator eventAggregator,
                                                                IDevEntityDbService devEntityDbService,
                                                                IWebApiClient webApiClient,
                                                                IWebDownloader webDownloader) : base(regionManager)
        {
            this.untappdService = untappdService;
            this.interactionRequestService = interactionRequestService;
            this.settingService = settingService;
            this.moduleManager = moduleManager;
            this.settingService = settingService;
            this.eventAggregator = eventAggregator;
            this.devEntityDbService = devEntityDbService;
            this.webApiClient = webApiClient;
            this.webDownloader = webDownloader;

            OpenFileCommand = new DelegateCommand(OpenFile);
            DropFileCommand = new DelegateCommand<DragEventArgs>(DropFile);
            CreateProjectCommand = new DelegateCommand(CreateProject);
        }

        protected override void Activate()
        {
            base.Activate();

            settingService.SetStartWelcomeView(true);
            untappdService.CleanUpUntappd();
            eventAggregator.GetEvent<OpenFileEvent>().Subscribe(RunUntappd);
            moduleManager.LoadModule(typeof(RecentFilesModule).Name);
            ActivateView(RegionNames.RecentFilesRegion, typeof(RecentFiles));
            SetDevData();
        }

        protected override void DeActivate()
        {
            base.DeActivate();
            eventAggregator.GetEvent<OpenFileEvent>().Unsubscribe(RunUntappd);
            DeActivateAllViews(RegionNames.RecentFilesRegion);
        }

        private void OpenFile()
        {
            string lastOpenFilePath = FileHelper.GetFirstFileItemPath(settingService.GetRecentFilePaths());
            string filePath = interactionRequestService.OpenFile(String.IsNullOrEmpty(lastOpenFilePath) ? String.Empty : Path.GetDirectoryName(lastOpenFilePath), Extensions.GetSupportExtensions());
            if (String.IsNullOrEmpty(filePath))
                return;

            RunUntappd(filePath);
        }

        private void DropFile(DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                return;

            string[] filesPaths = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (filesPaths.Length > 0)
                RunUntappd(filesPaths[0]);
        }

        private void CreateProject()
        {
            string untappdUserName = String.Empty;
            if (!interactionRequestService.AskReplaceText(Properties.Resources.UntappdUserNameCaption, ref untappdUserName))
                return;

            untappdService.Create(untappdUserName);
            moduleManager.LoadModule(typeof(MainModule).Name);
            ActivateView(RegionNames.RootRegion, typeof(Main));
            interactionRequestService.ClearMessageOnStatusBar();
        }

        private void RunUntappd(string filePath)
        {
            FileStatus fileStatus = FileHelper.Check(filePath, Extensions.GetSupportExtensions());
            if (!EnumsHelper.IsValidFileStatus(fileStatus))
            {
                interactionRequestService.ShowMessage(Properties.Resources.Warning, CommunicationHelper.GetFileStatusMessage(fileStatus, filePath));
                return;
            }
            try
            {
                string untappdUserName = String.Empty;
                if (FileHelper.GetExtensionWihtoutPoint(filePath).Equals(Extensions.CSV))
                {
                    if (!interactionRequestService.AskReplaceText(Properties.Resources.UntappdUserNameCaption, ref untappdUserName))
                        return;
                }

                untappdService.Initialize(filePath, untappdUserName);
            }
            catch (ArgumentException ex)
            {
                interactionRequestService.ShowError(Properties.Resources.Error, StringHelper.GetFullExceptionMessage(ex));
                return;
            }

            settingService.SetRecentFilePaths(FileHelper.AddFilePath(settingService.GetRecentFilePaths(), filePath, settingService.GetMaxRecentFilePaths()));
            moduleManager.LoadModule(typeof(MainModule).Name);
            ActivateView(RegionNames.RootRegion, typeof(Main));
            interactionRequestService.ShowMessageOnStatusBar(filePath);
        }

        private void SetDevData()
        {
            AvatarImage = GetAvatarImage();
            ProfileHeaderImage = GetProfileHeaderImage();
        }

        private BitmapSource GetAvatarImage()
        {
            UpdateDevAvatarImage();
            return devEntityDbService.GetBitmapSource("avatarImage");
        }

        private BitmapSource GetProfileHeaderImage()
        {
            UpdateDevProfileHeaderImage();
            return devEntityDbService.GetBitmapSource("profileHeaderImage");
        }

        private void UpdateDevAvatarImage()
        {
            string avatarImageUrl = webApiClient.GetDevAvatarImageUrl();
            if (String.IsNullOrEmpty(avatarImageUrl))
                return;

            string avatarImageUrlDb = devEntityDbService.GetValue<string>("avatarImageUrl");
            if (avatarImageUrl.Equals(avatarImageUrlDb))
                return;

            Stream stream = webDownloader.DownloadToStream(avatarImageUrl);
            if (stream == null)
                return;

            devEntityDbService.Add("avatarImageUrl", avatarImageUrl);
            devEntityDbService.AddFile("avatarImage", stream);
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
            if (stream == null)
                return;

            devEntityDbService.Add("profileHeaderImageUrl", profileHeaderImage);
            devEntityDbService.AddFile("profileHeaderImage", stream);
        }
    }
}