using System;

namespace UntappdViewer.Utils
{
    public static class MathHelper
    {
        public static bool Doublecompare(double value1, double value2)
        {
            return Math.Abs(value1) - Math.Abs(value2) < 0.0001;
        }
    }
}
