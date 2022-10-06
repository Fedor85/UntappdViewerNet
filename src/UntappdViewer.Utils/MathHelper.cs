using System;
using System.Collections.Generic;
using System.Linq;

namespace UntappdViewer.Utils
{
    public static class MathHelper
    {
        public static bool DoubleCompare(double value1, double value2)
        {
            return Math.Abs(Math.Abs(value1) - Math.Abs(value2)) < 0.0001;
        }

        public static int GetCeilingByStep(int value, int step)
        {
            if (step == 0)
                return value;

            if (value == 0)
                return step;

            double delta = value / (double)step;
            double ceilingValue = Math.Ceiling(delta);
            return Convert.ToInt32(ceilingValue * step);
        }

        public static double GetRoundByStep(double value, double step)
        {
            if (DoubleCompare(step, 0))
                return value;

            double delta = value / step;
            double ceilingValue = Math.Round(delta);
            return ceilingValue * step;
        }

        public static double GetAverageValue(Dictionary<double, int> dictionary)
        {
            int counter = 0;
            double totalValue = 0;
            foreach (KeyValuePair<double, int> keyValuePair in dictionary)
            {
                counter += keyValuePair.Value;
                totalValue += keyValuePair.Key * keyValuePair.Value;
            }

            return counter == 0 ? totalValue : totalValue / counter;
        }

        public static int GetTotalDaysByNow(List<DateTime> dates)
        {
            return !dates.Any() ? 0 : Convert.ToInt32(Math.Ceiling((DateTime.Now - dates.Min()).TotalDays));
        }

        public static double GetAverageCountByNow(List<DateTime> dates)
        {
            int totalDays = GetTotalDaysByNow(dates);
            int count = dates.Count;
            return totalDays == 0 ? count : count / Convert.ToDouble(totalDays);
        }
    }
}