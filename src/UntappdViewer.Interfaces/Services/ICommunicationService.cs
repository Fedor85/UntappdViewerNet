using System;
using System.Collections.Generic;
using System.Windows;

namespace UntappdViewer.Interfaces.Services
{
    public interface ICommunicationService
    {
        MessageBoxResult Ask(string caption, string message);

        void ShowMessage(string caption, string message);

        void ShowError(string caption, string message);

        string OpenFile(string initialDirectory, List<string> extensions);

        event Action<string> ShowMessageOnStatusBarEnvent;
         
        event Action ClearMessageOnStatusBarEnvent;

        void ShowMessageOnStatusBar(string message);

        void ClearMessageOnStatusBar();
    }
}