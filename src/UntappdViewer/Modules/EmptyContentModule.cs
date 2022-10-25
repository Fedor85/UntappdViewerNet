using Prism.Ioc;
using Prism.Regions;
using UntappdViewer.Views;

namespace UntappdViewer.Modules
{
    public class EmptyContentModule : BaseModue
    {
        public EmptyContentModule(IRegionManager regionManager) : base(regionManager)
        {
        }

        public override void OnInitialized(IContainerProvider containerProvider)
        {
            base.OnInitialized(containerProvider);
            regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, () => containerProvider.Resolve<EmptyContent>());
        }
    }
}