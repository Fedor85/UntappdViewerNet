using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace UntappdViewer.Behaviors
{
    public class ParentScrollingToken
    {
        private ScrollViewer parentScrollViewer;

        private List<ScrollViewer> childScrollViewers;

        public ParentScrollingToken()
        {
            childScrollViewers = new List<ScrollViewer>();
        }

        public void SetParentScrollViewer(ScrollViewer scrollViewer)
        {
            parentScrollViewer = scrollViewer;
        }

        public void AddChildScrollViewer(ScrollViewer scrollViewer)
        {
            scrollViewer.PreviewMouseWheel += PreviewMouseWheel;
            childScrollViewers.Add(scrollViewer);
        }

        public void Clear()
        {
            foreach (ScrollViewer scrollViewer in childScrollViewers)
                scrollViewer.PreviewMouseWheel -= PreviewMouseWheel;

            childScrollViewers.Clear();
        }

        private void PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (parentScrollViewer == null)
                return;

            //Block child PreviewMouseWheel
            e.Handled = true;
            MouseWheelEventArgs mouseWheelEventArgs = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
            mouseWheelEventArgs.RoutedEvent = UIElement.MouseWheelEvent;
            parentScrollViewer.RaiseEvent(mouseWheelEventArgs);
        }
    }
}