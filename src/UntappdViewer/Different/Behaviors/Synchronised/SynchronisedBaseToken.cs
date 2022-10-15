using System.Collections.Generic;
using System.Linq;

namespace UntappdViewer.Behaviors
{
    abstract public class SynchronisedBaseToken<T>
    {
        protected List<T> Items { get; set; }

        protected SynchronisedBaseToken()
        {
            Items = new List<T>();
        }

        public virtual void Register(T control)
        {
            Items.Add(control);
        }

        public virtual void Unregister(T control)
        {
            Items.Remove(control);
        }

        protected T GetObject(object item)
        {
            return (T) item;
        }

        protected IEnumerable<T> GetOtherItems(T currentItem)
        {
            return Items.Where(item => !item.Equals(currentItem));
        }
    }
}