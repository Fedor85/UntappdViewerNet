namespace UntappdViewer.Models.Different
{
    public class KeyValueParam<T1, T2> : KeyValue<T1, T2>
    {
        public ParametersContainer Parameters { get; private set; }

        public KeyValueParam(T1 key, T2 value) : base(key, value)
        {
            Parameters = new ParametersContainer();
        }
    }
}