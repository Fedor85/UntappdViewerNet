using System.Collections.Generic;
using UntappdViewer.Interfaces;

namespace UntappdWebApiClient
{
    public class CancellationToken<T> : ICancellationToken<T>
    {
        public List<T> Items { get; } = new();

        public bool Cancel { get;  set; }
    }
}