﻿using System.ComponentModel;
using System.Windows.Input;
using Prism.Commands;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Modules;
using UntappdViewer.Views;

namespace UntappdViewer.ViewModels
{
    public class WebDownloadProjectViewModel : RegionManagerBaseModel
    {
        private bool? accessToken;

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

        public WebDownloadProjectViewModel(IRegionManager regionManager, IUntappdService untappdService,
                                                                         IWebApiClient webApiClient,
                                                                         IModuleManager moduleManager) : base(regionManager)
        {
            this.untappdService = untappdService;
            this.webApiClient = webApiClient;
            this.moduleManager = moduleManager;

            CheckAccessTokenCommand = new DelegateCommand<string>(CheckAccessToken);
            OkButtonCommand = new DelegateCommand(Exit);
        }

        private void CheckAccessToken(string token)
        {
            //необходимо занулять чтобы срабатывало событие если токен иметт одинковое значение подряд
            AccessToken = null;
            webApiClient.Initialize(token);
            AccessToken = webApiClient.Check();
        }

        private void Exit()
        {
            moduleManager.LoadModule(typeof(MainModule).Name);
            ActivateView(RegionNames.RootRegion, typeof(Main));
        }
    }
}