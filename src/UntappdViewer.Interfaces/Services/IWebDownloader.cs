using System.IO;

namespace UntappdViewer.Interfaces.Services
{
    public interface IWebDownloader
    {
        bool DownloadToFile(string urlFile, string filePath);

        Stream DownloadToStream(string urlFile);
    }
}