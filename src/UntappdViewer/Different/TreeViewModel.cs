using Prism.Mvvm;

namespace UntappdViewer
{
    public class TreeItemViewModel : BindableBase
    {
        private bool isSelected;

        public long Id { get; private set; }

        private string Name { get; set; }

        public TreeItemViewModel(long id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}