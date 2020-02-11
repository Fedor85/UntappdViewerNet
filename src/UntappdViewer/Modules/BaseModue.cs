using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace UntappdViewer.Modules
{
    public class BaseModue :IModule
    {
        protected IRegionManager regionManager;

        public BaseModue(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }

        public virtual void OnInitialized(IContainerProvider containerProvider)
        {
        }
    }
}