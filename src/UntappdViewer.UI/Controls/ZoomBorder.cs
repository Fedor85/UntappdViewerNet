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

        public bool IsInitializeEvent { get; set; } = true;

        private void Initialize(UIElement element)
        {
            child = element;

            ZoomHelper.InitializeTransformGroup(child);
            if (IsInitializeEvent)
            {
                MouseWheel += ChildMouseWheel;
                MouseLeftButtonDown += ChildMouseLeftButtonDown;
                MouseLeftButtonUp += ChildMouseLeftButtonUp;
                MouseMove += ChildMouseMove;
                PreviewMouseRightButtonDown += ChildPreviewMouseRightButtonDown;
            }
        }

        public void ZoomChild(MouseWheelEventArgs mouseEventArgs)
        {
            if (child != null && UIHelper.IsMouseOver(child, mouseEventArgs))
                ZoomHelper.Zoom(child, mouseEventArgs.Delta, mouseEventArgs.GetPosition(child));
        }

        public void ChildMouseLeftButtonDown(MouseButtonEventArgs mouseEventArgs)
        {
            if (child == null || !UIHelper.IsMouseOver(child, mouseEventArgs))
                return;

            TranslateTransform translateTransform = ZoomHelper.GetTranslateTransform(child);
            start = mouseEventArgs.GetPosition(this);
            origin = new Point(translateTransform.X, translateTransform.Y);
            Cursor = Cursors.Hand;
            child.CaptureMouse();
        }

        public void ChildMouseLeftButtonUp(MouseButtonEventArgs mouseEventArgs)
        {
            if (child == null)
                return;

            child.ReleaseMouseCapture();
            Cursor = Cursors.Arrow;
        }

        public void MoveChild(MouseEventArgs mouseEventArgs)
        {
            if (child == null || !child.IsMouseCaptured)
                return;

            TranslateTransform translateTransform = ZoomHelper.GetTranslateTransform(child);
            Vector v = start - mouseEventArgs.GetPosition(this);
            translateTransform.X = origin.X - v.X;
            translateTransform.Y = origin.Y - v.Y;
        }

        public void ZoomChildReset()
        {
            ZoomHelper.Reset(child);
        }

        #region Child Events

        private void ChildMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ZoomChild(e);
        }

        private void ChildMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ChildMouseLeftButtonDown(e);
        }

        private void ChildMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ChildMouseLeftButtonUp(e);
        }

        private void ChildMouseMove(object sender, MouseEventArgs e)
        {
            MoveChild(e);
        }

        private void ChildPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ZoomChildReset();
        }

        #endregion
    }
}