using System.ComponentModel;

namespace UntappdViewer.UI.Controls.ViewModel
{
    public class TreeItemViewModel : INotifyPropertyChanged
    {
        private bool visibility;

        public event PropertyChangedEventHandler PropertyChanged;

        public long Id { get; }

        public string Name { get; }

        public bool Visibility
        {
            get { return visibility; }
            set
            {
                visibility = value;
                OnPropertyChanged("Visibility");
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

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}