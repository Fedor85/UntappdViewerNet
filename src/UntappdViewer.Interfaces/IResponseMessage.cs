namespace UntappdViewer.Interfaces
{
    public interface IResponseMessage
    {
        public int Code { get; }

        public string Message { get; set; }
    }
}