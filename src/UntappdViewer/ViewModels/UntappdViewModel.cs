using System.Windows;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Modules;
using UntappdViewer.Views;

namespace UntappdViewer.ViewModels
{
    public class UntappdViewModel: RegionManagerBaseModel
    {
        private IModuleManager moduleManager;

        private ISettingService settingService;

        private GridLength treeRegionWidth;

        public GridLength TreeRegionWidth
        {
            get { return treeRegionWidth; }
            set
            {
                treeRegionWidth = value;
                OnPropertyChanged();
            }
        }

        public UntappdViewModel(IModuleManager moduleManager, IRegionManager regionManager, ISettingService settingService) : base(regionManager)
        {
            this.moduleManager = moduleManager;
            this.settingService = settingService;
        }

        protected override void Activate()
        {
            base.Activate();
            TreeRegionWidth = new GridLength(settingService.GetTreeRegionWidth());
            moduleManager.LoadModule(typeof(TreeModue).Name);
            ActivateView(RegionNames.TreeRegion, typeof(Tree));
        }

        protected override void DeActivate()
        {
            base.DeActivate();
            settingService.SetTreeRegionWidth(TreeRegionWidth.Value);
            DeActivateAllViews(RegionNames.TreeRegion);
        }
    }
}