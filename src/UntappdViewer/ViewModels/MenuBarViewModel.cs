using System;
using System.IO;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Domain;
using UntappdViewer.Domain.Services;
using UntappdViewer.Events;
using UntappdViewer.Infrastructure;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Modules;
using UntappdViewer.Services;
using UntappdViewer.Services.PopupWindowAction;
using UntappdViewer.Views;

namespace UntappdViewer.ViewModels
{
    public class MenuBarViewModel: RegionManagerBaseModel
    {
        private UntappdService untappdService;

        private InteractionRequestService interactionRequestService;

        private ISettingService settingService;

        private IModuleManager moduleManager;

        private IEventAggregator eventAggregator;

        public ICommand GoToWelcomeCommand { get; }

        public ICommand RenameProjectCommand { get; }

        public ICommand SaveProjectCommand { get; }

        public ICommand SaveAsProjectCommand { get; }

        public MenuBarViewModel(UntappdService untappdService, InteractionRequestService interactionRequestService,
                                                                ISettingService settingService,
                                                                IModuleManager moduleManager,
                                                                IRegionManager regionManager,
                                                                IEventAggregator eventAggregator) : base(regionManager)
        {
            this.interactionRequestService = interactionRequestService;
            this.settingService = settingService;
            this.untappdService = untappdService;
            this.moduleManager = moduleManager;
            this.eventAggregator = eventAggregator;

            GoToWelcomeCommand = new DelegateCommand(GoToWelcome);
            RenameProjectCommand = new DelegateCommand(RenameProject);
            SaveProjectCommand = new DelegateCommand(SaveProject);
            SaveAsProjectCommand = new DelegateCommand(SaveAsProject);
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
            ActivateView(RegionNames.RootRegion, typeof(Welcome));
        }

        private void RenameProject()
        {
            ConfirmationResult<string> confirmationResult = interactionRequestService.AskReplaceText(Properties.Resources.RenameProject, untappdService.GetUntappdUserName());
            if (!confirmationResult.Result)
                return;

            untappdService.UpdateUntappdUserName(confirmationResult.Value);

        }

        private void SaveСhangesProject()
        {
            if (!untappdService.IsDirtyUntappd())
                return;

            if (!interactionRequestService.Ask(Properties.Resources.Warning, Properties.Resources.AskSaveСhangesUntappdProject))
                return;

            switch (FileHelper.GetExtensionWihtoutPoint(untappdService.FIlePath))
            {
                case Extensions.CSV:
                    SaveAsProject();
                    break;

                case Extensions.UNTP:
                    FileHelper.SaveFile(untappdService.FIlePath, untappdService.Untappd);
                    break;
                default:
                    throw new ArgumentException(String.Format(Properties.Resources.ArgumentExceptioSaveUntappdProject, untappdService.FIlePath));
            }
            untappdService.ResetСhanges();
        }

        private void SaveProject()
        {
            if (!FileHelper.GetExtensionWihtoutPoint(untappdService.FIlePath).Equals(Extensions.UNTP))
            {
                SaveAsProject();
                return;
            }

            if (!untappdService.IsDirtyUntappd())
                return;

            if (!interactionRequestService.Ask(Properties.Resources.Warning, Properties.Resources.AskSaveСhangesUntappdProject))
                return;

            FileHelper.SaveFile(untappdService.FIlePath, untappdService.Untappd);
            untappdService.ResetСhanges();
        }

        private void SaveAsProject()
        {
            string fileSavePath = interactionRequestService.SaveFile(String.IsNullOrEmpty(untappdService.FIlePath) ? String.Empty : Path.GetDirectoryName(untappdService.FIlePath), untappdService.GetUntappdProjectFileName(), Extensions.GetSaveExtensions());
            if (String.IsNullOrEmpty(fileSavePath))
                return;

            FileHelper.SaveFile(fileSavePath, untappdService.Untappd);
            untappdService.ResetСhanges();
            FileHelper.CreateDirectory(Path.Combine(Path.GetDirectoryName(fileSavePath), untappdService.GetUntappdProjectPhotoFilesDirectory(Path.GetFileNameWithoutExtension(fileSavePath))));
            untappdService.Initialize(fileSavePath);
            interactionRequestService.ShowMessageOnStatusBar(untappdService.FIlePath);
            settingService.SetRecentFilePaths(FileHelper.AddFilePath(settingService.GetRecentFilePaths(), fileSavePath, settingService.GetMaxRecentFilePaths()));
        }
    }
}