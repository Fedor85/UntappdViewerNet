using System;
using System.Collections.Generic;
using System.Linq;
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

        private IInteractionRequestService interactionRequestService;

        public ICommand CheckAccessTokenCommand { get; }

        public ICommand FulllDownloadButtonCommand { get; }

        public ICommand FirstDownloadButtonCommand { get; }

        public ICommand ToEndDownloadButtonCommand { get; }

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
                interactionRequestService.ShowError(Properties.Resources.Error, ex.Message);
            }
            finally
            {
                LoadingChangeActivity(false);
            }
        }

        private void FulllDownload()
        {
            if (untappdService.Untappd.Checkins.Count  > 0 && !interactionRequestService.Ask(Properties.Resources.Warning, Properties.Resources.AskDeletedCheckins))
                return;

            LoadingChangeActivity(true);

            Checkins.Clear();
            untappdService.Untappd.Checkins.Clear();

            FillCheckins(FulllDownload);
        }

        private void FulllDownload(List<Checkin> checkins)
        {
            webApiClient.FillFullCheckins(checkins);
        }

        private void FirstDownload()
        {
            LoadingChangeActivity(true);
            FillCheckins(FirstDownload);
        }

        private void FirstDownload(List<Checkin> checkins)
        {
            webApiClient.FillFirstCheckins(checkins, untappdService.Untappd.Checkins.Max(item => item.Id));
        }

        private void ToEndDownload()
        {
            LoadingChangeActivity(true);
            FillCheckins(ToEndDownload);
        }

        private void ToEndDownload(List<Checkin> checkins)
        {
            webApiClient.FillToEndCheckins(checkins, untappdService.Untappd.Checkins.Min(item => item.Id));
        }

        private async void FillCheckins(Action<List<Checkin>> fillCheckinsDelegate)
        {
            List<Checkin> checkins = new List<Checkin>();
            try
            {
                await Task.Run(() => fillCheckinsDelegate(checkins));
            }
            catch (Exception ex)
            {
                interactionRequestService.ShowError(Properties.Resources.Error, ex.Message);
            }
            finally
            {
                if (checkins.Count > 0)
                {
                    untappdService.AddCheckins(checkins);
                    Checkins = new List<Checkin>(untappdService.GetCheckins());
                }
                LoadingChangeActivity(false);
            }
        }

        private void WebApiClientChangeUploadedCountEvent(int count)
        {
            interactionRequestService.ShowMessageOnStatusBar($"{Properties.Resources.Uploaded}:{count}");
        }

        private void Exit()
        {
            moduleManager.LoadModule(typeof(UntappdModule).Name);
            ActivateView(RegionNames.MainRegion, typeof(Untappd));
        }
    }
}