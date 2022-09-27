using Prism.Ioc;
using Prism.Regions;
using UntappdViewer.Views;

namespace UntappdViewer.Modules
{
    public class StatisticsProjectModule : BaseModue
    {
        public StatisticsProjectModule(IRegionManager regionManager) : base(regionManager)
        {
        }

        public override void OnInitialized(IContainerProvider containerProvider)
        {
            base.OnInitialized(containerProvider);
            regionManager.RegisterViewWithRegion(RegionNames.MainRegion, () => containerProvider.Resolve<StatisticsProject>());
        }
    }
}