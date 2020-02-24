using System.Windows;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Domain.Services;
using UntappdViewer.Helpers;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Modules;
using UntappdViewer.Services;
using UntappdViewer.Views;

namespace UntappdViewer.ViewModels
{
    public class UntappdViewModel: RegionManagerBaseModel
    {
        private UntappdService untappdService;

        private IModuleManager moduleManager;

        private InteractionRequestService interactionRequestService;

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

        public UntappdViewModel(UntappdService untappdService, IModuleManager moduleManager,
                                                                IRegionManager regionManager,
                                                                InteractionRequestService interactionRequestService,
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

            interactionRequestService.ShowMessageOnStatusBar(CommunicationHelper.GetLoadingMessage(untappdService.FIlePath));
        }

        protected override void DeActivate()
        {
            base.DeActivate();
            DeActivateAllViews(RegionNames.TreeRegion);
            settingService.SetTreeRegionWidth(TreeRegionWidth.Value);
            untappdService.CleanUpUntappd();
        }
    }
}