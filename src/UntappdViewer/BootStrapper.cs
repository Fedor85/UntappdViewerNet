using System.Diagnostics;
using System.Windows;
using Prism.Modularity;
using Prism.Unity;
using Unity;
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
            moduleCatalog.AddModule(typeof(ShellModule));
            moduleCatalog.AddModule(typeof(MainModule), InitializationMode.OnDemand);
        }

        private void RegisterServices()
        {
            Container.RegisterType<IDialogService, DialogService>();

            ISettingService settingService = new SettingService();
            //if (Debugger.IsAttached)
            //    settingService.Reset();

            Container.RegisterInstance(settingService);
        }
    }
}