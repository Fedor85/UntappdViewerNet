using System.Windows.Media;

namespace UntappdViewer.Infrastructure
{
    public class DefaultValues
    {
        //#ffc100
        public static readonly Color MainColorLight = Color.FromRgb(255, 193, 0);

        //#fd9532
        public static readonly Color MainColorDark = Color.FromRgb(253, 149, 50);

        public static readonly GradientStopCollection MainGradient3 = GetMainGradient3();

        private static GradientStopCollection GetMainGradient3()
        {
            GradientStopCollection gradient = new GradientStopCollection();
            gradient.Add(new GradientStop(Color.FromRgb(255, 255, 255), 0));
            gradient.Add(new GradientStop(MainColorLight, 0.5));
            gradient.Add(new GradientStop(MainColorDark, 1));
            return gradient;
        }
    }
}