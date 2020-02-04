using System.ComponentModel;

namespace UntappdViewer.Interfaces
{
    public interface IClosable
    {
        void Closing(object sender, CancelEventArgs e);
    }
}