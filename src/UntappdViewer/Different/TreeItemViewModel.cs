using System.Windows;
using Prism.Mvvm;

namespace UntappdViewer
{
    public class TreeItemViewModel : BindableBase
    {
        private Visibility visibility;

        public long Id { get; private set; }

        public string Name { get; set; }

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

        public override string ToString()
        {
            return Name;
        }
    }
}