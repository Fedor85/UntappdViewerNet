using System.ComponentModel;
using System.Reflection;
using System.Windows;
using Prism.Mvvm;
using UntappdViewer.Interfaces;
using UntappdViewer.Interfaces.Services;

namespace UntappdViewer.ViewModels
{
    public class ShellViewModel: BindableBase, IClosable
    {
        private IDialogService dialogService;

        public string Title  => $"{Properties.Resources.AppName} ({Assembly.GetEntryAssembly()?.GetName().Version})";

        public ShellViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;
        }

        public void Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = dialogService.Ask(Properties.Resources.Warning, Properties.Resources.AskCloseApp) != MessageBoxResult.OK;
        }
    }
}