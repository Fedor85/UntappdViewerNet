﻿//using System.Diagnostics;

using System.Diagnostics;
using System.Linq;
using System.Windows;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Services.Dialogs;
using Prism.Unity;
using Unity;
using UntappdViewer.Domain.Services;
using UntappdViewer.Infrastructure.Services;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Modules;
using UntappdViewer.Reporting;
using UntappdViewer.Services;
using UntappdViewer.Views;
using UntappdWebApiClient;

namespace UntappdViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        private IArgumentsProvider argumentsProvider = new ArgumentsProvider();

        protected override Window CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            if (IsInstanceExists())
            {
                MessageBox.Show(UntappdViewer.Properties.Resources.InstanceExistsMessage, UntappdViewer.Properties.Resources.Warning);
                Current.Shutdown();
                return;
            }

            argumentsProvider.Arguments.AddRange(e.Args.ToList());
            base.OnStartup(e);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance(argumentsProvider);
            ISettingService settingService = new SettingService();
            //if (Debugger.IsAttached)
            //    settingService.Reset();

            containerRegistry.RegisterInstance(settingService);
            containerRegistry.RegisterInstance<IUntappdService>(new UntappdService(settingService));

            IDialogService dialogService = containerRegistry.GetContainer().Resolve<IDialogService>();
            containerRegistry.RegisterInstance<IInteractionRequestService>(new InteractionRequestService(dialogService));
            containerRegistry.RegisterDialog<NotificationDialog>();
            containerRegistry.RegisterDialog<AskDialog>();
            containerRegistry.RegisterDialog<TextBoxDialog>();

            containerRegistry.Register<IWebDownloader, WebDownloader>();
            containerRegistry.Register<IWebApiClient, Client>();

            containerRegistry.Register<IReportingService, ReportingService>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule(typeof(WelcomeModule), InitializationMode.OnDemand);
            moduleCatalog.AddModule(typeof(MainModule), InitializationMode.OnDemand);
            moduleCatalog.AddModule(typeof(LoadingModule), InitializationMode.OnDemand);

            moduleCatalog.AddModule(typeof(MenuBarModule), InitializationMode.OnDemand);
            moduleCatalog.AddModule(typeof(UntappdModule), InitializationMode.OnDemand);
            moduleCatalog.AddModule(typeof(StatusBarModule), InitializationMode.OnDemand);

            moduleCatalog.AddModule(typeof(TreeModue), InitializationMode.OnDemand);
            moduleCatalog.AddModule(typeof(RecentFilesModule), InitializationMode.OnDemand);

            moduleCatalog.AddModule(typeof(CheckinModule), InitializationMode.OnDemand);
            moduleCatalog.AddModule(typeof(PhotoLoadingModule), InitializationMode.OnDemand);
            moduleCatalog.AddModule(typeof(WebDownloadProjectModule), InitializationMode.OnDemand);
        }

        private bool IsInstanceExists()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);
            return processes.Length > 1;
        }
    }
}