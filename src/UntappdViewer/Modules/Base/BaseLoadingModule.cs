using Prism.Ioc;
using Prism.Regions;
using UntappdViewer.Views;

namespace UntappdViewer.Modules
{
    public class BaseLoadingModule: BaseModue
    {
        private readonly string regionName;

        public BaseLoadingModule(IRegionManager regionManager, string loadingRegionName) : base(regionManager)
        {
            regionName = loadingRegionName;
        }

        public override void OnInitialized(IContainerProvider containerProvider)
        {
            base.OnInitialized(containerProvider);
            regionManager.RegisterViewWithRegion(regionName, () => containerProvider.Resolve<Loading>());
        }
    }
}