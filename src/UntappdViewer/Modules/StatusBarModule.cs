using System.Linq;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Views;

namespace UntappdViewer.Modules
{
    public class StatusBarModule : BaseModue
    {
        private IModuleManager moduleManager;

        public StatusBarModule(IRegionManager regionManager, IModuleManager moduleManager) : base(regionManager)
        {
            this.moduleManager = moduleManager;
        }

        public override void OnInitialized(IContainerProvider containerProvider)
        {
            base.OnInitialized(containerProvider);
            // call Activate
            regionManager.Regions[RegionNames.StatusBarRegion].Add(containerProvider.Resolve<StatusBar>());
        }
    }
}