using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Prism.Commands;
using UntappdViewer.Interfaces.Services;

namespace UntappdViewer.ViewModels
{
    public class ShellViewModel
    {
        private IDialogService dialogService;

        public ICommand ClosingCommand { get; }

        public string Title  => $"{Properties.Resources.AppName} ({Assembly.GetEntryAssembly()?.GetName().Version})";

        public ShellViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;
            ClosingCommand = new DelegateCommand<CancelEventArgs>(Closing);
        }

        private void Closing(CancelEventArgs e)
        {
            e.Cancel = dialogService.Ask(Properties.Resources.Warning, Properties.Resources.AskCloseApp) != MessageBoxResult.OK;
        }
    }
}