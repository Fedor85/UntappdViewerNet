using System.Windows;
using UntappdViewer.Interfaces.Services;

namespace UntappdViewer.Services
{
    public class DialogService : IDialogService
    {
        public MessageBoxResult Ask(string caption, string message)
        {
            return MessageBox.Show(message, caption, MessageBoxButton.OKCancel);
        }
    }
}
