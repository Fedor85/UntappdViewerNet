using Prism.Regions;

namespace UntappdViewer.Modules
{
    public class CheckinLoadingModule : BaseLoadingModule
    {
        public CheckinLoadingModule(IRegionManager regionManager) : base(regionManager, RegionNames.CheckinLoadingRegion)
        {
        }
    }
}