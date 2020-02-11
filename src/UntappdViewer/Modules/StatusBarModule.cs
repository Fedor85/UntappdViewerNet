using Prism.Ioc;
using Prism.Regions;
using UntappdViewer.Views;

namespace UntappdViewer.Modules
{
    public class StatusBarModule : BaseModue
    {
        public StatusBarModule(IRegionManager regionManager) : base(regionManager)
        {
        }

        public override void OnInitialized(IContainerProvider containerProvider)
        {
            base.OnInitialized(containerProvider);
            // call Activate
            regionManager.Regions[RegionNames.StatusBarRegion].Add(containerProvider.Resolve<StatusBar>());
        }
    }
}