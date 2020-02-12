using System.Linq;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Modules;
using UntappdViewer.Services;
using UntappdViewer.Views;

namespace UntappdViewer.ViewModels
{
    public class MainViewModel: ActiveAwareBaseModel
    {
        private UntappdService untappdService;

        private IModuleManager moduleManager;

        private IRegionManager regionManager;

        private ICommunicationService communicationService;

        public MainViewModel(UntappdService untappdService, IModuleManager moduleManager, IRegionManager regionManager, ICommunicationService communicationService)
        {
            this.untappdService = untappdService;
            this.moduleManager = moduleManager;
            this.regionManager = regionManager;
            this.communicationService = communicationService;
        }

        protected override void Activate()
        {
            base.Activate();
            moduleManager.LoadModule(typeof(MenuBarModule).Name);
            moduleManager.LoadModule(typeof(UntappdModule).Name);
            moduleManager.LoadModule(typeof(StatusBarModule).Name);

            IRegion statusBarRegion = regionManager.Regions[RegionNames.StatusBarRegion];
            object statusBarView = statusBarRegion.Views.First(i => i.GetType().Equals(typeof(StatusBar)));
            statusBarRegion.Activate(statusBarView);

            communicationService.ShowMessageOnStatusBar(CommunicationHelper.GetLoadingMessage(untappdService.FIlePath));
        }

        protected override void DeActivate()
        {
            base.DeActivate();
            IRegion statusBarRegion = regionManager.Regions[RegionNames.StatusBarRegion];
            foreach (object activeView in statusBarRegion.ActiveViews)
                statusBarRegion.Deactivate(activeView);

        }
    }
}