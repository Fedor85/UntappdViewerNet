using System.Linq;
using System.Windows.Media;
using UntappdViewer.Interfaces;
using UntappdViewer.Utils;

namespace UntappdViewer.Helpers
{
    public class GradientHelper : IGradientHelper
    {
        private GradientStopCollection gradientStopCollection;

        public GradientHelper(GradientStopCollection gradientStopCollection)
        {
            this.gradientStopCollection = gradientStopCollection;
        }

        public Color GetRelativeColor(int index, int count)
        {
            return GetRelativeColor(GetOffSet(index, count));
        }

        public Color GetRelativeColor(double offset)
        {
            var point = gradientStopCollection.SingleOrDefault(f => f.Offset == offset);
            if (point != null) return point.Color;

            GradientStop before = gradientStopCollection.First(w => MathHelper.DoubleCompare(w.Offset, gradientStopCollection.Min(m => m.Offset)));
            GradientStop after = gradientStopCollection.First(w => MathHelper.DoubleCompare(w.Offset, gradientStopCollection.Max(m => m.Offset)));

            foreach (var gs in gradientStopCollection)
            {
                if (gs.Offset < offset && gs.Offset > before.Offset)
                    before = gs;

                if (gs.Offset > offset && gs.Offset < after.Offset)
                    after = gs;
            }

            var color = new Color();

            color.ScA = (float)((offset - before.Offset) * (after.Color.ScA - before.Color.ScA) / (after.Offset - before.Offset) + before.Color.ScA);
            color.ScR = (float)((offset - before.Offset) * (after.Color.ScR - before.Color.ScR) / (after.Offset - before.Offset) + before.Color.ScR);
            color.ScG = (float)((offset - before.Offset) * (after.Color.ScG - before.Color.ScG) / (after.Offset - before.Offset) + before.Color.ScG);
            color.ScB = (float)((offset - before.Offset) * (after.Color.ScB - before.Color.ScB) / (after.Offset - before.Offset) + before.Color.ScB);

            return color;
        }

        private double GetOffSet(int index, int count)
        {
            if (count == 1)
                return 0;

            double step = 1 / ((double)count - 1);
            return index * step;
        }
    }
}
