using System;
using System.Windows.Controls;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Properties;
using UntappdViewer.Views;

namespace UntappdViewer.Modules
{
    public class ShellModule : IModule
    {
        private IRegionManager regionManager { get; set; }

        public ShellModule(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            UserControl view;
            if(String.IsNullOrEmpty(Settings.Default.OpenFileInitialDirectory))
                view = containerProvider.Resolve<Welcome>();
            else
                view = containerProvider.Resolve<Untappd>();

            regionManager.Regions[RegionNames.RootRegion].Add(view);
        }
    }
}