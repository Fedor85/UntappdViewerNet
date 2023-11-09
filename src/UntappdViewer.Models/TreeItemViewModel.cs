using System;

namespace UntappdViewer.Models
{
    public class TreeItemViewModel : BasePropertyChanged
    {
        private bool visibility;

        public long Id { get; }

        public string Name { get; }


        public string NameToLower { get; }

        public bool IsUniqueCheckin { get; set; }

        public bool Visibility
        {
            get { return visibility; }
            private set { SetProperty(ref visibility, value); }
        }

        public TreeItemViewModel(long id, string name)
        {
            Id = id;
            Name = name;
            NameToLower = Name.Trim().ToLower();
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