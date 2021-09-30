using System;
using System.Collections.Generic;

namespace UntappdViewer.Interfaces.Services
{
    public interface IInteractionRequestService
    {
        event Action<string> ShowMessageOnStatusBarEvent;

        event Action ClearMessageOnStatusBarEnvent;

        void ShowMessage(string caption, string message);

        void ShowError(string caption, string message);

        bool Ask(string caption, string message);

        string AskReplaceText(string caption, string text);

        void ShowMessageOnStatusBar(string message);

        string OpenFile(string initialDirectory, List<string> extensions);

        string FolderBrowser(string initialDirectory);

        string SaveFile(string initialDirectory, string fileName, List<string> extensions);

        void ClearMessageOnStatusBar();
    }
}
