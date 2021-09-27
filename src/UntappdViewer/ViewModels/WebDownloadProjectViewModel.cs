using Prism.Regions;
using UntappdViewer.Interfaces.Services;

namespace UntappdViewer.ViewModels
{
    public class WebDownloadProjectViewModel : RegionManagerBaseModel
    {
        private IUntappdService untappdService;

        private IWebApiClient webApiClient;

        public WebDownloadProjectViewModel(IRegionManager regionManager, IUntappdService untappdService, IWebApiClient webApiClient) : base(regionManager)
        {
            this.untappdService = untappdService;
            this.webApiClient = webApiClient;
        }
    }
}