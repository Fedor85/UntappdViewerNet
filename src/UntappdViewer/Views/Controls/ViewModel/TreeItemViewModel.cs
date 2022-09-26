using Prism.Mvvm;

namespace UntappdViewer.Views.Controls.ViewModel
{
    public class TreeItemViewModel : BindableBase
    {
        private bool visibility;

        public long Id { get; }

        public string Name { get; }

        public bool Visibility
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
            Visibility = true;
        }

        public void Hide()
        {
            Visibility = false;
        }

        public void Visible()
        {
            Visibility = true;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}