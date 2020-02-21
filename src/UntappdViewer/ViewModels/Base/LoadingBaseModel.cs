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

        protected LoadingBaseModel(IModuleManager moduleManager, IRegionManager regionManager) : base(regionManager)
        {
            this.moduleManager = moduleManager;
        }

        protected void LoadingChangeActivity(bool isActivate)
        {
            moduleManager.LoadModule(typeof(LoadingModule).Name);
            IRegion region = regionManager.Regions[RegionNames.LoadingRegion];
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