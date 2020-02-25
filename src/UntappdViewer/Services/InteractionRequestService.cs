using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Prism.Interactivity.InteractionRequest;
using UntappdViewer.Services.PopupWindowAction;

namespace UntappdViewer.Services
{
    public class InteractionRequestService
    {
        public InteractionRequest<IConfirmation> ConfirmationRequest { get; }

      
        public InteractionRequest<INotification> NotificationRequest { get; }

        public event Action<string> ShowMessageOnStatusBarEnvent;

        public event Action ClearMessageOnStatusBarEnvent;

        public InteractionRequestService()
        {
            ConfirmationRequest = new InteractionRequest<IConfirmation>();
            NotificationRequest = new InteractionRequest<INotification>();
        }

        public void ShowMessage(string caption, string message)
        {
            NotificationRequest.Raise(GetIconNotification(caption, message, SystemIcons.Warning));
        }

        public void ShowError(string caption, string message)
        {
            NotificationRequest.Raise(GetIconNotification(caption, message, SystemIcons.Error));
        }

        public bool Ask(string caption, string message)
        {
            bool result = false;
            ConfirmationRequest.Raise(GetIconConfirmation(caption, message, null), c => result = c.Confirmed);
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

        private IconNotification GetIconNotification(string caption, string message, Icon icon)
        {
            IconNotification notification = new IconNotification();
            notification.Title = caption;
            notification.Content = message;
            if (icon != null)
                notification.Icon = ConvertIconToImageSource(icon);

            return notification;
        }

        private IconConfirmation GetIconConfirmation(string caption, string message, Icon icon)
        {
            IconConfirmation confirmation = new IconConfirmation();
            confirmation.Title = caption;
            confirmation.Content = message;
            if (icon != null)
                confirmation.Icon = ConvertIconToImageSource(icon);

            return confirmation;
        }

        private ImageSource ConvertIconToImageSource(Icon icon)
        {
            return Imaging.CreateBitmapSourceFromHIcon(
                icon.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
        }
    }
}