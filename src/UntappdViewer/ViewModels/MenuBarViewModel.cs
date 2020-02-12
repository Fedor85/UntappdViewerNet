using System.Linq;
using System.Windows.Input;
using Prism.Commands;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Modules;
using UntappdViewer.Views;

namespace UntappdViewer.ViewModels
{
    public class MenuBarViewModel
    {
        private IModuleManager moduleManager;

        private IRegionManager regionManager;

        public ICommand GoToWelcomeCommand { get; }

        public MenuBarViewModel(IModuleManager moduleManager, IRegionManager regionManager)
        {
            this.moduleManager = moduleManager;
            this.regionManager = regionManager;
            GoToWelcomeCommand = new DelegateCommand(GoToWelcome);
        }

        private void GoToWelcome()
        {
            moduleManager.LoadModule(typeof(WelcomeModule).Name);
            IRegion rootRegion = regionManager.Regions[RegionNames.RootRegion];
            object welcomeView = rootRegion.Views.First(i => i.GetType().Equals(typeof(Welcome)));
            rootRegion.Activate(welcomeView);
        }
    }
}