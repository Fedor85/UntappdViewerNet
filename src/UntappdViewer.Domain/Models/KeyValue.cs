namespace UntappdViewer.Domain.Models
{
    public class KeyValue<T1,T2>
    {
        /// <summary>
        /// X
        /// </summary>
        public T1 Key { get; set; }

        /// <summary>
        /// Y
        /// </summary>
        public T2 Value { get; set; }

        public KeyValue(T1 key, T2 value)
        {
            Key = key;
            Value = value;
        }

        public override string ToString()
        {
            return $"{Key}:{Value}";
        }
    }
}