using System.Windows.Input;
using Prism.Commands;
using Prism.Regions;
using UntappdViewer.Interfaces.Services;

namespace UntappdViewer.ViewModels
{
    public class WebDownloadProjectViewModel : RegionManagerBaseModel
    {
        private IUntappdService untappdService;

        private IWebApiClient webApiClient;

        public ICommand CheckAccessTokenCommand { get; }

        public WebDownloadProjectViewModel(IRegionManager regionManager, IUntappdService untappdService, IWebApiClient webApiClient) : base(regionManager)
        {
            this.untappdService = untappdService;
            this.webApiClient = webApiClient;

            CheckAccessTokenCommand = new DelegateCommand(CheckAccessToken);
        }

        private void CheckAccessToken()
        {
        }
    }
}