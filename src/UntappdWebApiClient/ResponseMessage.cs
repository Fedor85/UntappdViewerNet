using UntappdViewer.Interfaces;

namespace UntappdWebApiClient
{
    public class ResponseMessage(int Code) : IResponseMessage
    {
        public int Code { get; private set; } = Code;

        public string Message { get; set; }
    }
}