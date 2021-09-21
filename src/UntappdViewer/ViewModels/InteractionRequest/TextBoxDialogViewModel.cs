using System;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace UntappdViewer.ViewModels
{
    public class TextBoxDialogViewModel : BindableBase, IDialogAware
    {
        private string title;

        private string text;

        public ICommand CloseDialogCommand { get; }

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public string Text
        {
            get { return text; }
            set { SetProperty(ref text, value); }
        }

        public event Action<IDialogResult> RequestClose;

        public TextBoxDialogViewModel()
        {
            CloseDialogCommand = new DelegateCommand<string>(CloseDialog);
        }

        private void CloseDialog(string parameter)
        {
            bool result = Convert.ToBoolean(parameter);
            DialogResult dialogResult;
            if (result)
            {
                dialogResult = new DialogResult(ButtonResult.OK);
                dialogResult.Parameters.Add("name", Text);
            }
            else
            {
                dialogResult = new DialogResult(ButtonResult.Cancel);
            }
            RequestClose?.Invoke(dialogResult);
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Title = parameters.GetValue<string>("caption");
            Text = parameters.GetValue<string>("text");
        }
    }
}
