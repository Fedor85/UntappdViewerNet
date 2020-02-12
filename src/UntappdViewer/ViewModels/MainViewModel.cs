﻿using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Modules;
using UntappdViewer.Services;
using UntappdViewer.Views;

namespace UntappdViewer.ViewModels
{
    public class MainViewModel: RegionManagerBaseModel
    {
        private UntappdService untappdService;

        private IModuleManager moduleManager;


        private ICommunicationService communicationService;

        public MainViewModel(UntappdService untappdService, IModuleManager moduleManager, IRegionManager regionManager, ICommunicationService communicationService): base(regionManager)
        {
            this.untappdService = untappdService;
            this.moduleManager = moduleManager;
            this.communicationService = communicationService;
        }

        protected override void Activate()
        {
            base.Activate();
            moduleManager.LoadModule(typeof(MenuBarModule).Name);
            moduleManager.LoadModule(typeof(UntappdModule).Name);
            moduleManager.LoadModule(typeof(StatusBarModule).Name);

            ActivateView(RegionNames.MainRegion, typeof(Untappd));
            ActivateView(RegionNames.StatusBarRegion, typeof(StatusBar));

            communicationService.ShowMessageOnStatusBar(CommunicationHelper.GetLoadingMessage(untappdService.FIlePath));
        }

        protected override void DeActivate()
        {
            base.DeActivate();
            DeActivateAllViews(RegionNames.MainRegion);
            DeActivateAllViews(RegionNames.StatusBarRegion);
        }
    }
}