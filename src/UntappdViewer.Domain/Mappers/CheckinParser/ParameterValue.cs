using System;

namespace UntappdViewer.Domain.Mappers.CheckinParser
{
    public class ParameterValue
    {
        public string Value { get; set; }

        public int Number { get; set; }

        public ParameterValue(string value, int number)
        {
            Value = value;
            Number = number;
        }

        public override string ToString()
        {
            return String.Format("{0}:{1}", Number, Value);
        }
    }
}