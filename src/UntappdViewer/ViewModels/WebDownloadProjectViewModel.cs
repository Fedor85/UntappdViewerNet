using System.ComponentModel;
using System.Windows.Input;
using Prism.Commands;
using Prism.Regions;
using UntappdViewer.Interfaces.Services;

namespace UntappdViewer.ViewModels
{
    public class WebDownloadProjectViewModel : RegionManagerBaseModel
    {
        private bool? accessToken;

        private IUntappdService untappdService;

        private IWebApiClient webApiClient;

        public ICommand CheckAccessTokenCommand { get; }

        public bool? AccessToken
        {
            get
            {
                return accessToken;
            }
            set
            {
                accessToken = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AccessToken"));
            }
        }

        public WebDownloadProjectViewModel(IRegionManager regionManager, IUntappdService untappdService, IWebApiClient webApiClient) : base(regionManager)
        {
            this.untappdService = untappdService;
            this.webApiClient = webApiClient;

            CheckAccessTokenCommand = new DelegateCommand<string>(CheckAccessToken);
        }

        private void CheckAccessToken(string token)
        {
            AccessToken = null;
            //AccessToken = true;
            webApiClient.Initialize(token);
            AccessToken = webApiClient.Check();
        }
    }
}