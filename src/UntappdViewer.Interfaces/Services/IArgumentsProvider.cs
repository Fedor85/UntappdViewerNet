using System.Collections.Generic;

namespace UntappdViewer.Interfaces.Services
{
    public interface IArgumentsProvider
    {
        List<string> Arguments { get; set; }
    }
}