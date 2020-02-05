using System.Windows;

namespace UntappdViewer.Interfaces.Services
{
    public interface IDialogService
    {
        MessageBoxResult Ask(string caption, string message);

        string OpenFile(params string[] extensions);
    }
}