using Prism.Ioc;
using Prism.Regions;
using UntappdViewer.Views;

namespace UntappdViewer.Modules
{
    public class WebDownloadProjectModule : BaseModue
    {
        public WebDownloadProjectModule(IRegionManager regionManager) : base(regionManager)
        {
        }

        public override void OnInitialized(IContainerProvider containerProvider)
        {
            base.OnInitialized(containerProvider);
            regionManager.RegisterViewWithRegion(RegionNames.RootRegion, () => containerProvider.Resolve<WebDownloadProject>());
        }
    }
}