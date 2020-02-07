using System;
using System.Windows;
using Prism;
using Prism.Mvvm;
using UntappdViewer.Interfaces.Services;

namespace UntappdViewer.ViewModels
{
    public class UntappdViewModel : BindableBase, IActiveAware
    {
        private ISettingService settingService;

        private bool active;

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

        public bool IsActive
        {
            get
            {
                return active;
            }
            set
            {
                if (active != value)
                {
                    active = value;
                    if (active)
                        Activate();
                    else
                        DeActivate();
                }
            }
        }

        public UntappdViewModel(ISettingService settingService)
        {
            this.settingService = settingService;
        }

        private void DeActivate()
        {
            settingService.SetTreeRegionWidth(TreeRegionWidth.Value);
        }

        private void Activate()
        {
            TreeRegionWidth = new GridLength(settingService.GetTreeRegionWidth());
        }

        public event EventHandler IsActiveChanged;
    }
}
