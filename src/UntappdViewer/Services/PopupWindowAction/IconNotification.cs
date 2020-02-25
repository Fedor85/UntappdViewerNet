using System.Windows.Media;
using Prism.Interactivity.InteractionRequest;

namespace UntappdViewer.Services.PopupWindowAction
{
    public class IconNotification: Notification, IIconNotification
    {
        public ImageSource Icon { get; set; }
    }
}