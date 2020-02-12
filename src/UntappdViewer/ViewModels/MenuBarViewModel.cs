using System.Windows.Input;
using Prism.Commands;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Modules;
using UntappdViewer.Views;

namespace UntappdViewer.ViewModels
{
    public class MenuBarViewModel: RegionManagerBaseModel
    {
        private IModuleManager moduleManager;

        public ICommand GoToWelcomeCommand { get; }

        public MenuBarViewModel(IModuleManager moduleManager, IRegionManager regionManager): base(regionManager)
        {
            this.moduleManager = moduleManager;
            GoToWelcomeCommand = new DelegateCommand(GoToWelcome);
        }

        private void GoToWelcome()
        {
            moduleManager.LoadModule(typeof(WelcomeModule).Name);
            ActivateView(RegionNames.RootRegion, typeof(Welcome));
        }
    }
}