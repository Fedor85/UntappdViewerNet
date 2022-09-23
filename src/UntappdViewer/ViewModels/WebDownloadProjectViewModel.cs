using System;
using System.Collections.Generic;
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

        private string offsetUpdateBeer;

        private List<Checkin> checkins;

        private IUntappdService untappdService;

        private IWebApiClient webApiClient;

        private IModuleManager moduleManager;

        private ISettingService settingService;

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

        public string OffsetUpdateBeer
        {
            get
            {
                return offsetUpdateBeer;
            }
            set
            {
                SetProperty(ref offsetUpdateBeer, value);
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
                                                                         ISettingService settingService,
                                                                         IInteractionRequestService interactionRequestService) : base(moduleManager, regionManager)
        {
            this.untappdService = untappdService;
            this.webApiClient = webApiClient;
            this.moduleManager = moduleManager;
            this.settingService = settingService;
            this.interactionRequestService = interactionRequestService;

            CheckAccessTokenCommand = new DelegateCommand<string>(CheckAccessToken);
            FulllDownloadButtonCommand = new DelegateCommand(FulllDownloadCheckins);
            FirstDownloadButtonCommand = new DelegateCommand(FirstDownloadCheckins);
            ToEndDownloadButtonCommand = new DelegateCommand(ToEndDownloadCheckins);
            BeerUpdateButtonCommand = new DelegateCommand(UpdateBeers);
            OkButtonCommand = new DelegateCommand(Exit);
        }

        protected override void Activate()
        {
            base.Activate();
            Checkins = new List<Checkin>(untappdService.GetCheckins());
            webApiClient.UploadedProgress += interactionRequestService.ShowMessageOnStatusBar;
            interactionRequestService.ClearMessageOnStatusBar();
        }

        protected override void DeActivate()
        {
            base.DeActivate();
            Checkins.Clear();
            AccessToken = null;
            OffsetUpdateBeer = String.Empty;
            webApiClient.UploadedProgress -= interactionRequestService.ShowMessageOnStatusBar;
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
                if (AccessToken.HasValue && AccessToken.Value)
                {
                    long settingOffsetUpdateBeer = settingService.GetOffsetUpdateBeer();
                    if (settingOffsetUpdateBeer > 0)
                        OffsetUpdateBeer = settingOffsetUpdateBeer.ToString();
                }
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

        private void FulllDownloadCheckins()
        {
            if (untappdService.GetCheckins().Count  > 0 && !interactionRequestService.Ask(Properties.Resources.Warning, Properties.Resources.AskDeletedCheckins))
                return;

            LoadingChangeActivity(true);

            Checkins.Clear();
            untappdService.GetCheckins().Clear();

            FillCheckins(webApiClient.FillFullCheckins);
        }

        private void FirstDownloadCheckins()
        {
            LoadingChangeActivity(true);
            FillCheckins(webApiClient.FillFirstCheckins);
        }

        private void ToEndDownloadCheckins()
        {
            LoadingChangeActivity(true);
            FillCheckins(webApiClient.FillToEndCheckins);
        }

        private void UpdateBeers()
        {
            List<Beer> beers = untappdService.GetBeers();
            if (beers.Count == 0)
                return;

            LoadingChangeActivity(true);
            UpdateBeers(beers);
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

        private async void UpdateBeers(List<Beer> beers)
        {
            long offset = GetOffsetUpdateBeer();
            try
            {
                await Task.Run(() => webApiClient.UpdateBeers(beers, null, ref offset));
                SetOffsetUpdateBeer(0);
            }
            catch (Exception ex)
            {
                SetOffsetUpdateBeer(offset);
                interactionRequestService.ShowError(Properties.Resources.Error, StringHelper.GetFullExceptionMessage(ex));
            }
            finally
            {
                LoadingChangeActivity(false);
            }
        }

        private long GetOffsetUpdateBeer()
        {
            return Int64.TryParse(OffsetUpdateBeer, out var offset) ? offset : 0;
        }

        private void SetOffsetUpdateBeer(long offset)
        {
            settingService.SetOffsetUpdateBeer(offset);
            OffsetUpdateBeer = offset > 0 ? offset.ToString() : String.Empty;
        }

        private void Exit()
        {
            if (AccessToken.HasValue && AccessToken.Value)
            {
                long offset = GetOffsetUpdateBeer();
                if (offset != settingService.GetOffsetUpdateBeer())
                    settingService.SetOffsetUpdateBeer(offset);
            }

            moduleManager.LoadModule(typeof(UntappdModule).Name);
            ActivateView(RegionNames.MainRegion, typeof(Untappd));
        }
    }
}