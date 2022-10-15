using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace UntappdViewer.Behaviors
{
    public class SynchronisedWidthToken : SynchronisedBaseToken<Control>
    {
        private int countSizeChanged = 0;

        public override void Register(Control control)
        {
            base.Register(control);
            control.SizeChanged += SizeChanged;
        }

        public override void Unregister(Control control)
        {
            base.Unregister(control);
            control.SizeChanged -= SizeChanged;
        }

        private void SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            Control control = GetObject(sender);
            if (control == null)
                return;

            countSizeChanged++;

            if(countSizeChanged != Items.Count)
                return;

            FinishSetWidth(control);
        }

        private void FinishSetWidth(Control control)
        {
            double maxWidth = control.ActualWidth;
            foreach (Control item in GetOtherItems(control))
                maxWidth = Math.Max(maxWidth, item.ActualWidth);

            foreach (Control item in Items)
                item.Width = maxWidth;

            foreach (Control item in new List<Control>(Items))
                Unregister(item);

             countSizeChanged = 0;
        }

        private void RemoveEvent()
        {
            foreach (Control item in Items)
                item.SizeChanged -= SizeChanged;
        }
    }
}