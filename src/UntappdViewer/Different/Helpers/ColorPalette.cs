using System.Windows.Media;
using UntappdViewer.Interfaces;

namespace UntappdViewer.Helpers
{
    public class ColorPalette: IColorPalette
    {
        public IGradientHelper GradientHelper { get; private set; }

        public Color MainColorLight { get; private set; }

        public Color MainColorDark { get; private set; }

        public System.Drawing.Color ConvertColor(Color drawColor)
        {
            return System.Drawing.Color.FromArgb(drawColor.A, drawColor.R, drawColor.G, drawColor.B);
        }

        public static ColorPalette GetMainColorPalette()
        {
            ColorPalette colorPalette = new ColorPalette();

            colorPalette.MainColorLight = Infrastructure.DefaultValues.MainColorLight;
            colorPalette.MainColorDark = Infrastructure.DefaultValues.MainColorDark;
            colorPalette.GradientHelper = new GradientHelper(Infrastructure.DefaultValues.MainGradient3);

            return colorPalette;
        }
    }
}