using System;

namespace UntappdViewer.Domain.Mappers.CheckinParser
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
            return $"{Number}:{Name}";
        }
    }
}