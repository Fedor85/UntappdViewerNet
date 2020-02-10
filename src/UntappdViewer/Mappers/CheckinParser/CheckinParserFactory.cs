using System.Collections.Generic;

namespace UntappdViewer.Mappers.CheckinParser
{
    public class CheckinParserFactory
    {
        private List<ParameterNumber> parameterNumbers;

        public CheckinParserFactory(string parametersNameLine)
        {
            parameterNumbers = GetParameterNumbers(parametersNameLine);
        }

        public CheckinParser GetCheckinParser(string[] parametersValue)
        {
            return new CheckinParser(parameterNumbers, GetParameterValues(parametersValue));
        }

        private List<ParameterNumber> GetParameterNumbers(string parametersNameLine)
        {
            List<ParameterNumber> parameterNumbers = new List<ParameterNumber>();
            int counter = 1;
            foreach (string parameterName in parametersNameLine.Split(','))
                parameterNumbers.Add(new ParameterNumber(parameterName.Trim().ToLower(), counter++));

            return parameterNumbers;
        }

        private List<ParameterValue> GetParameterValues(string[] parametersValue)
        {
            List<ParameterValue> parameterValues = new List<ParameterValue>();
            int counter = 1;
            foreach (string parameterValue in parametersValue)
                parameterValues.Add(new ParameterValue(parameterValue, counter++));

            return parameterValues;
        }
    }
}
