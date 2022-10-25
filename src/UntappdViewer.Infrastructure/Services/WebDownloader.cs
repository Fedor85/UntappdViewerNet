using System;
using System.IO;
using System.Net;
using UntappdViewer.Interfaces.Services;

namespace UntappdViewer.Infrastructure.Services
{
    public class WebDownloader: IWebDownloader
    {
        public bool DownloadToFile(string urlFile, string filePath)
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

        public Stream DownloadToStream(string urlFile)
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    return client.OpenRead(urlFile);
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
    }
}