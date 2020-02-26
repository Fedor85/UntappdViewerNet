namespace UntappdViewer.Services.PopupWindowAction
{
    public class ConfirmationResult<T>
    {
        public T Value { get; set; }

        public bool Result { get; set; }

        public ConfirmationResult(T value, bool result)
        {
            Value = value;
            Result = result;
        }
    }
}