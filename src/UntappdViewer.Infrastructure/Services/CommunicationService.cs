using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Microsoft.Win32;
using UntappdViewer.Interfaces.Services;

namespace UntappdViewer.Infrastructure.Services
{
    public class CommunicationService : ICommunicationService
    {
        public MessageBoxResult Ask(string caption, string message)
        {
            return MessageBox.Show(message, caption, MessageBoxButton.OKCancel);
        }

        public void ShowMessage(string caption, string message)
        {
            MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public void ShowError(string caption, string message)
        {
            MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public string OpenFile(string initialDirectory, List<string> extensions)
         {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (!String.IsNullOrEmpty(initialDirectory))
                 openFileDialog.InitialDirectory = initialDirectory;

            openFileDialog.Filter = GetFilter(extensions);
            openFileDialog.ShowDialog();
            return openFileDialog.FileName;
        }

        public event Action<string> ShowMessageOnStatusBarEnvent;

        public event Action ClearMessageOnStatusBarEnvent;

        public void ShowMessageOnStatusBar(string message)
        {
            if (ShowMessageOnStatusBarEnvent != null)
                ShowMessageOnStatusBarEnvent.Invoke(message);
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
    }
}