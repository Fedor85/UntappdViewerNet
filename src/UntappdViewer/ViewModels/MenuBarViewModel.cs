using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Domain;
using UntappdViewer.Events;
using UntappdViewer.Helpers;
using UntappdViewer.Infrastructure;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Models;
using UntappdViewer.Modules;
using UntappdViewer.Utils;

namespace UntappdViewer.ViewModels
{
    public class MenuBarViewModel: RegionManagerBaseModel
    {
        private IUntappdService untappdService;

        private IInteractionRequestService interactionRequestService;

        private ISettingService settingService;

        private IModuleManager moduleManager;

        private IEventAggregator eventAggregator;

        private IWebDownloader webDownloader;

        private IReportingService reportingService;

        public ICommand GoToWelcomeCommand { get; }

        public ICommand RenameProjectCommand { get; }

        public ICommand SaveProjectCommand { get; }

        public ICommand SaveAsProjectCommand { get; }

        public ICommand DownloadProjectMediaCommand { get; }

        public ICommand UploadProjectPhotoCommand { get; }

        public ICommand WebDownloadProjectCommand { get; }

        public ICommand CheckinsProjectReportCommand { get; }

        public MenuBarViewModel(IUntappdService untappdService,  IInteractionRequestService interactionRequestService,
                                                                 ISettingService settingService,
                                                                 IModuleManager moduleManager,
                                                                 IRegionManager regionManager,
                                                                 IEventAggregator eventAggregator,
                                                                 IWebDownloader webDownloader,
                                                                 IReportingService reportingService) : base(regionManager)
        {
            this.interactionRequestService = interactionRequestService;
            this.settingService = settingService;
            this.untappdService = untappdService;
            this.moduleManager = moduleManager;
            this.eventAggregator = eventAggregator;
            this.webDownloader = webDownloader;
            this.reportingService = reportingService;

            GoToWelcomeCommand = new DelegateCommand(GoToWelcome);
            RenameProjectCommand = new DelegateCommand(RenameProject);
            SaveProjectCommand = new DelegateCommand(SaveProject);
            SaveAsProjectCommand = new DelegateCommand(SaveAsProject);
            DownloadProjectMediaCommand = new DelegateCommand(DownloadProjectMedia);
            UploadProjectPhotoCommand = new DelegateCommand(UploadProjectPhotos);
            WebDownloadProjectCommand = new DelegateCommand(WebDownloadProject);
            CheckinsProjectReportCommand = new DelegateCommand(CheckinsProjectReport);
        }

        protected override void Activate()
        {
            base.Activate();
            eventAggregator.GetEvent<SaveUntappdToFileEvent>().Subscribe(SaveСhangesProject);
        }

        protected override void DeActivate()
        {
            base.DeActivate();
            eventAggregator.GetEvent<SaveUntappdToFileEvent>().Unsubscribe(SaveСhangesProject);
        }

        private void GoToWelcome()
        {
            moduleManager.LoadModule(typeof(WelcomeModule).Name);
            ActivateView(RegionNames.RootRegion, typeof(Views.Welcome));
        }

        private void RenameProject()
        {
            string name = untappdService.GetUntappdUserName();
            string oldName = name;
            if (interactionRequestService.AskReplaceText(Properties.Resources.RenameProject,ref name) && !oldName.Equals(name))
                untappdService.UpdateUntappdUserName(name);
        }

        private void SaveProject()
        {
            if (!FileHelper.GetExtensionWihtoutPoint(untappdService.FilePath).Equals(Extensions.UNTP))
            {
                SaveAsProject();
                return;
            }

            if (!untappdService.IsDirtyUntappd())
                return;

            if (!interactionRequestService.Ask(Properties.Resources.Warning, Properties.Resources.AskSaveСhangesUntappdProject))
                return;

            FileHelper.SaveFile(untappdService.FilePath, untappdService.Untappd);
            untappdService.ResetСhanges();
        }

        private void SaveAsProject()
        {
            string fileSavePath = interactionRequestService.SaveFile(String.IsNullOrEmpty(untappdService.FilePath) ? String.Empty : Path.GetDirectoryName(untappdService.FilePath), untappdService.GetUntappdProjectFileName(), Extensions.GetSaveExtensions());
            if (String.IsNullOrEmpty(fileSavePath))
                return;

            FileHelper.SaveFile(fileSavePath, untappdService.Untappd);
            untappdService.Initialize(fileSavePath);
            untappdService.ResetСhanges();
            FileHelper.CreateDirectory(untappdService.GetFileDataDirectory());
            interactionRequestService.ShowMessageOnStatusBar(untappdService.FilePath);
            settingService.SetRecentFilePaths(FileHelper.AddFilePath(settingService.GetRecentFilePaths(), fileSavePath, settingService.GetMaxRecentFilePaths()));
        }

        private void DownloadProjectMedia()
        {
            if (!FileHelper.GetExtensionWihtoutPoint(untappdService.FilePath).Equals(Extensions.UNTP))
            {
                interactionRequestService.ShowMessage(Properties.Resources.Warning, Properties.Resources.WarningMessageSaveProjectToUNTP);
                return;
            }
            DownloadFiles();
        }

