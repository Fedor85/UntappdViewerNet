using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace UntappdViewer.Infrastructure
{
    public class ZipFileHelper
    {
        private List<FileItem> fileItems;

        private List<FileItem> directoryItems;

        public event Action<string> ZipProgress;

        public ZipFileHelper()
        {
            fileItems = new List<FileItem>();
            directoryItems = new List<FileItem>();
        }

        public void AddFile(string filePath)
        {
            int index = fileItems.Count == 0 ? 0 : fileItems.Max(item => item.Index) + 1;
            fileItems.Add(new FileItem(index, filePath));
        }
        public void AddDirectory(string directoryPath)
        {
            int index = directoryItems.Count == 0 ? 0 : directoryItems.Max(item => item.Index) + 1;
            directoryItems.Add(new FileItem(index, directoryPath));
        }

        public void SaveAsZip(string resultPath)
        {
            using (ZipFile zip = new ZipFile())
            {
                zip.SaveProgress += ZipSaveProgress;
                foreach (FileItem fileItem in fileItems)
                    zip.AddFile(fileItem.FilePath, String.Empty);

                foreach (FileItem directoryItem in directoryItems)
                    zip.AddDirectory(directoryItem.FilePath, directoryItem.FileName);

                zip.Save(resultPath);
            }
        }

        private void ZipSaveProgress(object sender, SaveProgressEventArgs e)
        {
            if (e.CurrentEntry != null)
                ZipProgressInvoke(e.CurrentEntry.ToString());
        }

        private void ZipProgressInvoke(string message)
        {
            if (ZipProgress != null)
                ZipProgress.Invoke(message);
        }

        public static string GetResultPath(string filePath)
        {
            return Path.Combine(Path.GetDirectoryName(filePath), String.Format("{0}.zip", Path.GetFileNameWithoutExtension(filePath)));
        }
    }
}