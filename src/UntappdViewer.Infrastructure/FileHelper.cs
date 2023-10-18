using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace UntappdViewer.Infrastructure
{
    public static class FileHelper
    {
        public static readonly string TempDirectory = Path.Combine(Path.GetTempPath(), $"uv_{Guid.NewGuid()}");

        public const int ExifImageDateTimeOriginal = 36867;

        public static FileStatus Check(string filePath, List<string> supportExtensions)
        {
            if (String.IsNullOrEmpty(filePath))
                return FileStatus.IsEmptyPath;

            if (!File.Exists(filePath))
                return FileStatus.NotExists;

            if (!supportExtensions.Contains(GetExtensionWihtoutPoint(filePath)))
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

        public static string GetExtensionWihtoutPoint(string filePath)
        {
            string extension = Path.GetExtension(filePath);
            if (String.IsNullOrEmpty(extension))
                return extension;

            return extension.Replace(".", String.Empty).Trim().ToLower();
        }

        public static void SaveFile(string filePath, object saveObject)
        {
            using (Stream stream = File.Open(filePath, FileMode.OpenOrCreate))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(stream, saveObject);
            }
        }

        public static void SaveStreamToFile(Stream stream, string filePath)
        {
            using (FileStream fileStream = File.Create(filePath))
            {
                byte[] bytesInStream = new byte[stream.Length];
                stream.Read(bytesInStream, 0, bytesInStream.Length);
                fileStream.Write(bytesInStream, 0, bytesInStream.Length);
            }
        }

        public static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        public static T OpenFile<T>(string filePath)
        {
            using (Stream stream = File.Open(filePath, FileMode.Open))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                return (T)binaryFormatter.Deserialize(stream);
            }
        }

        public static string AddFilePath(string allFilePaths, string filePath, int maxItems)
        {
            List<FileItem> fileItems = GetParseFilePaths(allFilePaths);
            AddFile(fileItems, filePath, maxItems);
            return GetMergedFilePaths(fileItems);
        }

        public static void AddFile(List<FileItem> fileItems, string filePath, int maxItems)
        {
            FileItem fileItem = fileItems.Find(item => item.FilePath.Equals(filePath));
            if (fileItem != null)
                fileItem.Index = -1;
            else
                fileItems.Insert(0, new FileItem(-1, filePath));

            UpdateFileItemIndex(fileItems);
            if (fileItems.Count > maxItems)
                fileItems.RemoveAt(fileItems.Count -1);
        }

        public static List<FileItem> GetExistsParseFilePaths(string filePaths)
        {
            List<FileItem> fileItems = GetParseFilePaths(filePaths).Where(item => File.Exists(item.FilePath)).ToList();
            UpdateFileItemIndex(fileItems);
            return fileItems;
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
                allPaths += $"{fileItem.Index}*{fileItem.FilePath}|";

            return allPaths.Remove(allPaths.Length - 1, 1);
        }

        public static string GetFirstFileItemPath(string fileItemPaths)
        {
            List<FileItem> fileItems = GetParseFilePaths(fileItemPaths);

            if (fileItems.Count == 0)
                return String.Empty;

            fileItems.Sort();
            return fileItems[0].FilePath;
        }

        public static void SetProperty(PropertyItem propertyItem, int id, string stringValue)
        {
            int lenght = stringValue.Length + 1;

            byte[] byteValue = new Byte[lenght];
            for (int i = 0; i < lenght - 1; i++)
                byteValue[i] = (byte)stringValue[i];

            byteValue[lenght - 1] = 0x00;

            propertyItem.Id = id;
            propertyItem.Type = 2;
            propertyItem.Value = byteValue;
            propertyItem.Len = lenght;
        }

        public static string GetTempFilePathByPath(string path, string prefix = null)
        {
            CreateDirectory(TempDirectory);
            string fileName = Path.GetFileName(path);
            return Path.Combine(TempDirectory, prefix ?? String.Empty, fileName);
        }

        public static void DeleteTempDirectory()
        {
            if (Directory.Exists(TempDirectory))
                Directory.Delete(TempDirectory, true);
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