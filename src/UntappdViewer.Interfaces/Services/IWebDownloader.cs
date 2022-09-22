namespace UntappdViewer.Interfaces.Services
{
    public interface IWebDownloader
    {
        bool DownloadFile(string urlFile, string filePath);
    }
}