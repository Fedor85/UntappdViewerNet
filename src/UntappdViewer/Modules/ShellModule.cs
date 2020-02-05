using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Views;

namespace UntappdViewer.Modules
{
    public class ShellModule : IModule
    {
        private IRegionManager regionManager { get; set; }

        public ShellModule(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            Welcome view = containerProvider.Resolve<Welcome>();
            regionManager.Regions[RegionNames.RootControlRegion].Add(view);
        }
    }
}