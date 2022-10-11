using System;
using System.Diagnostics;
using System.Threading;
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

        private int runPause = 3000;

        public Loading()
        {
            InitializeComponent();
            Loaded += LoadingLoaded;
            Unloaded += LoadingUnloaded;
        }

        private void LoadingLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            isRunStatusBar = true;
            RunStatusBarAsunc();
            RunStatusTimeBarAsunc();
        }

        private void LoadingUnloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            isRunStatusBar = false;
        }

        private async void RunStatusBarAsunc()
        {
            await Task.Run(() => RunStatusBar());
        }

        private void RunStatusBar()
        {
            Thread.Sleep(runPause);
            while (isRunStatusBar)
            {
                string message = String.Empty;
                for (int i = 1; i <= 14; i++)
                {
                    if (!isRunStatusBar)
                        break;

                    Thread.Sleep(150);
                    SetStatusBar(String.Empty);
                    SetStatusBar(message += ".");
                }
                SetStatusBar(String.Empty);
            }
        }

        private async void RunStatusTimeBarAsunc()
        {
            await Task.Run(() => RunStatusTimeBar());
        }

        private void RunStatusTimeBar()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            Thread.Sleep(runPause);
 
            while (isRunStatusBar)
            {
                TimeSpan timeSpan = stopWatch.Elapsed;
                Thread.Sleep(1000);
                SetStatusTimeBar($"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}");
            }
            stopWatch.Stop();
            SetStatusTimeBar(String.Empty);
        }

        private void SetStatusBar(object content)
        {
            Dispatcher.Invoke(() => StatusBar.Content = content);
        }

        private void SetStatusTimeBar(object content)
        {
            Dispatcher.Invoke(() => StatusTimeBar.Content = content);
        }
    }
}