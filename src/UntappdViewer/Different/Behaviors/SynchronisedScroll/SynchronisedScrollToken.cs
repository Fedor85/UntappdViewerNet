using System.Collections.Generic;
using System.Windows.Controls;

namespace UntappdViewer.Behaviors
{
    public class SynchronisedScrollToken
    {
        private List<ScrollViewer> registeredScrolls = new List<ScrollViewer>();

        public void Unregister(ScrollViewer scroll)
        {
            scroll.ScrollChanged -= ScrollChanged;
            registeredScrolls.Remove(scroll);
        }

        public void Register(ScrollViewer scroll)
        {
            scroll.ScrollChanged += ScrollChanged;
            registeredScrolls.Add(scroll);
        }

        private void ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer sendingScroll = sender as ScrollViewer;
            foreach (ScrollViewer potentialScroll in registeredScrolls)
            {
                if (potentialScroll == sendingScroll)
                    continue;

                if (potentialScroll.VerticalOffset != sendingScroll.VerticalOffset)
                    potentialScroll.ScrollToVerticalOffset(sendingScroll.VerticalOffset);

                if (potentialScroll.HorizontalOffset != sendingScroll.HorizontalOffset)
                    potentialScroll.ScrollToHorizontalOffset(sendingScroll.HorizontalOffset);
            }
        }
    }
}
