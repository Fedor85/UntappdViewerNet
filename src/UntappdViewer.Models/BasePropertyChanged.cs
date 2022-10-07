using System;

namespace UntappdViewer.Models
{
    [Serializable]
    public abstract class BasePropertyChanged
    {
        public event Action Changed;

        internal void OnPropertyChanged()
        {
            Changed?.Invoke();
        }
    }
}