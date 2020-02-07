using Prism.Mvvm;

namespace UntappdViewer.ViewModels
{
    public class StatusBarViewModel : BindableBase
    {
        private string message;

        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                OnPropertyChanged("Message");
            }
        }
    }
}