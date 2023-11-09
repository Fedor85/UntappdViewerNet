using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UntappdViewer.Models
{
    [Serializable]
    public abstract class BasePropertyChanged: INotifyPropertyChanged
    {
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
                return false;

            OnPropertyChanged(propertyName);
            storage = value;
            return true;
        }
    }
}