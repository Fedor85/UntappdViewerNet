using System.Linq;
using Prism.Events;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Events;
using UntappdViewer.Modules;
using UntappdViewer.Views;

namespace UntappdViewer.ViewModels
{
    public abstract class LoadingBaseModel: RegionManagerBaseModel
    {
        protected IModuleManager moduleManager;

        protected IEventAggregator eventAggregator;

        protected string loadingModuleName;

        protected string loadingRegionName;

        protected LoadingBaseModel(IModuleManager moduleManager, IRegionManager regionManager, IEventAggregator eventAggregator) : base(regionManager)
        {
            this.moduleManager = moduleManager;
            this.eventAggregator = eventAggregator;
            loadingModuleName = typeof(LoadingModule).Name;
            loadingRegionName = RegionNames.LoadingRegion;
        }

        protected void LoadingChangeActivity(bool isActivate, bool activateCancelButton = false)
        {
            moduleManager.LoadModule(loadingModuleName);
            IRegion region = regionManager.Regions[loadingRegionName];
            if (isActivate)
            {
                object view = region.Views.First(i => i.GetType().Equals(typeof(Loading)));
                region.Activate(view);
                if (activateCancelButton)
                    eventAggregator.GetEvent<LoadingActivateCancel>().Publish();
            }
            else
            {
                foreach (object activeView in region.ActiveViews)
                    region.Deactivate(activeView);
            }
        }
    }
}