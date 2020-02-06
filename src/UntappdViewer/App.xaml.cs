using System.Diagnostics;
using System.Windows;
using UntappdViewer.Properties;

namespace UntappdViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //if (Debugger.IsAttached)
            //    Settings.Default.Reset();

            base.OnStartup(e);
            BootStrapper bootstrapper = new BootStrapper();
            bootstrapper.Run();
        }
    }
}