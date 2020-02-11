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
    }
}