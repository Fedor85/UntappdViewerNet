using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Models;
using UntappdViewer.Modules;
using UntappdViewer.Utils;
using Checkin = UntappdViewer.Models.Checkin;
using Untappd = UntappdViewer.Views.Untappd;

namespace UntappdViewer.ViewModels
{
    public class WebDownloadProjectViewModel : LoadingBaseModel
    {
        private bool? accessToken;

        private List<Checkin> checkins;

        private IUntappdService untappdService;

        private IWebApiClient webApiClient;

        private IModuleManager moduleManager;

        private IInteractionRequestService interactionRequestService;

        public ICommand CheckAccessTokenCommand { get; }

        public ICommand FulllDownloadButtonCommand { get; }

        public ICommand FirstDownloadButtonCommand { get; }

        public ICommand ToEndDownloadButtonCommand { get; }
        
        public ICommand BeerUpdateButtonCommand { get; }

        public ICommand OkButtonCommand { get; }

        public bool? AccessToken
        {
            get
            {
                return accessToken;
            }
            set
            {
                SetProperty(ref accessToken, value);
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
                SetProperty(ref checkins, value);
            }
        }

        public WebDownloadProjectViewModel(IRegionManager regionManager, IUntappdService untappdService,
                                                                         IWebApiClient webApiClient,
                                                                         IModuleManager moduleManager,
                                                                         IInteractionRequestService interactionRequestService) : base(moduleManager, regionManager)
        {
            this.untappdService = untappdService;
            this.webApiClient = webApiClient;
            this.moduleManager = moduleManager;
            this.interactionRequestService = interactionRequestService;

            CheckAccessTokenCommand = new DelegateCommand<string>(CheckAccessToken);
            FulllDownloadButtonCommand = new DelegateCommand(FulllDownload);
            FirstDownloadButtonCommand = new DelegateCommand(FirstDownload);
            ToEndDownloadButtonCommand = new DelegateCommand(ToEndDownload);
            BeerUpdateButtonCommand = new DelegateCommand(BeerUpdate);
            OkButtonCommand = new DelegateCommand(Exit);
        }

        protected override void Activate()
        {
            base.Activate();
            Checkins = new List<Checkin>(untappdService.GetCheckins());
            webApiClient.ChangeUploadedCountEvent += WebApiClientChangeUploadedCountEvent;
            interactionRequestService.ClearMessageOnStatusBar();
        }

        protected override void DeActivate()
        {
            base.DeActivate();
            Checkins.Clear();
            AccessToken = null;
            webApiClient.ChangeUploadedCountEvent -= WebApiClientChangeUploadedCountEvent;
            interactionRequestService.ClearMessageOnStatusBar();
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
            try
            {
                AccessToken = await Task.Run(() => webApiClient.Check());
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

        private void FulllDownload()
        {
            if (untappdService.GetCheckins().Count  > 0 && !interactionRequestService.Ask(Properties.Resources.Warning, Properties.Resources.AskDeletedCheckins))
                return;

            LoadingChangeActivity(true);

            Checkins.Clear();
            untappdService.GetCheckins().Clear();

            FillCheckins(webApiClient.FillFullCheckins);
        }

        private void FirstDownload()
        {
            LoadingChangeActivity(true);
            FillCheckins(webApiClient.FillFirstCheckins);
        }

        private void ToEndDownload()
        {
            LoadingChangeActivity(true);
            FillCheckins(webApiClient.FillToEndCheckins);
        }

        private void BeerUpdate()
        {
            List<Beer> beers = untappdService.GetBeers().Where(IsUpdateBeer).ToList();
            if (beers.Count == 0)
                return;

            webApiClient.BeerUpdate(beers, IsUpdateBeer);
        }

        private bool IsUpdateBeer(Beer beer)
        {
            return beer.GlobalRatingScore == 0;
        }

        private async void FillCheckins(Action<CheckinsContainer> fillCheckinsDelegate)
        {
            try
            {
                await Task.Run(() => fillCheckinsDelegate(untappdService.Untappd.CheckinsContainer));
            }
            catch (Exception ex)
            {
                interactionRequestService.ShowError(Properties.Resources.Error, StringHelper.GetFullExceptionMessage(ex));
            }
            finally
            {
                untappdService.SortDataDescCheckins();
                Checkins = new List<Checkin>(untappdService.GetCheckins());
                LoadingChangeActivity(false);
            }
        }

        private void WebApiClientChangeUploadedCountEvent(int count)
        {
            interactionRequestService.ShowMessageOnStatusBar($"{Properties.Resources.Uploaded}: {count}");
        }

        private void Exit()
        {
            moduleManager.LoadModule(typeof(UntappdModule).Name);
            ActivateView(RegionNames.MainRegion, typeof(Untappd));
        }
    }
}