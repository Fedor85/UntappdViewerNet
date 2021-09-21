using System.Windows.Input;
using Prism.Commands;
using Prism.Services.Dialogs;
using UntappdViewer.ViewModels.Base;

namespace UntappdViewer.ViewModels
{
    public class AskDialogViewModel : BaseDialogModel
    {
        public ICommand CancelDialogCommand { get; }

        public AskDialogViewModel()
        {
            CancelDialogCommand = new DelegateCommand(CancelCloseDialog);
        }

        private void CancelCloseDialog()
        {
            CloseDialog(ButtonResult.Cancel);
        }
    }
}