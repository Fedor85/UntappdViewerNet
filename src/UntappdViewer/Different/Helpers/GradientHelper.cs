using System.Linq;
using System.Windows.Media;
using System.Windows.Threading;
using UntappdViewer.Interfaces;
using UntappdViewer.Utils;

namespace UntappdViewer.Helpers
{
    public class GradientHelper : IGradientHelper
    {
        private Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
        private GradientStopCollection gradientStopCollection;

        public GradientHelper(GradientStopCollection gradientStopCollection)
        {
            dispatcher = Dispatcher.CurrentDispatcher;
            this.gradientStopCollection = gradientStopCollection;
        }

        public Color GetRelativeColor(int index, int count)
        {
            return GetRelativeColor(GetOffSet(index, count));
        }

        public Color GetColor(int index)
        {
            return dispatcher.Invoke(() => GetColorDispatcher(index));
        }

        public Color GetRelativeColor(double offset)
        {
            return dispatcher.Invoke(() => GetRelativeColorDispatcher(offset));
        }

        private Color GetColorDispatcher(int index)
        {
            if (gradientStopCollection.Count == 0 ||
                index >= gradientStopCollection.Count)
                return Color.FromArgb(0, 0, 0, 0);

            return gradientStopCollection[index].Color;
        }


        private Color GetRelativeColorDispatcher(double offset)
        {
            var point = gradientStopCollection.SingleOrDefault(f => f.Offset == offset);
            if (point != null)
                return point.Color;

            GradientStop before = gradientStopCollection.First(w => MathHelper.DoubleCompare(w.Offset, gradientStopCollection.Min(m => m.Offset)));
            GradientStop after = gradientStopCollection.First(w => MathHelper.DoubleCompare(w.Offset, gradientStopCollection.Max(m => m.Offset)));

            foreach (GradientStop gs in gradientStopCollection)
            {
                if (gs.Offset < offset && gs.Offset > before.Offset)
                    before = gs;

                if (gs.Offset > offset && gs.Offset < after.Offset)
                    after = gs;
            }

            Color color = new Color();

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
