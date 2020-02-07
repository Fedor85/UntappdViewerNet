using System;
using System.Windows.Controls;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Views;

namespace UntappdViewer.Modules
{
    public class ShellModule : IModule
    {
        private IRegionManager regionManager;

        private ISettingService settingService;

        public ShellModule(IRegionManager regionManager, ISettingService settingService)
        {
            this.regionManager = regionManager;
            this.settingService = settingService;
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            UserControl view;
            if(String.IsNullOrEmpty(settingService.GetLastOpenedFilePath()))
                view = containerProvider.Resolve<Welcome>();
            else
                view = containerProvider.Resolve<Main>();

            regionManager.Regions[RegionNames.RootRegion].Add(view);
        }
    }
}