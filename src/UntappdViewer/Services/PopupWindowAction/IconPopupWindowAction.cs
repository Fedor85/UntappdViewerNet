using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Prism.Interactivity.InteractionRequest;

namespace UntappdViewer.Services.PopupWindowAction
{
    public class IconPopupWindowAction : Prism.Interactivity.PopupWindowAction
    {
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(ImageSource), typeof(IconPopupWindowAction), new PropertyMetadata(null));

        public ImageSource Icon
        {
            get { return (ImageSource)this.GetValue(IconProperty); }
            set { this.SetValue(IconProperty, value); }
        }

        protected override Window GetWindow(INotification notification)
        {
            Window defaultWindow = base.GetWindow(notification);
            IIconNotification iconNotification = notification as IIconNotification;
                defaultWindow.Icon = iconNotification.Icon;

            defaultWindow.PreviewKeyDown += PreviewKeyDown;
            return defaultWindow;
        }

        private void PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                ((Window)sender).Close();
        }
    }
}