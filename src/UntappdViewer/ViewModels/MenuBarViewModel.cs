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
using UntappdViewer.Modules;
using UntappdViewer.Utils;
using UntappdViewer.Views;
using Checkin = UntappdViewer.Models.Checkin;

namespace UntappdViewer.ViewModels
{
    public class MenuBarViewModel: LoadingBaseModel
    {
        private IUntappdService untappdService;

        private IStatisticsCalculation statisticsCalculation;

        private IInteractionRequestService interactionRequestService;

        private ISettingService settingService;

        private IModuleManager moduleManager;

        private IWebDownloader webDownloader;

        private IReportingService reportingService;

        public ICommand GoToWelcomeCommand { get; }

        public ICommand RenameProjectCommand { get; }

        public ICommand SaveProjectCommand { get; }

        public ICommand SaveAsProjectCommand { get; }

        public ICommand SaveAsZipArchiveCommand { get; }

        public ICommand DownloadProjectMediaCommand { get; }

        public ICommand UploadProjectPhotoCommand { get; }

        public ICommand WebDownloadProjectCommand { get; }

        public ICommand GalleryProjectCommand { get; }

        public ICommand StatisticsProjectCommand { get; }

        public ICommand CheckinsProjectReportCommand { get; }

        public ICommand StatisticsProjectReportCommand { get; }

        public ICommand SettingsCommand { get; }

        public ICommand AboutCommand { get; }

        public MenuBarViewModel(IUntappdService untappdService,  IInteractionRequestService interactionRequestService,
                                                                 IStatisticsCalculation statisticsCalculation,
                                                                 ISettingService settingService,
                                                                 IModuleManager moduleManager,
                                                                 IRegionManager regionManager,
                                                                 IEventAggregator eventAggregator,
                                                                 IWebDownloader webDownloader,
                                                                 IReportingService reportingService) : base(moduleManager, regionManager, eventAggregator)
        {
            this.interactionRequestService = interactionRequestService;
            this.statisticsCalculation = statisticsCalculation;
            this.settingService = settingService;
            this.untappdService = untappdService;
            this.moduleManager = moduleManager;
            this.webDownloader = webDownloader;
            this.reportingService = reportingService;

            GoToWelcomeCommand = new DelegateCommand(GoToWelcome);
            RenameProjectCommand = new DelegateCommand(RenameProject);
            SaveProjectCommand = new DelegateCommand(SaveProject);
            SaveAsProjectCommand = new DelegateCommand(SaveAsProject);
            SaveAsZipArchiveCommand = new DelegateCommand(SaveAsZipArchive);
            DownloadProjectMediaCommand = new DelegateCommand(DownloadProjectMedia);
            UploadProjectPhotoCommand = new DelegateCommand(UploadProjectPhotos);
            WebDownloadProjectCommand = new DelegateCommand(WebDownloadProject);
            GalleryProjectCommand = new DelegateCommand(GalleryProject);
            StatisticsProjectCommand = new DelegateCommand(StatisticsProject);
            CheckinsProjectReportCommand = new DelegateCommand(CheckinsProjectReport);
            StatisticsProjectReportCommand = new DelegateCommand(StatisticsProjectReport);
            SettingsCommand = new DelegateCommand(Settings);
            AboutCommand = new DelegateCommand(About);
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
            if (!untappdService.IsUNTPProject())
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
            untappdService.Initialize(fileSavePath, settingService.GetDefaultUserName());
            untappdService.ResetСhanges();
            FileHelper.CreateDirectory(untappdService.GetFileDataDirectory());
            ShowMessageOnStatusBar(untappdService.FilePath);
            settingService.SetRecentFilePaths(FileHelper.AddFilePath(settingService.GetRecentFilePaths(), fileSavePath, settingService.GetMaxRecentFilePaths()));
        }

        private void SaveAsZipArchive()
        {
            if (!untappdService.IsUNTPProject())
            {
                interactionRequestService.ShowMessage(Properties.Resources.Warning, Properties.Resources.WarningMessageSaveProjectToUNTP);
                return;
            }
            LoadingChangeActivity(true, true);
            SaveAsZipArchiveAsync();
        }

        private async void SaveAsZipArchiveAsync()
        {
            string previousMessageOnStatusBar = interactionRequestService.GetCurrentMessageOnStatusBar();
            string mainFilePath = untappdService.FilePath;

            ZipFileHelper zipFileHelper = new ZipFileHelper();
            zipFileHelper.ZipProgress += ShowMessageOnStatusBar;
            zipFileHelper.AddFile(mainFilePath);
            zipFileHelper.AddDirectory(untappdService.GetFileDataDirectory());
            eventAggregator.GetEvent<LoadingCancel>().Subscribe(zipFileHelper.CancellationTokenSource.Cancel);

            string resultPath = ZipFileHelper.GetResultPath(mainFilePath);
            try
            {
                await Task.Run(() => zipFileHelper.SaveAsZip(resultPath));
            }
            catch (Exception ex)
            {
                interactionRequestService.ShowError(Properties.Resources.Error, StringHelper.GetFullExceptionMessage(ex));
            }
            finally
            {
                ShowMessageOnStatusBar(previousMessageOnStatusBar);
                eventAggregator.GetEvent<LoadingCancel>().Unsubscribe(zipFileHelper.CancellationTokenSource.Cancel);
                LoadingChangeActivity(false);
            }
        }

