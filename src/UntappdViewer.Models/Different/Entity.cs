namespace UntappdViewer.Models.Different
{
    public class Entity: KeyValue<string, object>
    {
        public int Id { get; set; }

        protected Entity() { }

        public Entity(string key, object value) : base(key, value)
        {
        }
    }
}