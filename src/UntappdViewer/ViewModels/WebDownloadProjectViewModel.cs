using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Events;
using UntappdViewer.Helpers;
using UntappdViewer.Interfaces;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Models;
using UntappdViewer.Models.Different;
using UntappdViewer.Modules;
using UntappdViewer.Utils;
using Checkin = UntappdViewer.Models.Checkin;
using Untappd = UntappdViewer.Views.Untappd;

namespace UntappdViewer.ViewModels
{
    public class WebDownloadProjectViewModel : LoadingBaseModel
    {
        private IUntappdService untappdService;

        private IWebApiClient webApiClient;

        private IWebDownloader webDownloader;

        private IModuleManager moduleManager;

        private ISettingService settingService;

        private IInteractionRequestService interactionRequestService;

        private bool isVisibilityLogInControl;

        private string accessToken;

        private bool? isValidAccessToken;

        private bool isShowPassword;

        private bool isVisibilityDownloadControl;

        private bool isEnabledAPIUntappd;

        private string tableCaption;

        private bool isCheckedSaveAccessToken;

        private string offsetUpdateBeer;

        private bool isEnabledFillServingTypeButton;

        private bool isEnabledCollaborationButton;

        private List<Checkin> checkins;

        public bool IsVisibilityLogInControl
        {
            get { return isVisibilityLogInControl; }
            set { SetProperty(ref isVisibilityLogInControl, value); }
        }

        public string AccessToken
        {
            get { return accessToken; }
            set { SetProperty(ref accessToken, value); }
        }

        public bool? IsValidAccessToken
        {
            get { return isValidAccessToken; }
            set { SetProperty(ref isValidAccessToken, value); }
        }

        public bool IsShowPassword
        {
            get { return isShowPassword; }
            set { SetProperty(ref isShowPassword, value); }
        }

        public bool IsVisibilityDownloadControl
        {
            get { return isVisibilityDownloadControl; }
            set { SetProperty(ref isVisibilityDownloadControl, value); }
        }

        public bool IsEnabledAPIUntappd
        {
            get { return isEnabledAPIUntappd; }
            set { SetProperty(ref isEnabledAPIUntappd, value); }
        }

        public string TableCaption
        {
            get { return tableCaption; }
            set { SetProperty(ref tableCaption, value); }
        }

        public bool IsCheckedSaveAccessToken
        {
            get { return isCheckedSaveAccessToken; }
            set { SetProperty(ref isCheckedSaveAccessToken, value); }
        }

        public string OffsetUpdateBeer
        {
            get { return offsetUpdateBeer; }
            set { SetProperty(ref offsetUpdateBeer, value); }
        }

        public bool IsEnabledFillServingTypeButton
        {
            get { return isEnabledFillServingTypeButton; }
            set { SetProperty(ref isEnabledFillServingTypeButton, value); }
        }

        public bool IsEnabledCollaborationButton
        {
            get { return isEnabledCollaborationButton; }
            set { SetProperty(ref isEnabledCollaborationButton, value); }
        }

        public List<Checkin> Checkins
        {
            get { return checkins; }
            set { SetProperty(ref checkins, value); }
        }

        private ICancellationToken<Checkin> webClientCancellation;

        public ICommand WebApiClientLogInCommand { get; }

        public ICommand ContinueButtonCommand { get; }

        public ICommand FullDownloadButtonCommand { get; }

        public ICommand FirstDownloadButtonCommand { get; }

        public ICommand ToEndDownloadButtonCommand { get; }
        
        public ICommand BeerUpdateButtonCommand { get; }

        public ICommand FillServingTypeButtonCommand { get; }

        public ICommand FillCollaborationButtonCommand { get; }

        public ICommand BackButtonCommand { get; }

        public ICommand OkButtonCommand { get; }


        public WebDownloadProjectViewModel(IRegionManager regionManager, IUntappdService untappdService,
                                                                         IWebApiClient webApiClient,
                                                                         IWebDownloader webDownloader,
                                                                         IModuleManager moduleManager,
                                                                         IEventAggregator eventAggregator,
                                                                         ISettingService settingService,
                                                                         IInteractionRequestService interactionRequestService) : base(moduleManager, regionManager, eventAggregator)
        {
            this.untappdService = untappdService;
            this.webApiClient = webApiClient;
            this.webDownloader = webDownloader;
            this.moduleManager = moduleManager;
            this.settingService = settingService;
            this.interactionRequestService = interactionRequestService;

            IsVisibilityLogInControl = true;

            WebApiClientLogInCommand = new DelegateCommand<string>(WebApiClientLogIn);
            ContinueButtonCommand = new DelegateCommand(() => InitializeDownloadControl(webApiClient.IsLogOn));

            FullDownloadButtonCommand = new DelegateCommand(FullDownloadCheckins);
            FirstDownloadButtonCommand = new DelegateCommand(FirstDownloadCheckins);
            ToEndDownloadButtonCommand = new DelegateCommand(ToEndDownloadCheckins);
            BeerUpdateButtonCommand = new DelegateCommand(UpdateBeers);

            FillServingTypeButtonCommand = new DelegateCommand(FillServingType);
            FillCollaborationButtonCommand = new DelegateCommand(FillCollaboration);

            BackButtonCommand = new DelegateCommand(InitializeLogInControl);

            OkButtonCommand = new DelegateCommand(Exit);

            webClientCancellation = webApiClient.GetCancellationToken<Checkin>();

        }

