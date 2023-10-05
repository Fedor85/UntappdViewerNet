using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using UntappdViewer.UI.Helpers;

namespace UntappdViewer.UI.Controls
{
    public class ZoomBorder : Border
    {
        private UIElement child;

        private Point origin;

        private Point start;

        public override UIElement Child
        {
            get { return base.Child; }
            set
            {
                if (value != null && value != Child)
                    Initialize(value);

                base.Child = value;
            }
        }

        private void Initialize(UIElement element)
        {
            child = element;
            if (child == null)
                return;

            ZoomHelper.InitializeTransformGroup(child);

            MouseWheel += ChildMouse;
            MouseLeftButtonDown += ChildMouseLeft;
            MouseLeftButtonUp += ChildMouseLeftButtonUp;
            MouseMove += ChildMouseMove;
            PreviewMouseRightButtonDown += ChildPreviewMouseRightButtonDown;
        }

        #region Child Events

        private void ChildMouse(object sender, MouseWheelEventArgs e)
        {
            ZoomHelper.Zoom(child, e.Delta, e.GetPosition(child));
        }

        private void ChildMouseLeft(object sender, MouseButtonEventArgs e)
        {
            if (child == null)
                return;

            TranslateTransform translateTransform = ZoomHelper.GetTranslateTransform(child);
            start = e.GetPosition(this);
            origin = new Point(translateTransform.X, translateTransform.Y);
            Cursor = Cursors.Hand;
            child.CaptureMouse();
        }

        private void ChildMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (child == null)
                return;

            child.ReleaseMouseCapture();
            Cursor = Cursors.Arrow;
        }

        private void ChildPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ZoomHelper.Reset(child);
        }

        private void ChildMouseMove(object sender, MouseEventArgs e)
        {
            if (child == null || !child.IsMouseCaptured)
                return;

            TranslateTransform translateTransform = ZoomHelper.GetTranslateTransform(child);
            Vector v = start - e.GetPosition(this);
            translateTransform.X = origin.X - v.X;
            translateTransform.Y = origin.Y - v.Y;
        }

        #endregion
    }
}