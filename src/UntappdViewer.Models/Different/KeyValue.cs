namespace UntappdViewer.Models.Different
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

        protected KeyValue()
        {
        }

        public KeyValue(T1 key, T2 value)
        {
            Key = key;
            Value = value;
        }

        public override bool Equals(object obj)
        {
            KeyValue<T1, T2> keyValue = obj as KeyValue<T1, T2>;
            if (keyValue == null)
                return false;

            return Key.Equals(keyValue.Key) && Value.Equals(keyValue.Value);
        }

        public override string ToString()
        {
            return $"{Key}:{Value}";
        }
    }
}