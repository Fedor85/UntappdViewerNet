using System.Collections.Generic;
using UntappdViewer.Interfaces.Services;

namespace UntappdViewer.Infrastructure.Services
{
    public class ArgumentsProvider: IArgumentsProvider
    {
        public List<string> Arguments { get; set; }

        public ArgumentsProvider()
        {
            Arguments = new List<string>();
        }
    }
}