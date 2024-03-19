using System.Collections.Generic;

namespace UntappdViewer.Interfaces
{
    public interface ICancellationToken<T>: IBaseCancellationToken
    {
        List<T> Items { get; }
    }
}