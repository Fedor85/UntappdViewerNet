using System.Linq;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Modules;
using UntappdViewer.Views;

namespace UntappdViewer.ViewModels
{
    public abstract class LoadingBaseModel: RegionManagerBaseModel
    {
        protected IModuleManager moduleManager;

        protected string loadingModuleName;

        protected string loadingRegionName;

        protected LoadingBaseModel(IModuleManager moduleManager, IRegionManager regionManager) : base(regionManager)
        {
            this.moduleManager = moduleManager;
            loadingModuleName = typeof(LoadingModule).Name;
            loadingRegionName = RegionNames.LoadingRegion;
        }

        protected void LoadingChangeActivity(bool isActivate)
        {
            moduleManager.LoadModule(loadingModuleName);
            IRegion region = regionManager.Regions[loadingRegionName];
            if (isActivate)
            {
                object view = region.Views.First(i => i.GetType().Equals(typeof(Loading)));
                region.Activate(view);
            }
            else
            {
                foreach (object activeView in region.ActiveViews)
                    region.Deactivate(activeView);
            }
        }
    }
}