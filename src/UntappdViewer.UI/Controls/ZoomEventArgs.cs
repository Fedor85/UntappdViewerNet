using System;
using System.Windows;

namespace UntappdViewer.UI.Controls
{
    public class ZoomEventArgs: EventArgs
    {
        public Point Point { get; }

        public int Delta { get; }

        public ZoomEventArgs(Point point, int delta)
        {
            Point = point;
            Delta = delta;
        }
    }
}