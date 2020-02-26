using System.Windows.Input;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Domain.Services;
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

        private IModuleManager moduleManager;

        public ICommand GoToWelcomeCommand { get; }

        public ICommand RenameProjectCommand { get; }

        public ICommand SaveProjectCommand { get; }

        public ICommand SaveAsProjectCommand { get; }

        public MenuBarViewModel(UntappdService untappdService, InteractionRequestService interactionRequestService,
                                                                IModuleManager moduleManager,
                                                                IRegionManager regionManager): base(regionManager)
        {
            this.untappdService = untappdService;
            this.moduleManager = moduleManager;
            this.interactionRequestService = interactionRequestService;
            GoToWelcomeCommand = new DelegateCommand(GoToWelcome);
            RenameProjectCommand = new DelegateCommand(RenameProject);
            SaveProjectCommand = new DelegateCommand(SaveProject);
            SaveAsProjectCommand = new DelegateCommand(SaveAsProject);
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

        private void SaveProject()
        {
        }

        private void SaveAsProject()
        {
        }
    }
}