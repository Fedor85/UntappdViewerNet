using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Domain;
using UntappdViewer.Domain.Services;
using UntappdViewer.Events;
using UntappdViewer.Helpers;
using UntappdViewer.Infrastructure;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Models;
using UntappdViewer.Modules;
using UntappdViewer.Services;

namespace UntappdViewer.ViewModels
{
    public class MenuBarViewModel: RegionManagerBaseModel
    {
        private UntappdService untappdService;

        private InteractionRequestService interactionRequestService;

        private ISettingService settingService;

        private IModuleManager moduleManager;

        private IEventAggregator eventAggregator;

        private IWebDownloader webDownloader;

        public ICommand GoToWelcomeCommand { get; }

        public ICommand RenameProjectCommand { get; }

        public ICommand SaveProjectCommand { get; }

        public ICommand SaveAsProjectCommand { get; }

        public ICommand UploadProjectPhotoCommand { get; }

        public MenuBarViewModel(UntappdService untappdService, InteractionRequestService interactionRequestService,
                                                                ISettingService settingService,
                                                                IModuleManager moduleManager,
                                                                IRegionManager regionManager,
                                                                IEventAggregator eventAggregator,
                                                                IWebDownloader webDownloader) : base(regionManager)
        {
            this.interactionRequestService = interactionRequestService;
            this.settingService = settingService;
            this.untappdService = untappdService;
            this.moduleManager = moduleManager;
            this.eventAggregator = eventAggregator;
            this.webDownloader = webDownloader;

            GoToWelcomeCommand = new DelegateCommand(GoToWelcome);
            RenameProjectCommand = new DelegateCommand(RenameProject);
            SaveProjectCommand = new DelegateCommand(SaveProject);
            SaveAsProjectCommand = new DelegateCommand(SaveAsProject);
            UploadProjectPhotoCommand = new DelegateCommand(UploadProjectPhotos);
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
            string oldName = untappdService.GetUntappdUserName();
            string newName = interactionRequestService.AskReplaceText(Properties.Resources.RenameProject, oldName);
            if (!oldName.Equals(newName))
                untappdService.UpdateUntappdUserName(newName);
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
            FileHelper.CreateDirectory(untappdService.GetFullUntappdProjectPhotoFilesDirectory());
            interactionRequestService.ShowMessageOnStatusBar(untappdService.FilePath);
            settingService.SetRecentFilePaths(FileHelper.AddFilePath(settingService.GetRecentFilePaths(), fileSavePath, settingService.GetMaxRecentFilePaths()));

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
            string photoPath = untappdService.GetFullCheckinPhotoFilePath(checkin);
            if (!File.Exists(photoPath))
                webDownloader.DownloadFile(checkin.UrlPhoto, photoPath);

            File.Copy(photoPath, Path.Combine(uploadDirectory, untappdService.GetUploadSavePhotoFileName(checkin)), true);
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
    }
}