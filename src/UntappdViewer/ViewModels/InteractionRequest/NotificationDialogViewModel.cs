using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Prism.Services.Dialogs;
using UntappdViewer.ViewModels.Base;

namespace UntappdViewer.ViewModels
{
    public class NotificationDialogViewModel : BaseDialogModel
    {
        private ImageSource iconSource;

        public ImageSource IconSource
        {
            get { return iconSource; }
            set { SetProperty(ref iconSource, value); }
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
           base.OnDialogOpened(parameters);
            if (parameters.Keys.Contains("icon"))
                IconSource = Imaging.CreateBitmapSourceFromHIcon(parameters.GetValue<Icon>("icon").Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }
    }
}