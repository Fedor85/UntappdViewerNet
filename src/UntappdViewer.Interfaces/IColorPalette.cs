using System.Windows.Media;

namespace UntappdViewer.Interfaces
{
    public interface IColorPalette
    {
        IGradientHelper GradientHelper { get; }

        Color MainColorLight { get; }

        Color MainColorDark { get; }

        System.Drawing.Color ConvertColor(Color drawColor);
    }
}