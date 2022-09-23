using Prism.Ioc;
using Prism.Regions;
using UntappdViewer.Views;

namespace UntappdViewer.Modules
{
    public class GalleryProjectModule : BaseModue
    {
        public GalleryProjectModule(IRegionManager regionManager) : base(regionManager)
        {
        }

        public override void OnInitialized(IContainerProvider containerProvider)
        {
            base.OnInitialized(containerProvider);
            regionManager.RegisterViewWithRegion(RegionNames.MainRegion, () => containerProvider.Resolve<GalleryProject>());
        }
    }
}