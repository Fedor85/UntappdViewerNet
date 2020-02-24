using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Microsoft.Win32;
using Prism.Interactivity.InteractionRequest;

namespace UntappdViewer.Services
{
    public class InteractionRequestService
    {
        public InteractionRequest<IConfirmation> ConfirmationRequest { get; }

       

        public InteractionRequest<INotification> NotificationRequest { get; }

        public event Action<string> ShowMessageOnStatusBarEnvent;

        public event Action ClearMessageOnStatusBarEnvent;

        private Confirmation confirmation;

        private Notification notification;

        public InteractionRequestService()
        {
            ConfirmationRequest = new InteractionRequest<IConfirmation>();
            confirmation = new Confirmation();
            NotificationRequest = new InteractionRequest<INotification>();
            notification = new Notification();
        }

        public void ShowMessage(string caption, string message)
        {
            notification.Title = caption;
            notification.Content = message;
            NotificationRequest.Raise(notification);
            ResetNotification(notification);
        }

        public bool Ask(string caption, string message)
        {
            bool result = false;
            confirmation.Title = caption;
            confirmation.Content = message;
            ConfirmationRequest.Raise(confirmation, c=> result = c.Confirmed);
            ResetNotification(confirmation);
            return result;
        }

        public void ShowMessageOnStatusBar(string message)
        {
            if (ShowMessageOnStatusBarEnvent != null)
                ShowMessageOnStatusBarEnvent.Invoke(message);
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

        public void ClearMessageOnStatusBar()
        {
            if (ClearMessageOnStatusBarEnvent != null)
                ClearMessageOnStatusBarEnvent.Invoke();
        }

        private void ResetNotification(INotification notification)
        {
            notification.Title = String.Empty;
            notification.Content = null;
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