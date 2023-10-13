using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
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

        private void LoadingLoaded(object sender, RoutedEventArgs e)
        {
            isRunStatusBar = true;
            RunStatusBarAsunc();
        }

        private void LoadingUnloaded(object sender, RoutedEventArgs e)
        {
            isRunStatusBar = false;
        }

        private async void RunStatusBarAsunc()
        {
            await Task.Run(() => RunStatusBar());
        }

        private void RunStatusBar()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            Thread.Sleep(runPause);
            bool isVisibleImageAwesome = false;
            while (isRunStatusBar)
            {
                TimeSpan timeSpan = stopWatch.Elapsed;
                Thread.Sleep(1000);
                if (!isVisibleImageAwesome)
                {
                    SetImageAwesome(Visibility.Visible);
                    isVisibleImageAwesome = true;
                }
                SetStatusTimeBar($"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}");
            }
            stopWatch.Stop();
            SetStatusTimeBar(String.Empty);
            SetImageAwesome(Visibility.Hidden);
        }

        private void SetImageAwesome(Visibility visibility)
        {
            Dispatcher.Invoke(() => ImageAwesome.Visibility = visibility);
        }

        private void SetStatusTimeBar(object content)
        {
            Dispatcher.Invoke(() => StatusTimeBar.Content = content);
        }
    }
}