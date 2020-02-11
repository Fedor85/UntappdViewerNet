using System.Collections.Generic;
using System.Text;

namespace UntappdViewer
{
    public static class Utils
    {
        public static string Join<T>(List<T> parameters)
        {
            StringBuilder text = new StringBuilder();
            foreach (T parameter in parameters)
            {
                text.Append(parameter);
                text.Append(";");
            }
            return text.ToString();
        }
    }
}