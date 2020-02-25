using System.Windows.Media;
using Prism.Interactivity.InteractionRequest;

namespace UntappdViewer.Services.PopupWindowAction
{
    class IconConfirmation : Confirmation, IIconNotification
    {
        public ImageSource Icon { get; set; }
    }
}