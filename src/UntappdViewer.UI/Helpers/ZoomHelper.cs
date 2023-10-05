using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace UntappdViewer.UI.Helpers
{
    public static class ZoomHelper
    {
        public static void InitializeTransformGroup(UIElement element)
        {
            if (element == null)
                return;

            TransformGroup transformGroup = new TransformGroup();

            ScaleTransform scaleTransform = new ScaleTransform();
            transformGroup.Children.Add(scaleTransform);

            TranslateTransform translateTransform = new TranslateTransform();
            transformGroup.Children.Add(translateTransform);

            element.RenderTransform = transformGroup;
            element.RenderTransformOrigin = new Point(0, 0);
        }

        public static void Zoom(UIElement element, int delta, Point position)
        {
            if (element == null)
                return;

            ScaleTransform scaleTransform = GetScaleTransform(element).CloneCurrentValue(); ;

            double zoom = delta > 0 ? 0.2 : -0.2;
            if (!(delta > 0) && (scaleTransform.ScaleX < 0.4 || scaleTransform.ScaleY < 0.4))
                return;

            TranslateTransform translateTransform = GetTranslateTransform(element).CloneCurrentValue();

            double abosuluteX = position.X * scaleTransform.ScaleX + translateTransform.X;
            double abosuluteY = position.Y * scaleTransform.ScaleY + translateTransform.Y;

            scaleTransform.ScaleX += zoom;
            scaleTransform.ScaleY += zoom;

            translateTransform.X = abosuluteX - position.X * scaleTransform.ScaleX;
            translateTransform.Y = abosuluteY - position.Y * scaleTransform.ScaleY;

            TransformGroup transformGroup = new TransformGroup();
            transformGroup.Children.Add(scaleTransform);
            transformGroup.Children.Add(translateTransform);
            element.RenderTransform = transformGroup;
        }

        public static void Reset(UIElement element)
        {
            if (element == null)
                return;

            // reset zoom
            ScaleTransform scaleTransform = GetScaleTransform(element);
            scaleTransform.ScaleX = 1.0;
            scaleTransform.ScaleY = 1.0;

            // reset pan
            TranslateTransform translateTransform = GetTranslateTransform(element);
            translateTransform.X = 0;
            translateTransform.Y = 0;
        }

        public static void AnimationReset(UIElement element, TimeSpan animationsSpeed)
        {
            if (element == null)
                return;

            // reset zoom
            ScaleTransform scaleTransform = GetScaleTransform(element);
            DoubleAnimation scaleDoubleAnimation = new DoubleAnimation(1, animationsSpeed);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleDoubleAnimation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleDoubleAnimation);

            //reset pan
            TranslateTransform translateTransform = GetTranslateTransform(element);
            DoubleAnimation translatDoubleAnimation = new DoubleAnimation(0, animationsSpeed);
            translateTransform.BeginAnimation(TranslateTransform.XProperty, translatDoubleAnimation);
            translateTransform.BeginAnimation(TranslateTransform.YProperty, translatDoubleAnimation);
        }

        public static TranslateTransform GetTranslateTransform(UIElement element)
        {
            return GetChildrenTransform<TranslateTransform>(element);
        }

        private static ScaleTransform GetScaleTransform(UIElement element)
        {
            return GetChildrenTransform<ScaleTransform>(element);
        }

        private static T GetChildrenTransform<T>(UIElement element) where T : Transform
        {
            return (T)((TransformGroup)element.RenderTransform).Children.First(item => item is T);
        }
    }
}