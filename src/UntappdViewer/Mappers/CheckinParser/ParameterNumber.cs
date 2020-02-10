using System;

namespace UntappdViewer.Mappers.CheckinParser
{
    public class ParameterNumber
    {
        public string Name { get; set; }

        public int Number { get; set; }

        public ParameterNumber(string name, int number)
        {
            Name = name;
            Number = number;
        }

        public override string ToString()
        {
            return String.Format("{0}:{1}", Number, Name);
        }
    }
}