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
using UntappdViewer.Views;

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
            IconNotification notification = GetINotification<IconNotification>(new IconNotification(), caption, message);
            AddIcon(notification, SystemIcons.Warning);
            NotificationRequest.Raise(notification);
        }

        public void ShowError(string caption, string message)
        {
            IconNotification notification = GetINotification<IconNotification>(new IconNotification(), caption, message);
            AddIcon(notification, SystemIcons.Error);
            NotificationRequest.Raise(notification);
        }

        public bool Ask(string caption, string message)
        {
            bool result = false;
            ConfirmationRequest.Raise(GetINotification<Confirmation>(new Confirmation(), caption, message), c => result = c.Confirmed);
            return result;
        }

        public ConfirmationResult<string> AskReplaceText(string caption, string text)
        {
            bool result = false;
            TextBox textBox = new TextBox();
            textBox.TextContent.Text = text;
            Confirmation confirmation = new Confirmation();
            confirmation.Title = caption;
            confirmation.Content = textBox;
            ConfirmationRequest.Raise(confirmation, c => result = c.Confirmed);
            return new ConfirmationResult<string>(textBox.TextContent.Text, result);
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

        public string SaveFile(string initialDirectory, string fileName, List<string> extensions)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (!String.IsNullOrEmpty(initialDirectory))
                saveFileDialog.InitialDirectory = initialDirectory;

            saveFileDialog.FileName = fileName;
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.Filter = GetFilter(extensions);
            bool? result = saveFileDialog.ShowDialog(Application.Current.MainWindow);
            if (result.HasValue && result.Value)
                return saveFileDialog.FileName;

            return String.Empty;
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

        private T GetINotification<T>(INotification notification ,string caption, string message)
        {
            notification.Title = caption;
            Views.MessageBox messageBox = new Views.MessageBox();
            messageBox.Message.Text = message;
            notification.Content = messageBox;
            return (T) notification;
        }

        private void AddIcon(IIconNotification iconNotification, Icon icon)
        {
            if (icon != null)
                iconNotification.Icon = ConvertIconToImageSource(icon);
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