        private async void DownloadFiles()
        {
            List<Checkin> checkins = untappdService.GetCheckins();
            int count = checkins.Count;
            int counter = 1;
            foreach (Checkin checkin in checkins)
            {
                interactionRequestService.ShowMessageOnStatusBar(CommunicationHelper.GetLoadingMessage($"{counter++}/{count} -> {checkin.Beer.Name}"));
                await Task.Run(() => DownloadFiles(checkin));
            }
            interactionRequestService.ShowMessageOnStatusBar(CommunicationHelper.GetLoadingMessage(untappdService.FilePath));
        }

        private void DownloadFiles(Checkin checkin)
        {
            DownloadFile(checkin.UrlPhoto, untappdService.GetCheckinPhotoFilePath(checkin));
            DownloadFile(checkin.Beer.LabelUrl, untappdService.GetBeerLabelFilePath(checkin.Beer));
            DownloadFile(checkin.Beer.Brewery.LabelUrl, untappdService.GetBreweryLabelFilePath(checkin.Beer.Brewery));
            foreach (Badge badge in checkin.Badges)
                DownloadFile(badge.ImageUrl, untappdService.GetBadgeImageFilePath(badge));
        }

        private void DownloadFile(string webPath, string filePath)
        {
            if (!String.IsNullOrEmpty(webPath) && !File.Exists(filePath))
            {
                string directoryName = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryName))
                    FileHelper.CreateDirectory(directoryName);

                webDownloader.DownloadFile(webPath, filePath);
            }
        }

        private void UploadProjectPhotos()
        {
            string directoryPath = interactionRequestService.FolderBrowser(Path.GetDirectoryName(untappdService.FilePath));
            if (String.IsNullOrEmpty(directoryPath))
                return;

            CallBackConteiner<List<long>> callBackConteiner = new CallBackConteiner<List<long>>();
            eventAggregator.GetEvent<RequestCheckinsEvent>().Publish(callBackConteiner);
            UploadProjectPhotosAsync(callBackConteiner.Content, directoryPath);
        }

        private async void UploadProjectPhotosAsync(List<long> checkinIds, string uploadDirectory)
        {
            for (int i = 0; i < checkinIds.Count; i++)
            {
                Checkin checkin = untappdService.GetCheckin(checkinIds[i]);
                if (checkin == null || String.IsNullOrEmpty(checkin.UrlPhoto))
                    continue;

                string message = $"{Properties.Resources.Loading} {i + 1}/{checkinIds.Count}: {checkin.UrlPhoto}";
                interactionRequestService.ShowMessageOnStatusBar(message);
                await Task.Run(() => UploadCheckinPhoto(checkin, uploadDirectory));
            }
            interactionRequestService.ShowMessageOnStatusBar(CommunicationHelper.GetLoadingMessage(untappdService.FilePath));
        }

        private void UploadCheckinPhoto(Checkin checkin, string uploadDirectory)
        {
            string photoPath = untappdService.GetCheckinPhotoFilePath(checkin);
            if (!File.Exists(photoPath))
                webDownloader.DownloadFile(checkin.UrlPhoto, photoPath);

            string targetPath = Path.Combine(uploadDirectory, untappdService.GetUploadSavePhotoFileName(checkin));
            if (File.Exists(targetPath))
                return;

            SavePhoto(photoPath, targetPath, checkin);
        }

        private void SavePhoto(string soursePath, string targetPath, Checkin checkin)
        {
            Image image = Image.FromFile(soursePath);
            PropertyItem propertyItem = image.PropertyItems[0];
            FileHelper.SetProperty(propertyItem, FileHelper.ExifImageDateTimeOriginal, checkin.CreatedDate.ToString("yyyy/MM/dd"));
            image.SetPropertyItem(propertyItem);
            image.Save(targetPath);
        }

        private void SaveСhangesProject()
        {
            if (!untappdService.IsDirtyUntappd())
                return;

            if (!interactionRequestService.Ask(Properties.Resources.Warning, Properties.Resources.AskSaveСhangesUntappdProject))
                return;

            switch (FileHelper.GetExtensionWihtoutPoint(untappdService.FilePath))
            {
                case Extensions.CSV:
                    SaveAsProject();
                    break;

                case Extensions.UNTP:
                    FileHelper.SaveFile(untappdService.FilePath, untappdService.Untappd);
                    break;
                default:
                    throw new ArgumentException(String.Format(Properties.Resources.ArgumentExceptioSaveUntappdProject, untappdService.FilePath));
            }
            untappdService.ResetСhanges();
        }

        private void WebDownloadProject()
        {
            if (!FileHelper.GetExtensionWihtoutPoint(untappdService.FilePath).Equals(Extensions.UNTP))
            {
                interactionRequestService.ShowMessage(Properties.Resources.Warning, Properties.Resources.WarningMessageSaveProjectToUNTP);
                return;
            }

            moduleManager.LoadModule(typeof(WebDownloadProjectModule).Name);
            ActivateView(RegionNames.MainRegion, typeof(Views.WebDownloadProject));
        }

        private void CheckinsProjectReport()
        {
            string reportPath = Path.Combine(untappdService.GetFileDataDirectory(), $"{untappdService.Untappd.UserName}.xlsx");
            reportingService.CreateAllCheckinsReport(untappdService.GetCheckins(), reportPath);
            try
            {
                System.Diagnostics.Process.Start(reportPath);
            }
            catch (Exception ex)
            {
                interactionRequestService.ShowError(Properties.Resources.Error, StringHelper.GetFullExceptionMessage(ex));
            }
        }
    }
}