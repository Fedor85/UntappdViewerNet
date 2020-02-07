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
        public CommunicationService()
        {
            
        }

        public MessageBoxResult Ask(string caption, string message)
        {
            return MessageBox.Show(message, caption, MessageBoxButton.OKCancel);
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

        public event Action<string> ShowMessageEnvent;

        public event Action ClearMessageEnvent;

        public void ShowMessage(string message)
        {
            if (ShowMessageEnvent != null)
                ShowMessageEnvent.Invoke(message);
        }

        public void ClearMessage()
        {
            if (ClearMessageEnvent != null)
                ClearMessageEnvent.Invoke();
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