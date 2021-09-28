using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Modules;
using UntappdViewer.Views;
using Checkin = UntappdViewer.Models.Checkin;

namespace UntappdViewer.ViewModels
{
    public class WebDownloadProjectViewModel : LoadingBaseModel
    {
        private bool? accessToken;

        private List<Checkin> checkins;

        private IUntappdService untappdService;

        private IWebApiClient webApiClient;

        private IModuleManager moduleManager;

        public ICommand CheckAccessTokenCommand { get; }

        public ICommand OkButtonCommand { get; }

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

        public List<Checkin> Checkins
        {
            get
            {
                return checkins;
            }
            set
            {
                checkins = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Checkins"));
            }
        }

        public WebDownloadProjectViewModel(IRegionManager regionManager, IUntappdService untappdService,
                                                                         IWebApiClient webApiClient,
                                                                         IModuleManager moduleManager) : base(moduleManager, regionManager)
        {
            this.untappdService = untappdService;
            this.webApiClient = webApiClient;
            this.moduleManager = moduleManager;
            CheckAccessTokenCommand = new DelegateCommand<string>(CheckAccessToken);
            OkButtonCommand = new DelegateCommand(Exit);
        }

        protected override void Activate()
        {
            base.Activate();
            Checkins = new List<Checkin>(untappdService.GeCheckins());
        }

        protected override void DeActivate()
        {
            base.DeActivate();
            Checkins.Clear();
            AccessToken = null;
        }

        private void CheckAccessToken(string token)
        {
            LoadingChangeActivity(true);
            CheckAccessTokenAsync(token);
        }

        private async void CheckAccessTokenAsync(string token)
        {
            AccessToken = null;
            webApiClient.Initialize(token);
            AccessToken = await Task.Run(() => webApiClient.Check());
            LoadingChangeActivity(false);
        }

        private void Exit()
        {
            moduleManager.LoadModule(typeof(MainModule).Name);
            ActivateView(RegionNames.RootRegion, typeof(Main));
        }
    }
}