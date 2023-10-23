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
using CheckinVM = UntappdViewer.ViewModels.Controls.CheckinViewModel;

namespace UntappdViewer.ViewModels
{
    public class CheckinViewModel : LoadingBaseModel
    {
        private IUntappdService untappdService;

        private IWebDownloader webDownloader;

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
                                                                IRegionManager regionManager) : base(moduleManager, regionManager, eventAggregator)
        {
            this.untappdService = untappdService;
            this.webDownloader = webDownloader;

            loadingModuleName = typeof(PhotoLoadingModule).Name;
            loadingRegionName = RegionNames.PhotoLoadingRegion;
            CheckinVenueLocationCommand  = new DelegateCommand(CheckinVenueLocation);
        }

        private void CheckinVenueLocation()
        {

        }

        protected override void Activate()
        {
            base.Activate();
            eventAggregator.GetEvent<ChekinUpdateEvent>().Subscribe(ChekinUpdate);
        }

        protected override void DeActivate()
        {
            base.DeActivate();
            Checkin = null;
            eventAggregator.GetEvent<ChekinUpdateEvent>().Unsubscribe(ChekinUpdate);
        }

        private void ChekinUpdate(Checkin checkin)
        {
            if (checkin != null)
            {
                untappdService.DownloadMediaFiles(webDownloader, checkin);
                Checkin = Mapper.GetCheckinViewModel(untappdService, checkin);
            }
            else
            {
                Checkin = null;
            }
        }
    }
}