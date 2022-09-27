using System;

namespace UntappdViewer.Utils
{
    public static class MathHelper
    {
        public static bool DoubleCompare(double value1, double value2)
        {
            return Math.Abs(value1) - Math.Abs(value2) < 0.0001;
        }

        public static int RoundByStep(int value, int step)
        {
            if (step == 0)
                return value;

            double delta = value / (double)step;
            double ceilingValue = Math.Ceiling(delta);
            return Convert.ToInt32(ceilingValue * step);
        }
    }
}