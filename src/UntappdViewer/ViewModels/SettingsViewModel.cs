using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Infrastructure;
using UntappdViewer.Interfaces;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Modules;
using UntappdViewer.Utils;
using UntappdViewer.Views;

namespace UntappdViewer.ViewModels
{
    public class SettingsViewModel: LoadingBaseModel
    {
        private ISettingService settingService;

        private IWebApiClient webApiClient;

        private IModuleManager moduleManager;

        private IInteractionRequestService interactionRequestService;

        private string accessToken;

        private bool? isValidAccessToken;

        private bool? isValidAuthenticateUrl;

        private string authenticateUrlMessageText;

        private bool? isValidGenerateAccessToken;

        private string generateAccessTokenMessageText;

        private bool isShowAccessToken;

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

        public bool? IsValidAuthenticateUrl
        {
            get { return isValidAuthenticateUrl; }
            set { SetProperty(ref isValidAuthenticateUrl, value); }
        }
        public string AuthenticateUrlMessageText
        {
            get { return authenticateUrlMessageText; }
            set { SetProperty(ref authenticateUrlMessageText, value); }
        }

        public bool? IsValidGenerateAccessToken
        {
            get { return isValidGenerateAccessToken; }
            set { SetProperty(ref isValidGenerateAccessToken, value); }
        }
        public string GenerateAccessTokenMessageText
        {
            get { return generateAccessTokenMessageText; }
            set { SetProperty(ref generateAccessTokenMessageText, value); }
        }

        public bool IsShowAccessToken
        {
            get { return isShowAccessToken; }
            set { SetProperty(ref isShowAccessToken, value); }
        }

        public ICommand CheckAccessTokenCommand { get; }

        public ICommand GetCodeCommand { get; }

        public ICommand GetAccessTokenCommand { get; }

        public ICommand CancelButtonCommand { get; }

        public ICommand OkButtonCommand { get; }

        public SettingsViewModel(ISettingService settingService, IRegionManager regionManager,
                                                                 IWebApiClient webApiClient,
                                                                 IEventAggregator eventAggregator,
                                                                 IModuleManager moduleManager,
                                                                 IInteractionRequestService interactionRequestService) : base(moduleManager, regionManager, eventAggregator)
        {
            this.settingService = settingService;
            this.webApiClient = webApiClient;
            this.moduleManager = moduleManager;
            this.interactionRequestService = interactionRequestService;

            CheckAccessTokenCommand = new DelegateCommand<string>(CheckAccessToken);
            GetCodeCommand = new DelegateCommand<string[]>(GetCode);
            GetAccessTokenCommand = new DelegateCommand<string[]>(GetAccessToken);
            CancelButtonCommand = new DelegateCommand(Exit);
            OkButtonCommand = new DelegateCommand(Ok);

            isShowAccessToken = true;
        }

        protected override void Activate()
        {
            base.Activate();

            FillAccessToken();
        }

        protected override void DeActivate()
        {
            base.DeActivate();

            isShowAccessToken = true;
            ResetGetCodeControl();
        }

        private void FillAccessToken()
        {
            string accessToken = settingService.GetAccessToken();
            if (String.IsNullOrEmpty(accessToken))
                return;

            IsShowAccessToken = false;
            AccessToken = accessToken;
        }

        private void CheckAccessToken(string token)
        {
            LoadingChangeActivity(true);
            CheckAccessTokenAsync(token);
        }

        private void GetCode(string[] initialDataByClientID)
        {
            ResetGetCodeControl();

            IResponseMessage codeResponseMessage = webApiClient.CheckAuthenticateUrl(initialDataByClientID[0], initialDataByClientID[1]);
            if (!codeResponseMessage.IsValid)
            {
                AuthenticateUrlMessageText = codeResponseMessage.Message;
                IsValidAuthenticateUrl = false;
                return;
            }

            IsValidAuthenticateUrl = true;
            AuthenticateUrlMessageText = Properties.Resources.CopyTheCodeFromWebBrowser;
            ProcessStartHelper.ProcessStart(codeResponseMessage.Message);
        }
        
        private void ResetGetCodeControl()
        {
            IsValidAuthenticateUrl = null;
            ResetGenerateAccessTokenControl();
        }

        private void GetAccessToken(string[] initialDataByGetAccessToken)
        {
            ResetGenerateAccessTokenControl();
            IResponseMessage accessTokenResponseMessage = webApiClient.GetAccessToken(initialDataByGetAccessToken[0], initialDataByGetAccessToken[1], 
                                                                                        initialDataByGetAccessToken[2], initialDataByGetAccessToken[3]);
            if (!accessTokenResponseMessage.IsValid)
            {
                GenerateAccessTokenMessageText = accessTokenResponseMessage.Message;
                IsValidGenerateAccessToken = false;
                return;
            }

            IsValidGenerateAccessToken = true;
            GenerateAccessTokenMessageText = Properties.Resources.AccessTokenIsFill;
            AccessToken = accessTokenResponseMessage.Message;
            CheckAccessTokenAsync(AccessToken);
            IsShowAccessToken = true;
        }

        private void ResetGenerateAccessTokenControl()
        {
            IsValidGenerateAccessToken = null;
        }

        private async void CheckAccessTokenAsync(string token)
        {
            try
            {
                IsValidAccessToken = await Task.Run(() => webApiClient.LogOn(token));
                webApiClient.LogOff();
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

        private void Ok()
        {
            if (IsValidAccessToken.HasValue && IsValidAccessToken.Value || String.IsNullOrEmpty(AccessToken))
                settingService.SetAccessToken(AccessToken);

            Exit();
        }

        private void Exit()
        {
            moduleManager.LoadModule(typeof(UntappdModule).Name);
            ActivateView(RegionNames.MainRegion, typeof(Untappd));
        }
    }
}