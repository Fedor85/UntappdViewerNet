using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Views;

namespace UntappdViewer.Modules
{
    public class StatusBarModule: IModule
    {
        private IRegionManager regionManager;

        public StatusBarModule(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

    
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            regionManager.RegisterViewWithRegion(RegionNames.StatusBarRegion, () => containerProvider.Resolve<StatusBar>());
        }
    }
}