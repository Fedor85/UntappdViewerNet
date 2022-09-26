namespace UntappdViewer.Views.Controls.ViewModel
{
    public class Entity
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public Entity(long id, string name)
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