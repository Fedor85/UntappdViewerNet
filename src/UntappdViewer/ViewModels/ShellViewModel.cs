using System.ComponentModel;
using System.Windows;
using Prism.Mvvm;
using UntappdViewer.Interfaces;

namespace UntappdViewer.ViewModels
{
    public class ShellViewModel: BindableBase, ICloseable
    {
        public string Title { get; set; } = "UntappdViewer";

        public void Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = MessageBox.Show("Вы желаете закрыть приложение?", "Внимание!", MessageBoxButton.OKCancel) != MessageBoxResult.OK;
        }
    }
}
