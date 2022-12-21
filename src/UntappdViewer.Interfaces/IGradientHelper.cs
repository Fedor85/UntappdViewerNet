using System.Windows.Media;

namespace UntappdViewer.Interfaces
{
    public interface IGradientHelper
    {
        Color GetRelativeColor(double min, double max, double current);

        Color GetRelativeColor(int index, int count);

        Color GetRelativeColor(double offset);

        Color GetColor(int index);

        object GetGradientStopCollection();
    }
}