        protected override void Activate()
        {
            base.Activate();

            IsVisibilityLogInControl = true;
            SetCheckins();

            webApiClient.UploadedProgress += interactionRequestService.ShowMessageOnLoading;
            interactionRequestService.ClearMessageOnStatusBar();
            
            LogIn();
        }

        protected override void DeActivate()
        {
            base.DeActivate();

            Checkins.Clear();
            TableCaption = String.Empty;

            IsVisibilityLogInControl = true;
            IsCheckedSaveAccessToken = false;
            AccessToken = null;

            IsVisibilityDownloadControl = false;
            OffsetUpdateBeer = String.Empty;

            webApiClient.UploadedProgress -= interactionRequestService.ShowMessageOnStatusBar;
            interactionRequestService.ClearMessageOnStatusBar();
        }

        private void SetCheckins()
        {
            Checkins = new List<Checkin>(untappdService.GetCheckins());
            TableCaption = $"{Properties.Resources.Checkins} ({Checkins.Count}):";
        }

        private void LogIn()
        {
            if (webApiClient.IsLogOn)
            {
                InitializeDownloadControl(true);
                return;
            }

            string accessToken = settingService.GetAccessToken();
            if (String.IsNullOrEmpty(accessToken))
                return;

            AccessToken = accessToken;
            IsShowPassword = false;
            WebApiClientLogIn(accessToken);
        }

        private void WebApiClientLogIn(string token)
        {
            LoadingChangeActivity(true);
            WebApiClientLogInAsync(token);
        }