        private void ShowMessageOnStatusBar(string message)
        {
            interactionRequestService.ShowMessageOnStatusBar(message);
        }

        private void DownloadProjectMedia()
        {
            if (!untappdService.IsUNTPProject())
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
                ShowMessageOnStatusBar(CommunicationHelper.GetLoadingMessage(counter++, count, checkin.Beer.Name));
                await Task.Run(() => untappdService.DownloadMediaFiles(webDownloader, checkin));
            }
            ShowMessageOnStatusBar(CommunicationHelper.GetLoadingMessage(untappdService.FilePath));
        }

        private void UploadProjectPhotos()
        {
            if (!untappdService.IsUNTPProject())
            {
                interactionRequestService.ShowMessage(Properties.Resources.Warning, Properties.Resources.WarningMessageSaveProjectToUNTP);
                return;
            }

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
                ShowMessageOnStatusBar(message);
                await Task.Run(() => UploadCheckinPhoto(checkin, uploadDirectory));
            }
            ShowMessageOnStatusBar(CommunicationHelper.GetLoadingMessage(untappdService.FilePath));
        }

        private void UploadCheckinPhoto(Checkin checkin, string uploadDirectory)
        {
            string photoPath = untappdService.GetCheckinPhotoFilePath(checkin);
            if (!File.Exists(photoPath))
            {
                if (!webDownloader.DownloadToFile(checkin.UrlPhoto, photoPath))
                    return;
            }

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
            if (!untappdService.IsUNTPProject())
            {
                interactionRequestService.ShowMessage(Properties.Resources.Warning, Properties.Resources.WarningMessageSaveProjectToUNTP);
                return;
            }

            moduleManager.LoadModule(typeof(WebDownloadProjectModule).Name);
            ActivateView(RegionNames.MainRegion, typeof(WebDownloadProject));
        }

        private void GalleryProject()
        {
            if (!untappdService.IsUNTPProject())
            {
                interactionRequestService.ShowMessage(Properties.Resources.Warning, Properties.Resources.WarningMessageSaveProjectToUNTP);
                return;
            }
            moduleManager.LoadModule(typeof(GalleryProjectModule).Name);
            ActivateView(RegionNames.MainRegion, typeof(GalleryProject));
        }

        private void StatisticsProject()
        {
            moduleManager.LoadModule(typeof(StatisticsProjectModule).Name);
            ActivateView(RegionNames.MainRegion, typeof(StatisticsProject));
        }

        private void CheckinsProjectReport()
        {
            if (!untappdService.IsUNTPProject())
            {
                interactionRequestService.ShowMessage(Properties.Resources.Warning, Properties.Resources.WarningMessageSaveProjectToUNTP);
                return;
            }

            LoadingChangeActivity(true);
            CheckinsProjectReportAsync();
        }

        private void StatisticsProjectReport()
        {
            if (!untappdService.IsUNTPProject())
            {
                interactionRequestService.ShowMessage(Properties.Resources.Warning, Properties.Resources.WarningMessageSaveProjectToUNTP);
                return;
            }

            LoadingChangeActivity(true);
            StatisticsProjectReportAsync();
        }

        private async void StatisticsProjectReportAsync()
        {
            try
            {
                string reportPath = await reportingService.CreateStatisticsReportAsync(statisticsCalculation, untappdService.GetReportsDirectory());
                System.Diagnostics.Process.Start(reportPath);
            }
            catch (Exception ex)
            {
                interactionRequestService.ShowError(Properties.Resources.Error, StringHelper.GetFullExceptionMessage(ex));
            }
            finally
            {
                LoadingChangeActivity(false);
            }
          
        }

        private async void CheckinsProjectReportAsync()
        {
            try
            {
                string reportPath = await reportingService.CreateAllCheckinsReportrAsync(untappdService.GetCheckins(),
                                                                                         untappdService.GetReportsDirectory());
                System.Diagnostics.Process.Start(reportPath);
            }
            catch (Exception ex)
            {
                interactionRequestService.ShowError(Properties.Resources.Error, StringHelper.GetFullExceptionMessage(ex));
            }
            finally
            {
                LoadingChangeActivity(false);
            }
        }

        private void Settings()
        {
            moduleManager.LoadModule(typeof(SettingsModule).Name);
            ActivateView(RegionNames.MainRegion, typeof(Settings));
        }

        private void About()
        {
            About about = new About();
            about.ShowDialog();
        }
    }
}