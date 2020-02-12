using System.Windows;
using Prism.Modularity;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Modules;

namespace UntappdViewer.ViewModels
{
    public class UntappdViewModel: ActiveAwareBaseModel
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

        public UntappdViewModel(IModuleManager moduleManager, ISettingService settingService)
        {
            this.moduleManager = moduleManager;
            this.settingService = settingService;
        }

        protected override void Activate()
        {
            base.Activate();
            TreeRegionWidth = new GridLength(settingService.GetTreeRegionWidth());
            moduleManager.LoadModule(typeof(TreeModue).Name);
        }

        protected override void DeActivate()
        {
            base.DeActivate();
            settingService.SetTreeRegionWidth(TreeRegionWidth.Value);
        }
    }
}