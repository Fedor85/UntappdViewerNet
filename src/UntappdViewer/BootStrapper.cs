using System.Windows;
using Prism.Unity;
using Unity;

namespace UntappdViewer
{
    public class BootStrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return this.Container.Resolve<Shell>();
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
    }
}