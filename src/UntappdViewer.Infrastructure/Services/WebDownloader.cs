using System.Net;
using UntappdViewer.Interfaces.Services;

namespace UntappdViewer.Infrastructure.Services
{
    public class WebDownloader: IWebDownloader
    {
        public void DownloadFile(string urlFile, string filePath)
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(urlFile, filePath);
            }
        }
    }
}