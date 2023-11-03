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

            clipRect.RadiusX = clipRect.RadiusY = Math.Max(0.0, CornerRadius.TopLeft - BorderThickness.Left * 0.5);
            clipRect.Rect = new Rect(Child.RenderSize);
            Child.Clip = clipRect;
        }
    }
}