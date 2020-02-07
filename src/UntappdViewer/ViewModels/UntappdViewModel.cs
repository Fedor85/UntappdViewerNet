using System;
using System.Windows;
using Prism;
using Prism.Mvvm;
using UntappdViewer.Properties;

namespace UntappdViewer.ViewModels
{
    public class UntappdViewModel : BindableBase, IActiveAware
    {
        private bool active;

        private GridLength treeRegionWidth;

        public GridLength TreeRegionWidth
        {
            get { return treeRegionWidth; }
            set
            {
                treeRegionWidth = value;
                OnPropertyChanged("TreeRegionWidth");
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

        private void DeActivate()
        {
            Settings.Default.TreeRegionWidth = TreeRegionWidth.Value;
            Settings.Default.Save();
        }

        private void Activate()
        {
            double width = Settings.Default.TreeRegionWidth;
            if (width > 0)
                TreeRegionWidth = new GridLength(width);
        }

        public event EventHandler IsActiveChanged;
    }
}
