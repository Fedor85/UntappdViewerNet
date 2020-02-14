using System;
using System.Collections.Generic;
using System.IO;

namespace UntappdViewer.Infrastructure
{
    public static class FileHelper
    {
        public static string GetExtensionWihtoutPoint(string filePath)
        {
            string extension = Path.GetExtension(filePath);
            if (String.IsNullOrEmpty(extension))
                return extension;

            return extension.Replace(".", String.Empty).Trim().ToLower();
        }

        public static FileStatus Check(string filePath, List<string> supportExtensions)
        {
            if (String.IsNullOrEmpty(filePath))
                return FileStatus.IsEmptyPath;

            if (!File.Exists(filePath))
                return FileStatus.NotExists;

            if (!supportExtensions.Contains(FileHelper.GetExtensionWihtoutPoint(filePath)))
                return FileStatus.NoSupported;

            try
            {
                using (FileStream stream = File.OpenRead(filePath)) { }

            }
            catch (IOException ex)
            {
                if (ex.HResult == -2147024864)
                    return FileStatus.IsLocked;
            }
            return FileStatus.Available;
        }

        public static string AddFilePath(string allFilePaths, string filePath, int maxitems)
        {
            List<FileItem> fileItems = GetParseFilePaths(allFilePaths);
            AddFile(fileItems, filePath, maxitems);
            return GetMergedFilePaths(fileItems);
        }

        public static void AddFile(List<FileItem> fileItems, string filePath, int maxitems)
        {
            FileItem fileItem = fileItems.Find(item => item.FilePath.Equals(filePath));
            if (fileItem != null)
                fileItem.Index = -1;
            else
                fileItems.Insert(0, new FileItem(-1, filePath));

            UpdateFileItemIndex(fileItems);
            if (fileItems.Count > maxitems)
                fileItems.RemoveAt(fileItems.Count -1);
        }

        public static List<FileItem> GetParseFilePaths(string filePaths)
        {
            List<FileItem> fileItems = new List<FileItem>();
            if (!String.IsNullOrEmpty(filePaths))
            {
                foreach (string filePath in filePaths.Split('|'))
                {
                    string[] items = filePath.Trim().Split('*');
                    fileItems.Add(new FileItem(Convert.ToInt32(items[0]), items[1]));
                }
            }
            return fileItems;
        }

        public static string GetMergedFilePaths(List<FileItem> fileItems)
        {
            if (fileItems.Count == 0)
                return String.Empty;

            string allPaths = String.Empty;
            foreach (FileItem fileItem in fileItems)
                allPaths += String.Format("{0}*{1}|", fileItem.Index, fileItem.FilePath);

            return allPaths.Remove(allPaths.Length - 1, 1); ;
        }

        public static string GetFirstFileItemPath(string fileItemPaths)
        {
            List<FileItem> fileItems = GetParseFilePaths(fileItemPaths);

            if (fileItems.Count == 0)
                return String.Empty;

            fileItems.Sort();
            return fileItems[0].FilePath;
        }

        private static void UpdateFileItemIndex(List<FileItem> fileItems)
        {
            fileItems.Sort();
            int counter = 1;
            foreach (FileItem fileItem in fileItems)
                fileItem.Index = counter++;
        }
    }
}