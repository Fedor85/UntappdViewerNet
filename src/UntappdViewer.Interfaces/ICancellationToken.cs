using System.Collections.Generic;

namespace UntappdViewer.Interfaces
{
    public interface ICancellationToken<T>
    {
        List<T> Items { get; }

        bool Cancel { get; set; }
    }
}