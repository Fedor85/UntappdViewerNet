using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace UntappdViewer.Behaviors
{
    public class ParentScrollingToken
    {
        private ScrollViewer ParentScrollViewer;

        private List<ScrollViewer> ChildScrollViewers;

        public ParentScrollingToken()
        {
            ChildScrollViewers = new List<ScrollViewer>();
        }

        public void SetParentScrollViewer(ScrollViewer scrollViewer)
        {
            ParentScrollViewer = scrollViewer;
        }

        public void AddChildScrollViewer(ScrollViewer scrollViewer)
        {
            scrollViewer.PreviewMouseWheel += PreviewMouseWheel;
            ChildScrollViewers.Add(scrollViewer);
        }

        public void Clear()
        {
            foreach (ScrollViewer scrollViewer in ChildScrollViewers)
                scrollViewer.PreviewMouseWheel -= PreviewMouseWheel;

            ChildScrollViewers.Clear();
        }

        private void PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (ParentScrollViewer == null)
                return;

            //Block child PreviewMouseWheel
            e.Handled = true;
            MouseWheelEventArgs mouseWheelEventArgs = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
            mouseWheelEventArgs.RoutedEvent = UIElement.MouseWheelEvent;
            ParentScrollViewer.RaiseEvent(mouseWheelEventArgs);
        }
    }
}