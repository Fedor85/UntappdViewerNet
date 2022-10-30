using System.Windows.Media;

namespace UntappdViewer.Interfaces
{
    public interface IGradientHelper
    {
        Color GetRelativeColor(int index, int count);

        Color GetRelativeColor(double offset);

        Color GetColor(int index);
    }
}