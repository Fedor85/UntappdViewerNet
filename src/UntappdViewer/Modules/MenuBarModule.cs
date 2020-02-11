using Prism.Ioc;
using Prism.Regions;
using UntappdViewer.Views;

namespace UntappdViewer.Modules
{
    public class MenuBarModule: BaseModue
    {
        public MenuBarModule(IRegionManager regionManager) : base(regionManager)
        {
        }

        public override void OnInitialized(IContainerProvider containerProvider)
        {
            base.OnInitialized(containerProvider);
            // call Activate
            regionManager.RegisterViewWithRegion(RegionNames.MenuRegion, () => containerProvider.Resolve<MenuBar>());
        }
    }
}