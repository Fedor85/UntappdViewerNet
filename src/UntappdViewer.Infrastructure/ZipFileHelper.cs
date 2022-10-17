using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace UntappdViewer.Infrastructure
{
    public class ZipFileHelper
    {
        private List<FileItem> fileItems;

        private List<FileItem> directoryItems;

        public event Action<string> ZipProgress;

        public CancellationTokenSource CancellationTokenSource { get; private set; }

        public ZipFileHelper()
        {
            fileItems = new List<FileItem>();
            directoryItems = new List<FileItem>();
            CancellationTokenSource = new CancellationTokenSource();
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
            if (CancellationTokenSource.IsCancellationRequested)
                e.Cancel = true;

            if (e.CurrentEntry == null || e.EntriesTotal == 0 || e.EntriesSaved == 0 || e.EventType != ZipProgressEventType.Saving_AfterWriteEntry)
                return;

            int percent = (int)Math.Truncate((double)e.EntriesSaved / e.EntriesTotal * 100);
            string message = $"[{percent}% ({e.EntriesSaved}/{e.EntriesTotal})]:{e.CurrentEntry.FileName}";
            ZipProgressInvoke(message);
        }

        private void ZipProgressInvoke(string message)
        {
            ZipProgress?.Invoke(message);
        }

        private string GetTempFilePath(object sender)
        {
            FieldInfo fieldInfo = sender.GetType().GetField("_temporaryFileName", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fieldInfo == null)
                return String.Empty;

            object tempFilePath = fieldInfo.GetValue(sender);
            return tempFilePath?.ToString() ?? String.Empty;
        }

        public static string GetResultPath(string filePath)
        {
            return Path.Combine(Path.GetDirectoryName(filePath), $"{Path.GetFileNameWithoutExtension(filePath)}.zip");
        }
    }
}