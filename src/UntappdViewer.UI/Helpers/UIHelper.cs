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

        public static T FindFirstChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject childObject = VisualTreeHelper.GetChild(parent, i);

                T child = childObject as T;
                if (child != null)
                    return child;

                child = FindFirstChild<T>(childObject);
                if (child != null)
                    return child;
            }
            return null;
        }
    }
}