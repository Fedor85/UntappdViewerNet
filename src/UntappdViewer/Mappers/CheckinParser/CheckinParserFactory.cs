using System.Collections.Generic;

namespace UntappdViewer.Mappers.CheckinParser
{
    public class CheckinParserFactory
    {
        private List<ParameterNumber> parameterNumbers;

        public CheckinParserFactory(string parametersLine)
        {
            parameterNumbers = GetParameterNumbers(parametersLine);
        }

        public CheckinParser GetCheckinParser(string parametersValueLine)
        {
            return new CheckinParser(parameterNumbers, GetParameterValues(parametersValueLine));
        }

        private List<ParameterNumber> GetParameterNumbers(string parametersNameLine)
        {
            List<ParameterNumber> parameterNumbers = new List<ParameterNumber>();
            int counter = 1;
            foreach (string parameterName in parametersNameLine.Split(','))
                parameterNumbers.Add(new ParameterNumber(parameterName.Trim().ToLower(), counter++));

            return parameterNumbers;
        }

        private List<ParameterValue> GetParameterValues(string parametersValueLine)
        {
            List<ParameterValue> parameterValues = new List<ParameterValue>();
            int counter = 1;
            foreach (string parameterValue in parametersValueLine.Split(','))
                parameterValues.Add(new ParameterValue(parameterValue, counter++));

            return parameterValues;
        }
    }
}
