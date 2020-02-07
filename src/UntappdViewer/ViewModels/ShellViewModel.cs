using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Prism.Commands;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Modules;
using UntappdViewer.Services;
using Untappd = UntappdViewer.Models.Untappd;

namespace UntappdViewer.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        private IDialogService dialogService;

        private IRegionManager regionManager;

        private ISettingService settingService;

        private IModuleManager moduleManager;

        private string title;

        public ICommand ClosingCommand { get; }

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged();
            }
        }

        public ShellViewModel(UntappdService untappdService, IDialogService dialogService, IRegionManager regionManager, ISettingService settingService, IModuleManager moduleManager)
        {
            this.dialogService = dialogService;
            this.regionManager = regionManager;
            this.settingService = settingService;
            this.moduleManager = moduleManager;
            ClosingCommand = new DelegateCommand<CancelEventArgs>(Closing);
            untappdService.InitializeUntappd += UntappdServiceInitializeUntappd;
            Title = GetTitle(String.Empty);
            Activate();
        }

        private void Activate()
        {
            if (String.IsNullOrEmpty(settingService.GetLastOpenedFilePath()))
                moduleManager.LoadModule(typeof(WelcomeModule).Name);
            else
                moduleManager.LoadModule(typeof(MainModule).Name);
        }

        private void UntappdServiceInitializeUntappd(Untappd untappd)
        {
            Title = GetTitle(untappd.UserName);
        }

        private void Closing(CancelEventArgs e)
        {
            if (dialogService.Ask(Properties.Resources.Warning, Properties.Resources.AskCloseApp) == MessageBoxResult.OK)
                DeactivateViews();
            else
                e.Cancel = true;
        }

        private void DeactivateViews()
        {
            foreach (IRegion region in regionManager.Regions)
            {
                foreach (object view in region.Views)
                    region.Deactivate(view);
            }
        }

        private string GetTitle(string userName)
        {
            return  $"{Properties.Resources.AppName} {(String.IsNullOrEmpty(userName) ? String.Empty : userName)} ({Assembly.GetEntryAssembly()?.GetName().Version})";
        }
    }
}