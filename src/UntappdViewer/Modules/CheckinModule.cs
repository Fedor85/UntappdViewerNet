using Prism.Ioc;
using Prism.Regions;
using UntappdViewer.Views;

namespace UntappdViewer.Modules
{
    public class CheckinModule: BaseModue
    {
        public CheckinModule(IRegionManager regionManager) : base(regionManager)
        {
        }
        public override void OnInitialized(IContainerProvider containerProvider)
        {
            base.OnInitialized(containerProvider);
            regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, () => containerProvider.Resolve<Checkin>());
        }
    }
}