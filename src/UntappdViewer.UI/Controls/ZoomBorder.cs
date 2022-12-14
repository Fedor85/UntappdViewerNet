using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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

            TransformGroup transformGroup = new TransformGroup();

            ScaleTransform scaleTransform = new ScaleTransform();
            transformGroup.Children.Add(scaleTransform);

            TranslateTransform translateTransform = new TranslateTransform();
            transformGroup.Children.Add(translateTransform);

            child.RenderTransform = transformGroup;
            child.RenderTransformOrigin = new Point(0.0, 0.0);
            MouseWheel += ChildMouse;
            MouseLeftButtonDown += ChildMouseLeft;
            MouseLeftButtonUp += ChildMouseLeftButtonUp;
            MouseMove += ChildMouseMove;
            PreviewMouseRightButtonDown += ChildPreviewMouseRightButtonDown;
        }

        private void Reset()
        {
            if (child == null)
                return;

            // reset zoom
            ScaleTransform scaleTransform = GetScaleTransform(child);
            scaleTransform.ScaleX = 1.0;
            scaleTransform.ScaleY = 1.0;

            // reset pan
            TranslateTransform translateTransform = GetTranslateTransform(child);
            translateTransform.X = 0.0;
            translateTransform.Y = 0.0;
        }

        private TranslateTransform GetTranslateTransform(UIElement element)
        {
            return GetChildrenTransform<TranslateTransform>(element);
        }

        private ScaleTransform GetScaleTransform(UIElement element)
        {
            return GetChildrenTransform<ScaleTransform>(element);
        }

        private T GetChildrenTransform<T>(UIElement element) where T : Transform
        {
            return (T)((TransformGroup)element.RenderTransform).Children.First(item => item is T);
        }

        #region Child Events

        private void ChildMouse(object sender, MouseWheelEventArgs e)
        {
            if (child == null)
                return;

            ScaleTransform scaleTransform = GetScaleTransform(child);
            TranslateTransform translateTransform = GetTranslateTransform(child);

            double zoom = e.Delta > 0 ? .2 : -.2;
            if (!(e.Delta > 0) && (scaleTransform.ScaleX < .4 || scaleTransform.ScaleY < .4))
                return;

            Point relative = e.GetPosition(child);
            double abosuluteX = relative.X * scaleTransform.ScaleX + translateTransform.X;
            double abosuluteY = relative.Y * scaleTransform.ScaleY + translateTransform.Y;

            scaleTransform.ScaleX += zoom;
            scaleTransform.ScaleY += zoom;

            translateTransform.X = abosuluteX - relative.X * scaleTransform.ScaleX;
            translateTransform.Y = abosuluteY - relative.Y * scaleTransform.ScaleY;
        }

        private void ChildMouseLeft(object sender, MouseButtonEventArgs e)
        {
            if (child == null)
                return;

            TranslateTransform translateTransform = GetTranslateTransform(child);
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
            Reset();
        }

        private void ChildMouseMove(object sender, MouseEventArgs e)
        {
            if (child == null || !child.IsMouseCaptured)
                return;

            TranslateTransform translateTransform = GetTranslateTransform(child);
            Vector v = start - e.GetPosition(this);
            translateTransform.X = origin.X - v.X;
            translateTransform.Y = origin.Y - v.Y;
        }

        #endregion
    }
}