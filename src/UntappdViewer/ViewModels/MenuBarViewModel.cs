using System.Windows.Input;
using Prism.Commands;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Domain.Services;
using UntappdViewer.Modules;
using UntappdViewer.Views;

namespace UntappdViewer.ViewModels
{
    public class MenuBarViewModel: RegionManagerBaseModel
    {
        private UntappdService untappdService;

        private IModuleManager moduleManager;

        public ICommand GoToWelcomeCommand { get; }

        public ICommand RenameProjectCommand { get; }

        public MenuBarViewModel(UntappdService untappdService, IModuleManager moduleManager, IRegionManager regionManager): base(regionManager)
        {
            this.untappdService = untappdService;
            this.moduleManager = moduleManager;

            GoToWelcomeCommand = new DelegateCommand(GoToWelcome);
            RenameProjectCommand = new DelegateCommand(RenameProject);
        }

        private void GoToWelcome()
        {
            moduleManager.LoadModule(typeof(WelcomeModule).Name);
            ActivateView(RegionNames.RootRegion, typeof(Welcome));
        }

        private void RenameProject()
        {

        }
    }
}