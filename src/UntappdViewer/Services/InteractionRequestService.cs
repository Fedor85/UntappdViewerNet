using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Prism.Services.Dialogs;
using UntappdViewer.Interfaces.Services;
using Application = System.Windows.Application;
using DialogResult = System.Windows.Forms.DialogResult;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace UntappdViewer.Services
{
    public class InteractionRequestService: IInteractionRequestService
    {
        private IDialogService dialogService;

        public event Action<string> ShowMessageOnStatusBarEvent;

        public event Action ClearMessageOnStatusBarEnvent;

        public InteractionRequestService(IDialogService dialogService)
        {
            this.dialogService = dialogService;
        }

        public void ShowMessage(string caption, string message)
        {
            ShowNotificationDialog(caption, message, SystemIcons.Warning);
        }

        public void ShowError(string caption, string message)
        {
            ShowNotificationDialog(caption, message, SystemIcons.Error);
        }

        public bool Ask(string caption, string message)
        {
            DialogParameters dialogParameters = new DialogParameters();
            dialogParameters.Add("caption", caption);
            dialogParameters.Add("message", message);
            bool dialogResult = false;
            dialogService.ShowDialog("AskDialog", dialogParameters, dialog =>
            {
                if (dialog.Result == ButtonResult.OK)
                    dialogResult = true;
            });
            return dialogResult;
        }

        public bool AskReplaceText(string caption, ref string text)
        {
            DialogParameters dialogParameters = new DialogParameters();
            dialogParameters.Add("caption", caption);
            dialogParameters.Add("text", text);
            string dialogText = text;
            bool dialogResult = false;

            dialogService.ShowDialog("TextBoxDialog", dialogParameters, dialog=>
            {
                if (dialog.Result == ButtonResult.OK)
                {
                    dialogText = dialog.Parameters.GetValue<string>("name").Trim();
                    dialogResult = true;
                }
            });

            text = dialogText;
            return dialogResult;
        }

        public void ShowMessageOnStatusBar(string message)
        {
            if (ShowMessageOnStatusBarEvent != null)
                ShowMessageOnStatusBarEvent.Invoke(message);
        }

        public string OpenFile(string initialDirectory, List<string> extensions)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (!String.IsNullOrEmpty(initialDirectory))
                openFileDialog.InitialDirectory = initialDirectory;

            openFileDialog.Filter = GetFilter(extensions);
            openFileDialog.ShowDialog(Application.Current.MainWindow);
            return openFileDialog.FileName;
        }

        public string FolderBrowser(string initialDirectory)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (!String.IsNullOrEmpty(initialDirectory))
                folderBrowserDialog.SelectedPath = initialDirectory;

            return folderBrowserDialog.ShowDialog() != DialogResult.OK ? String.Empty : folderBrowserDialog.SelectedPath;
        }

        public string SaveFile(string initialDirectory, string fileName, List<string> extensions)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (!String.IsNullOrEmpty(initialDirectory))
                saveFileDialog.InitialDirectory = initialDirectory;

            saveFileDialog.FileName = fileName;
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.Filter = GetFilter(extensions);
            bool? result = saveFileDialog.ShowDialog(Application.Current.MainWindow);
            return result.HasValue && result.Value ? saveFileDialog.FileName : String.Empty;
        }

        public void ClearMessageOnStatusBar()
        {
            if (ClearMessageOnStatusBarEnvent != null)
                ClearMessageOnStatusBarEnvent.Invoke();
        }

        private string GetFilter(List<string> extensions)
        {
            if (extensions.Count == 0)
                return String.Empty;

            StringBuilder filter = new StringBuilder();
            foreach (string extension in extensions)
                filter.AppendFormat("(*.{0})|*.{0}|", extension);

            return filter.Remove(filter.Length - 1, 1).ToString();
        }

        private void ShowNotificationDialog(string caption, string message, Icon icon)
        {
            DialogParameters dialogParameters = new DialogParameters();
            dialogParameters.Add("caption", caption);
            dialogParameters.Add("message", message);
            dialogParameters.Add("icon", icon);
            dialogService.ShowDialog("NotificationDialog", dialogParameters, null);
        }
    }
}