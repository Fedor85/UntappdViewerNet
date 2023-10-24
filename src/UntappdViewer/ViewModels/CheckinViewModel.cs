using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Events;
using UntappdViewer.Helpers;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Models;
using UntappdViewer.Modules;
using UntappdViewer.Utils;
using CheckinVM = UntappdViewer.ViewModels.Controls.CheckinViewModel;

namespace UntappdViewer.ViewModels
{
    public class CheckinViewModel : LoadingBaseModel
    {
        private IUntappdService untappdService;

        private IWebDownloader webDownloader;

        private IInteractionRequestService interactionRequestService;

        private readonly CheckinVM emptyCheckin = CheckinVM.GetEmpty();

        private CheckinVM checkin;

        public CheckinVM Checkin
        {
            get { return checkin; }
            set { SetProperty(ref checkin, value); }
        }

        public ICommand CheckinVenueLocationCommand { get; }


        public CheckinViewModel(IUntappdService untappdService, IWebDownloader webDownloader,
                                                                IEventAggregator eventAggregator,
                                                                IModuleManager moduleManager,
                                                                IRegionManager regionManager,
                                                                IInteractionRequestService interactionRequestService) : base(moduleManager, regionManager, eventAggregator)
        {
            this.untappdService = untappdService;
            this.webDownloader = webDownloader;
            this.interactionRequestService = interactionRequestService;

            loadingModuleName = typeof(CheckinLoadingModule).Name;
            loadingRegionName = RegionNames.CheckinLoadingRegion;
            CheckinVenueLocationCommand  = new DelegateCommand(CheckinVenueLocation);
        }

        private void CheckinVenueLocation()
        {

        }

        protected override void Activate()
        {
            base.Activate();
            eventAggregator.GetEvent<ChekinUpdateEvent>().Subscribe(FillChekin);
        }

        protected override void DeActivate()
        {
            base.DeActivate();
            Checkin = null;
            eventAggregator.GetEvent<ChekinUpdateEvent>().Unsubscribe(FillChekin);
        }

        private void FillChekin(Checkin checkin)
        {
            Checkin = emptyCheckin;
            if (checkin == null)
                return;

            LoadingChangeActivity(true);
            FillChekinAsuc(checkin);
        }

        private async void FillChekinAsuc(Checkin checkin)
        {
            try
            {
                await Task.Run(() => untappdService.DownloadMediaFiles(webDownloader, checkin));
                Checkin = Mapper.GetCheckinViewModel(untappdService, checkin);
            }
            catch (Exception ex)
            {
                interactionRequestService.ShowError(Properties.Resources.Error, StringHelper.GetFullExceptionMessage(ex));
            }
            finally
            {
                LoadingChangeActivity(false);
            }
        }
    }
}