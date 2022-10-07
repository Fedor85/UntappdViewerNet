using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using UntappdViewer.Utils;

namespace UntappdViewer.Behaviors
{
    public class SynchronisedScrollToken
    {
        private List<ScrollViewer> registeredScrolls = new List<ScrollViewer>();

        public void Register(ScrollViewer scroll)
        {
            scroll.ScrollChanged += ScrollChanged;
            registeredScrolls.Add(scroll);
        }

        public void Unregister(ScrollViewer scroll)
        {
            scroll.ScrollChanged -= ScrollChanged;
            registeredScrolls.Remove(scroll);
        }

        private void ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer sendingScroll = sender as ScrollViewer;
            if (sendingScroll == null)
                return;

            foreach (ScrollViewer potentialScroll in registeredScrolls.Where(item => item != sendingScroll))
            {
                if (!MathHelper.DoubleCompare(potentialScroll.VerticalOffset, sendingScroll.VerticalOffset))
                    potentialScroll.ScrollToVerticalOffset(sendingScroll.VerticalOffset);

                if (!MathHelper.DoubleCompare(potentialScroll.HorizontalOffset, sendingScroll.HorizontalOffset))
                    potentialScroll.ScrollToHorizontalOffset(sendingScroll.HorizontalOffset);
            }
        }
    }
}