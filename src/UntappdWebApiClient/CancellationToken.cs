using System.Collections.Generic;
using UntappdViewer.Interfaces;

namespace UntappdWebApiClient
{
    public class CancellationToken<T> : ICancellationToken<T>
    {
        public List<T> Items { get;  private set; }

        public bool Cancel { get;  set; }

        public CancellationToken()
        {
            Items = new List<T>();
        }
    }
}