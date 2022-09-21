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

        public async void SaveAsZipAsync(string resultPath)
        {
            await Task.Run(() => SaveAsZip(resultPath));
        }

        public void SaveAsZip(string resultPath)
        {
            using (ZipFile zip = new ZipFile())
            {
                foreach (FileItem fileItem in fileItems)
                    zip.AddFile(fileItem.FilePath, String.Empty);

                foreach (FileItem directoryItem in directoryItems)
                    zip.AddDirectory(directoryItem.FilePath, directoryItem.FileName);

                zip.Save(resultPath);
            }
        }

        public static string GetResultPath(string filePath)
        {
            return Path.Combine(Path.GetDirectoryName(filePath), String.Format("{0}.zip", Path.GetFileNameWithoutExtension(filePath)));
        }
    }
}