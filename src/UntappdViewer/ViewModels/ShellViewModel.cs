using System.ComponentModel;
using System.Windows;
using Prism.Mvvm;
using UntappdViewer.Interfaces;
using UntappdViewer.Interfaces.Services;

namespace UntappdViewer.ViewModels
{
    public class ShellViewModel: BindableBase, IClosable
    {
        private IDialogService dialogService;

        public string Title { get; set; } = "UntappdViewer";

        public ShellViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;
        }

        public void Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = dialogService.Ask("Внимание!", "Вы желаете закрыть приложение?") != MessageBoxResult.OK;
        }
    }
}
