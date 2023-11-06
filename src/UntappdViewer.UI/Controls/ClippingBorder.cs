using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace UntappdViewer.UI.Controls
{
    public class ClippingBorder : Border
    {
        private RectangleGeometry clipRect = new RectangleGeometry();

        private object oldClip;

        protected override void OnRender(DrawingContext dc)
        {
            OnApplyChildClip();
            base.OnRender(dc);
        }

        public override UIElement Child
        {
            get { return base.Child; }
            set { SetChild(value); }
        }

        private void SetChild(UIElement child)
        {
            if (Child == child)
                return;

            // Restore original clipping
            if (Child != null)
                Child.SetValue(ClipProperty, oldClip);

            // If we dont set it to null we could leak a Geometry object
            oldClip = child?.ReadLocalValue(ClipProperty);

            base.Child = child;
        }

        protected virtual void OnApplyChildClip()
        {
            if (Child == null)
                return;

            FrameworkElement frameworkElement = Child as FrameworkElement;
            if (frameworkElement != null && !frameworkElement.IsLoaded)
                frameworkElement.Loaded += ChildLoaded;
            else
                ChildClip();
        }

        private void ChildClip()
        {
            clipRect.RadiusX = clipRect.RadiusY = Math.Max(0.0, CornerRadius.TopLeft - BorderThickness.Left * 0.5);
            clipRect.Rect = new Rect(Child.RenderSize);
            Child.Clip = clipRect;
        }

        private void ChildLoaded(object sender, RoutedEventArgs e)
        {
            ChildClip();
            ((FrameworkElement)sender).Loaded -= ChildLoaded;
        }
    }
}