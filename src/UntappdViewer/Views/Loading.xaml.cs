using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace UntappdViewer.Views
{
    /// <summary>
    /// Interaction logic for Loading.xaml
    /// </summary>
    public partial class Loading : UserControl
    {
        private bool isRunStatusBar;

        public Loading()
        {
            InitializeComponent();
            Loaded += LoadingLoaded;
            Unloaded += LoadingUnloaded;
        }

        private void LoadingLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            isRunStatusBar = true;
            RunStatusBar();
            RunStatusTimeBar();
        }

        private void LoadingUnloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            isRunStatusBar = false;
        }

        private async void RunStatusBar()
        {
            while (isRunStatusBar)
            {
                for (int i = 1; i <= 14; i++)
                {
                    if (!isRunStatusBar)
                        break;

                    await Task.Delay(200);
                    StatusBar.Content += ".";
                }
                StatusBar.Content = String.Empty;
            }
        }

        private async void RunStatusTimeBar()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            while (isRunStatusBar)
            {
                await Task.Delay(1);
                TimeSpan timeSpan = stopWatch.Elapsed;
                StatusTimeBar.Content = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
            }
            stopWatch.Stop();
            StatusTimeBar.Content = String.Empty;
        }
    }
}