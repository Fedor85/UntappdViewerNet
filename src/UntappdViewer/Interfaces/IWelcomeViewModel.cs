using System.Windows;

namespace UntappdViewer.Interfaces
{
    public interface IWelcomeViewModel
    {
        void OpenFileButtonClick(object sender, RoutedEventArgs e);

        void FileOnDrop(object sender, DragEventArgs e);
    }
}
