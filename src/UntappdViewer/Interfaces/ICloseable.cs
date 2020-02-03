using System;
using System.ComponentModel;

namespace UntappdViewer.Interfaces
{
    public interface ICloseable
    {
        void Closing(object sender, CancelEventArgs e);
    }
}
