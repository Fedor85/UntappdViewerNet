using System.Windows;
using Prism.Mvvm;

namespace UntappdViewer
{
    public class TreeItemViewModel : BindableBase
    {
        private Visibility visibility;

        public long Id { get; }

        public string Name { get; }

        public Visibility Visibility
        {
            get { return visibility; }
            set
            {
                visibility = value;
                RaisePropertyChanged();
            }
        }

        public TreeItemViewModel(long id, string name)
        {
            Id = id;
            Name = name;
            Visibility = Visibility.Visible;
        }

        public void Hide()
        {
            Visibility = Visibility.Hidden;
        }

        public void Visible()
        {
            Visibility = Visibility.Visible;
        }

        public bool IsHidden()
        {
            return Visibility == Visibility.Collapsed || Visibility == Visibility.Hidden;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}