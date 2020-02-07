using System;
using System.Collections.Generic;
using System.Windows;

namespace UntappdViewer.Interfaces.Services
{
    public interface ICommunicationService
    {
        MessageBoxResult Ask(string caption, string message);

        string OpenFile(string initialDirectory, List<string> extensions);

        event Action<string> ShowMessageEnvent;
         
        event Action ClearMessageEnvent;

        void ShowMessage(string message);

        void ClearMessage();
    }
}