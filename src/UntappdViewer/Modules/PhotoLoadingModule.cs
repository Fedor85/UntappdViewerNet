using Prism.Regions;

namespace UntappdViewer.Modules
{
    public class PhotoLoadingModule : BaseLoadingModule
    {
        public PhotoLoadingModule(IRegionManager regionManager) : base(regionManager, RegionNames.PhotoLoadingRegion)
        {
        }
    }
}