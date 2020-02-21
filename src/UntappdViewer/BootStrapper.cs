using System.Diagnostics;
using System.Windows;
using Prism.Events;
using Prism.Modularity;
using Prism.Unity;
using Unity;
using UntappdViewer.Domain.Services;
using UntappdViewer.Infrastructure.Services;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Modules;
using UntappdViewer.Views;

namespace UntappdViewer
{
    public class BootStrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();
            Window window = Shell as Window;
            if (window != null && Application.Current != null)
            {
                Application.Current.MainWindow = window;
                Application.Current.MainWindow.Show();
            }
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            RegisterServices();
        }

        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();
            RegisterModules(ModuleCatalog);
        }

        private void RegisterModules(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule(typeof(WelcomeModule), InitializationMode.OnDemand);
            moduleCatalog.AddModule(typeof(MainModule), InitializationMode.OnDemand);

            moduleCatalog.AddModule(typeof(MenuBarModule), InitializationMode.OnDemand);
            moduleCatalog.AddModule(typeof(UntappdModule), InitializationMode.OnDemand);
            moduleCatalog.AddModule(typeof(StatusBarModule), InitializationMode.OnDemand);

            moduleCatalog.AddModule(typeof(TreeModue), InitializationMode.OnDemand);
            moduleCatalog.AddModule(typeof(RecentFilesModule), InitializationMode.OnDemand);

            moduleCatalog.AddModule(typeof(CheckinModule), InitializationMode.OnDemand);
        }

        private void RegisterServices()
        {
            Container.RegisterInstance<ICommunicationService>(new CommunicationService());

            ISettingService settingService = new SettingService();
            //if (Debugger.IsAttached)
            //    settingService.Reset();

            Container.RegisterInstance(settingService);
            Container.RegisterInstance(new UntappdService(settingService));
        }
    }
}