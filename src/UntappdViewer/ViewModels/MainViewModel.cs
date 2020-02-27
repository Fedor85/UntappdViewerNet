using Prism.Events;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Events;
using UntappdViewer.Modules;
using UntappdViewer.Views;

namespace UntappdViewer.ViewModels
{
    public class MainViewModel: RegionManagerBaseModel
    {
        private IModuleManager moduleManager;

        private IEventAggregator eventAggregator;

        public MainViewModel(IModuleManager moduleManager, IRegionManager regionManager,
                                                           IEventAggregator eventAggregator) : base(regionManager)
        {
            this.moduleManager = moduleManager;
            this.eventAggregator = eventAggregator;
        }

        protected override void Activate()
        {
            base.Activate();
            moduleManager.LoadModule(typeof(MenuBarModule).Name);
            moduleManager.LoadModule(typeof(UntappdModule).Name);
            moduleManager.LoadModule(typeof(StatusBarModule).Name);

            ActivateView(RegionNames.StatusBarRegion, typeof(StatusBar));
            ActivateView(RegionNames.MainRegion, typeof(Untappd));
        }

        protected override void DeActivate()
        {
            base.DeActivate();
            eventAggregator.GetEvent<SaveUntappdToFileEvent>().Publish();
            DeActivateAllViews(RegionNames.MainRegion);
            DeActivateAllViews(RegionNames.StatusBarRegion);
        }
    }
}