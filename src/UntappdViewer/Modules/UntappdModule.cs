using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Views;

namespace UntappdViewer.Modules
{
    public class UntappdModule: IModule
    {
        private IRegionManager regionManager;

        public UntappdModule(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            regionManager.RegisterViewWithRegion(RegionNames.MainRegion, () => containerProvider.Resolve<Untappd>());
        }
    }
}
