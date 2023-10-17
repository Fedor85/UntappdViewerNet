using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace UntappdViewer.UI.Helpers
{
    public static class UIHelper
    {
        public static Window GetWindow(UserControl control)
        {
            return FindParent<Window>(control);
        }

        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            if (parentObject == null)
                return null;

            T parent = parentObject as T;
            return parent != null ? parent : FindParent<T>(parentObject);
        }
    }
}