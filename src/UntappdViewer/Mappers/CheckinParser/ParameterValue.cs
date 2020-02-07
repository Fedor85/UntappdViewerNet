namespace UntappdViewer.Mappers.CheckinParser
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
    }
}