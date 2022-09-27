namespace UntappdViewer.Views.Controls.ViewModel
{
    public class ChartViewModel<T1,T2>
    {
        /// <summary>
        /// X
        /// </summary>
        public T1 Key { get; set; }

        /// <summary>
        /// Y
        /// </summary>
        public T2 Value { get; set; }

        public ChartViewModel(T1 key, T2 value)
        {
            Key = key;
            Value = value;
        }
    }
}