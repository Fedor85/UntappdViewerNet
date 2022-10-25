using System;
using System.Windows;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Helpers;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Modules;
using UntappdViewer.Views;

namespace UntappdViewer.ViewModels
{
    public class UntappdViewModel: RegionManagerBaseModel
    {
        private IUntappdService untappdService;

        private IModuleManager moduleManager;

        private IInteractionRequestService interactionRequestService;

        private ISettingService settingService;

        private GridLength treeRegionWidth;

        public GridLength TreeRegionWidth
        {
            get { return treeRegionWidth; }
            set
            {
                SetProperty(ref treeRegionWidth, value);
            }
        }

        public UntappdViewModel(IUntappdService untappdService, IModuleManager moduleManager,
                                                                IRegionManager regionManager,
                                                                IInteractionRequestService interactionRequestService,
                                                                ISettingService settingService) : base(regionManager)
        {
            this.untappdService = untappdService;
            this.moduleManager = moduleManager;
            this.interactionRequestService = interactionRequestService;
            this.settingService = settingService;
        }

        protected override void Activate()
        {
            base.Activate();
            TreeRegionWidth = new GridLength(settingService.GetTreeRegionWidth());

            moduleManager.LoadModule(typeof(TreeModue).Name);
            ActivateView(RegionNames.TreeRegion, typeof(Tree));

            if (untappdService.IsEmptyUntappd())
            {
                moduleManager.LoadModule(typeof(EmptyContentModule).Name);
                ActivateView(RegionNames.ContentRegion, typeof(EmptyContent));
            }

            if (!String.IsNullOrEmpty(untappdService.FilePath))
                interactionRequestService.ShowMessageOnStatusBar(CommunicationHelper.GetLoadingMessage(untappdService.FilePath));
        }

        protected override void DeActivate()
        {
            base.DeActivate();
            DeActivateAllViews(RegionNames.TreeRegion);
            settingService.SetTreeRegionWidth(TreeRegionWidth.Value);
        }
    }
}