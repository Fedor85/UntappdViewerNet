using Prism.Events;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Events;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Modules;
using UntappdViewer.Views;

namespace UntappdViewer.ViewModels
{
    public class MainViewModel: RegionManagerBaseModel
    {
        private IModuleManager moduleManager;

        private IEventAggregator eventAggregator;

        private ISettingService settingService;

        public MainViewModel(IModuleManager moduleManager, IRegionManager regionManager,
                                                           IEventAggregator eventAggregator,
                                                           ISettingService settingService) : base(regionManager)
        {
            this.moduleManager = moduleManager;
            this.eventAggregator = eventAggregator;
            this.settingService = settingService;
        }

        protected override void Activate()
        {
            base.Activate();

            settingService.SetStartWelcomeView(false);

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