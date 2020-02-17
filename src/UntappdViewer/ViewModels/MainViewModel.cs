using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Modules;
using UntappdViewer.Views;

namespace UntappdViewer.ViewModels
{
    public class MainViewModel: RegionManagerBaseModel
    {
        private IModuleManager moduleManager;

        public MainViewModel(IModuleManager moduleManager, IRegionManager regionManager): base(regionManager)
        {
            this.moduleManager = moduleManager;
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
            DeActivateAllViews(RegionNames.MainRegion);
            DeActivateAllViews(RegionNames.StatusBarRegion);
        }
    }
}