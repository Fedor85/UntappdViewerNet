using System.Windows;
using System.Windows.Controls;

namespace UntappdViewer.UI.Helpers
{
    public static class UIHelper
    {
        public static Window GetWindow(UserControl control)
        {
            if (control == null)
                return null;

            Window window = Window.GetWindow(control);
            if (window != null)
                return window;

            UserControl parentControl = control.Parent as UserControl;
            return GetWindow(parentControl);
        }
    }
}