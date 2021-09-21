using System;
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
using UntappdViewer.Services;
using UntappdViewer.Views;

namespace UntappdViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            ISettingService settingService = new SettingService();
            //if (Debugger.IsAttached)
            //    settingService.Reset();

            containerRegistry.RegisterInstance(settingService);
            containerRegistry.RegisterInstance(new UntappdService(settingService));

            IDialogService dialogService = containerRegistry.GetContainer().Resolve<IDialogService>();
            containerRegistry.RegisterInstance<IInteractionRequestService>(new InteractionRequestService(dialogService));
            containerRegistry.RegisterDialog<NotificationDialog>();
            containerRegistry.RegisterDialog<AskDialog>();
            containerRegistry.RegisterDialog<TextBoxDialog>();

            containerRegistry.Register<IWebDownloader, WebDownloader>();
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
        }
    }
}