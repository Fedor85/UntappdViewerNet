using System;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace UntappdViewer.ViewModels.Base
{
    public class BaseDialogModel : BindableBase, IDialogAware
    {
        private string title;

        private string message;

        public ICommand OkDialogCommand { get; }

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public string Message
        {
            get { return message; }
            set { SetProperty(ref message, value); }
        }

        public event Action<IDialogResult> RequestClose;

        public BaseDialogModel()
        {
            OkDialogCommand = new DelegateCommand(OkCloseDialog);
        }


        protected virtual void OkCloseDialog()
        {
            CloseDialog(ButtonResult.OK);
        }

        protected void CloseDialog(ButtonResult buttonResult)
        {
            RequestClose?.Invoke(new DialogResult(buttonResult));
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public virtual void OnDialogOpened(IDialogParameters parameters)
        {
            Title = parameters.GetValue<string>("caption");
            Message = parameters.GetValue<string>("message");
        }
    }
}