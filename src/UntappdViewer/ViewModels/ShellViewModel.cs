using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Prism.Commands;
using Prism.Regions;
using UntappdViewer.Interfaces.Services;

namespace UntappdViewer.ViewModels
{
    public class ShellViewModel
    {
        private IDialogService dialogService;

        private IRegionManager regionManager;

        public ICommand ClosingCommand { get; }

        public string Title  => $"{Properties.Resources.AppName} ({Assembly.GetEntryAssembly()?.GetName().Version})";

        public ShellViewModel(IDialogService dialogService, IRegionManager regionManager)
        {
            this.dialogService = dialogService;
            this.regionManager = regionManager;
            ClosingCommand = new DelegateCommand<CancelEventArgs>(Closing);
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
    }
}