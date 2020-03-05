using Prism.Regions;

namespace UntappdViewer.Modules
{
    public class LoadingModule : BaseLoadingModule
    {
        public LoadingModule(IRegionManager regionManager) : base(regionManager, RegionNames.LoadingRegion)
        {
        }
    }
}