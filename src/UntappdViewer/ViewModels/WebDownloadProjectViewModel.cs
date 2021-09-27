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

            CheckAccessTokenCommand = new DelegateCommand<string>(CheckAccessToken);
        }

        private void CheckAccessToken(string token)
        {
            webApiClient.Initialize(token);
            string message;
            bool check = webApiClient.Check(out message);
        }
    }
}