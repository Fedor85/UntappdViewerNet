using System;
using System.Net;
using UntappdViewer.Interfaces.Services;

namespace UntappdViewer.Infrastructure.Services
{
    public class WebDownloader: IWebDownloader
    {
        public bool DownloadFile(string urlFile, string filePath)
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    client.DownloadFile(urlFile, filePath);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
    }
}