        private async void WebApiClientLogInAsync(string token)
        {
            try
            {
                IsValidAccessToken = await Task.Run(() => webApiClient.LogOn(token));
                if (IsValidAccessToken.HasValue && IsValidAccessToken.Value)
                {
                    if(IsCheckedSaveAccessToken)
                        settingService.SetAccessToken(token);

                    InitializeDownloadControl(true);
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

        private void FullDownloadCheckins()
        {
            if (untappdService.GetCheckins().Count  > 0 && !interactionRequestService.Ask(Properties.Resources.Warning, Properties.Resources.AskDeletedCheckins))
                return;

            Checkins.Clear();
            untappdService.GetCheckins().Clear();

            LoadingChangeActivity(true, true);
            FillCheckins(webApiClient.FillFullCheckins);
        }

        private void FirstDownloadCheckins()
        {
            LoadingChangeActivity(true, true);
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
                FillFullFirstCheckinsAsync();
            else
                FillCheckins(webApiClient.FillFirstCheckins);
        }

        private async void FillFullFirstCheckinsAsync()
        {
            BeforeRunWebClient();
            try
            {
                await Task.Run(() => FillFullFirstCheckins());
            }
            catch (Exception ex)
            {
                interactionRequestService.ShowError(Properties.Resources.Error, StringHelper.GetFullExceptionMessage(ex));
            }
            finally
            {
                AfterRunWebClient();
            }
        }
        private void ToEndDownloadCheckins()
        {
            LoadingChangeActivity(true, true);
            FillCheckins(webApiClient.FillToEndCheckins);
        }

        private void FillFullFirstCheckins()
        {
            webApiClient.FillFirstCheckins(untappdService.Untappd.CheckinsContainer, webClientCancellation);
            List<Checkin> checkins = webClientCancellation.Items;
             if (checkins.Count == 0)
                return;

            string message = interactionRequestService.GetCurrentMessageOnLoading();
            webApiClient.FillServingType(checkins, DefaultValues.DefaultServingType, webClientCancellation);

            List<Beer> beers = untappdService.Untappd.CheckinsContainer.GetBeers(checkins);
            long offset = 0;
            webApiClient.UpdateBeers(beers, null, ref offset, webClientCancellation);

            webApiClient.FillCollaboration(beers, untappdService.GetFullBreweries(), webClientCancellation);

            DownloadMediaFiles(checkins);
            interactionRequestService.ShowMessageOnLoading(message);
        }

        private void DownloadMediaFiles(List<Checkin> checkins)
        {
            int count = checkins.Count;
            int counter = 1;
            foreach (Checkin checkin in checkins)
            {
                interactionRequestService.ShowMessageOnLoading(CommunicationHelper.GetLoadingMessage(counter++, count, checkin.Beer.Name));
                untappdService.DownloadMediaFiles(webDownloader, checkin);
            }
        }

        private void UpdateBeers()
        {
            List<Beer> beers = untappdService.GetBeers();
            if (!beers.Any())
                return;

            LoadingChangeActivity(true, true);
            UpdateBeers(beers);
        }

        private void FillServingType()
        {
            List<Checkin> checkins = untappdService.GetCheckins().Where(item => String.IsNullOrEmpty(item.ServingType)).ToList();
            if (!checkins.Any())
                return;

            LoadingChangeActivity(true, true);
            FillServingType(checkins);
        }

        private void FillCollaboration()
        {
            List<Beer> beers = untappdService.GetBeers().Where(item => item.Collaboration.State == CollaborationState.Undefined).ToList();
            if (!beers.Any())
                return;

            LoadingChangeActivity(true, true);
            FillCollaboration(beers);
        }

        private async void FillCheckins(Action<CheckinsContainer, ICancellationToken<Checkin>> fillCheckinsDelegate)
        {
            BeforeRunWebClient();
            try
            {
                await Task.Run(() => fillCheckinsDelegate(untappdService.Untappd.CheckinsContainer, webClientCancellation));
            }
            catch (Exception ex)
            {
                interactionRequestService.ShowError(Properties.Resources.Error, StringHelper.GetFullExceptionMessage(ex));
            }
            finally
            {
                AfterRunWebClient();
            }
        }

        private async void UpdateBeers(List<Beer> beers)
        {
            BeforeRunWebClient();
            long offset = GetOffsetUpdateBeer();
            try
            {
                await Task.Run(() => webApiClient.UpdateBeers(beers, null, ref offset, webClientCancellation));
            }
            catch (Exception ex)
            {
                interactionRequestService.ShowError(Properties.Resources.Error, StringHelper.GetFullExceptionMessage(ex));
            }
            finally
            {
                settingService.SetOffsetUpdateBeer(offset);
                SetOffsetUpdateBeer();
                AfterRunWebClient();
            }
        }

        private async void FillServingType(List<Checkin> checkins)
        {
            BeforeRunWebClient();
            try
            {
                await Task.Run(() => webApiClient.FillServingType(checkins, DefaultValues.DefaultServingType, webClientCancellation));
            }
            catch (Exception ex)
            {
                interactionRequestService.ShowError(Properties.Resources.Error, StringHelper.GetFullExceptionMessage(ex));
            }
            finally
            {
                AfterRunWebClient();
            }
        }

        private async void FillCollaboration(List<Beer> beers)
        {
            BeforeRunWebClient();
            try
            {
                await Task.Run(() => webApiClient.FillCollaboration(beers, untappdService.GetFullBreweries(), webClientCancellation));
            }
            catch (Exception ex)
            {
                interactionRequestService.ShowError(Properties.Resources.Error, StringHelper.GetFullExceptionMessage(ex));
            }
            finally
            {
                AfterRunWebClient();
            }
        }

        private void BeforeRunWebClient()
        {
            eventAggregator.GetEvent<LoadingCancel>().Subscribe(LoadingCanceled);
            interactionRequestService.ShowMessageOnStatusBar(String.Empty);
        }

        private void AfterRunWebClient()
        {
            webClientCancellation.Cancel = false;
            webClientCancellation.Items.Clear();
            eventAggregator.GetEvent<LoadingCancel>().Unsubscribe(LoadingCanceled);

            untappdService.SortDataDescCheckins();
            SetCheckins();
            SetEnabledFillServingTypeButton();
            SetEnabledCollaborationButton();

            interactionRequestService.ShowMessageOnStatusBar(interactionRequestService.GetCurrentMessageOnLoading());
            LoadingChangeActivity(false);
        }

        private void LoadingCanceled()
        {
            webClientCancellation.Cancel = true;
        }

        private void InitializeDownloadControl(bool isLogOn)
        {
            IsVisibilityLogInControl = false;

            if (isLogOn)
            {
                IsEnabledAPIUntappd = true;
                SetOffsetUpdateBeer();
            }
            else
            {
                IsEnabledAPIUntappd = false;
                OffsetUpdateBeer = String.Empty;
            }
               
            SetEnabledFillServingTypeButton();
            SetEnabledCollaborationButton();

            IsVisibilityDownloadControl = true;
        }

        private void InitializeLogInControl()
        {
            IsVisibilityDownloadControl = false;
            OffsetUpdateBeer = String.Empty;
            IsShowPassword = !webApiClient.IsLogOn;

            IsVisibilityLogInControl = true;
        }

        private long GetOffsetUpdateBeer()
        {
            return Int64.TryParse(OffsetUpdateBeer, out var offset) ? offset : 0;
        }

        private void SetOffsetUpdateBeer()
        {
            long offset = settingService.GetOffsetUpdateBeer();
            OffsetUpdateBeer = offset > 0 ? offset.ToString() : String.Empty;
        }

        private void SetEnabledFillServingTypeButton()
        {
            IsEnabledFillServingTypeButton = Checkins.Any(item => String.IsNullOrEmpty(item.ServingType));
        }

        private void SetEnabledCollaborationButton()
        {
            List<Beer> beers = untappdService.GetBeers();
            IsEnabledCollaborationButton = beers.Any(item => item.Collaboration.State == CollaborationState.Undefined);
        }

        private void Exit()
        {
            settingService.SetOffsetUpdateBeer(GetOffsetUpdateBeer());
            moduleManager.LoadModule(typeof(UntappdModule).Name);
            ActivateView(RegionNames.MainRegion, typeof(Untappd));
        }
    }
}