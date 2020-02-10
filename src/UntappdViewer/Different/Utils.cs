using System.Collections.Generic;
using System.Text;
using UntappdViewer.Mappers.CheckinParser;

namespace UntappdViewer
{
    public static class Utils
    {
        public static string JoinParameterNumber(List<ParameterNumber> parameterNumbers)
        {
            StringBuilder text = new StringBuilder();
            foreach (ParameterNumber parameterNumber in parameterNumbers)
            {
                text.Append(parameterNumber);
                text.Append(";");
            }
            return text.ToString();
        }

        public static string JoinParameterValue(List<ParameterValue> parameterValues)
        {
            StringBuilder text = new StringBuilder();
            foreach (ParameterValue parameterValue in parameterValues)
            {
                text.Append(parameterValue);
                text.Append(";");
            }
            return text.ToString();
        }
    }
}