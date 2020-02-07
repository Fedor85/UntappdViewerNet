using System.Windows;
using UntappdViewer.Interfaces.Services;

namespace UntappdViewer.ViewModels
{
    public class UntappdViewModel: ActiveAwareBaseModel
    {
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

        public UntappdViewModel(ISettingService settingService)
        {
            this.settingService = settingService;
        }

        protected override void Activate()
        {
            base.Activate();
            TreeRegionWidth = new GridLength(settingService.GetTreeRegionWidth());
        }

        protected override void DeActivate()
        {
            base.DeActivate();
            settingService.SetTreeRegionWidth(TreeRegionWidth.Value);
        }
    }
}