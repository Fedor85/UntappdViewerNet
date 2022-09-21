using System;
using System.Collections.Generic;

namespace UntappdViewer.Interfaces.Services
{
    public interface IInteractionRequestService
    {
        event Action<string> ShowMessageOnStatusBarEvent;

        void ShowMessage(string caption, string message);

        void ShowError(string caption, string message);

        bool Ask(string caption, string message);

        bool AskReplaceText(string caption, ref string text);

        void ShowMessageOnStatusBar(string message);

        string GetCurrentwMessageOnStatusBar();

        string OpenFile(string initialDirectory, List<string> extensions);

        string FolderBrowser(string initialDirectory);

        string SaveFile(string initialDirectory, string fileName, List<string> extensions);

        void ClearMessageOnStatusBar();
    }
}