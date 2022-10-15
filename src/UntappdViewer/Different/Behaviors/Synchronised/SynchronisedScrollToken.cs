using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using UntappdViewer.Utils;

namespace UntappdViewer.Behaviors
{
    public class SynchronisedScrollToken : SynchronisedBaseToken<ScrollViewer>
    {
        public override void Register(ScrollViewer scroll)
        {
            base.Register(scroll);
            scroll.ScrollChanged += ScrollChanged;
        }

        public override void Unregister(ScrollViewer scroll)
        {
            base.Unregister(scroll);
            scroll.ScrollChanged -= ScrollChanged;
        }

        private void ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer sendingScroll = GetObject(sender);
            if (sendingScroll == null)
                return;

            foreach (ScrollViewer potentialScroll in GetOtherItems(sendingScroll))
            {
                if (!MathHelper.DoubleCompare(potentialScroll.VerticalOffset, sendingScroll.VerticalOffset))
                    potentialScroll.ScrollToVerticalOffset(sendingScroll.VerticalOffset);

                if (!MathHelper.DoubleCompare(potentialScroll.HorizontalOffset, sendingScroll.HorizontalOffset))
                    potentialScroll.ScrollToHorizontalOffset(sendingScroll.HorizontalOffset);
            }
        }
    }
}