using System.ComponentModel;
using Prism.Mvvm;
using UntappdViewer.Interfaces;

namespace UntappdViewer.ViewModels
{
    public class ShellViewModel: BindableBase, ICloseable
    {
        public string Title { get; set; } = "UntappdViewer";

        public void Closing(object sender, CancelEventArgs e)
        {
            
        }
    }
}
