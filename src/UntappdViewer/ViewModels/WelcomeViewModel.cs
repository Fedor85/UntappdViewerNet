using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Prism.Commands;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Domain;
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

        private IDevEntityDbService devEntityDbService;

        private IWebApiClient webApiClient;

        private IWebDownloader webDownloader;

        private BitmapSource avatarImage;

        private BitmapSource profileHeaderImage;

        private List<FileItem> fileItems;

        public ICommand OpenFileCommand { get; }

        public ICommand DropFileCommand { get; }

        public ICommand CreateProjectCommand { get; }

        public ICommand OpenRecentFileCommand { get; }

        public ICommand DeleteRecentFileByListCommand { get; }

        public string EmailUrl
        {
            get { return StringHelper.GetEmailUrl(Properties.Resources.Email); }
        }

        public BitmapSource AvatarImage
        {
            get { return avatarImage; }
            set { SetProperty(ref avatarImage, value); }
        }

        public BitmapSource ProfileHeaderImage
        {
            get { return profileHeaderImage; }
            set { SetProperty(ref profileHeaderImage, value); }
        }

        public List<FileItem> FileItems
        {
            get { return fileItems; }
            set { SetProperty(ref fileItems, value); }
        }

        public WelcomeViewModel(IUntappdService untappdService, IInteractionRequestService interactionRequestService,
                                                                ISettingService settingService,
                                                                IModuleManager moduleManager,
                                                                IRegionManager regionManager,
                                                                IDevEntityDbService devEntityDbService,
                                                                IWebApiClient webApiClient,
                                                                IWebDownloader webDownloader) : base(regionManager)
        {
            this.untappdService = untappdService;
            this.interactionRequestService = interactionRequestService;
            this.settingService = settingService;
            this.moduleManager = moduleManager;
            this.settingService = settingService;
            this.devEntityDbService = devEntityDbService;
            this.webApiClient = webApiClient;
            this.webDownloader = webDownloader;

            OpenFileCommand = new DelegateCommand(OpenFile);
            DropFileCommand = new DelegateCommand<DragEventArgs>(DropFile);
            CreateProjectCommand = new DelegateCommand(CreateProject);
            OpenRecentFileCommand = new DelegateCommand<string>(OpenRecentFile);
            DeleteRecentFileByListCommand = new DelegateCommand<string>(DeleteRecentFileByList);
        }

        protected override void Activate()
        {
            base.Activate();

            FileItems = FileHelper.GetExistsParseFilePaths(settingService.GetRecentFilePaths());
            settingService.SetStartWelcomeView(true);
            untappdService.CleanUpUntappd();
            SetDevData();
        }

        protected override void DeActivate()
        {
            base.DeActivate();
            FileItems.Clear();
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

        private void OpenRecentFile(string filePath)
        {
            FileHelper.AddFile(FileItems, filePath, settingService.GetMaxRecentFilePaths());
            RunUntappd(filePath);
        }

        private void DeleteRecentFileByList(string filePath)
        {
            FileHelper.RemoveFilePath(FileItems, filePath);
            settingService.SetRecentFilePaths(FileHelper.GetMergedFilePaths(FileItems));
            FileItems = FileHelper.GetExistsParseFilePaths(settingService.GetRecentFilePaths());
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
            FillAvatarImage();
            FillProfileHeaderImage();
        }

        private async void FillAvatarImage()
        {
            try
            {
                await Task.Run(() => UpdateDevImage(webApiClient.GetDevAvatarImageUrl, "avatarImageUrl", "avatarImage"));
            }
            finally
            {
                AvatarImage = devEntityDbService.GetBitmapSource("avatarImage");
            }
        }


        private async void FillProfileHeaderImage()
        {
            try
            {
                await Task.Run(() => UpdateDevImage(webApiClient.GetDevProfileHeaderImageUrl, "profileHeaderImageUrl", "profileHeaderImage"));
            }
            finally
            {
                ProfileHeaderImage = devEntityDbService.GetBitmapSource("profileHeaderImage");
            }
        }

        private void UpdateDevImage(Func<string> funcGetImage, string imageUrl, string imageName)
        {
            string profileHeaderImage = funcGetImage.Invoke();
            if (String.IsNullOrEmpty(profileHeaderImage))
                return;

            string profileHeaderImageDb = devEntityDbService.GetValue<string>(imageUrl);
            if (profileHeaderImage.Equals(profileHeaderImageDb))
                return;

            Stream stream = webDownloader.DownloadToStream(profileHeaderImage);
            if (stream == null)
                return;

            devEntityDbService.Add(imageUrl, profileHeaderImage);
            devEntityDbService.AddFile(imageName, stream);
        }
    }
}