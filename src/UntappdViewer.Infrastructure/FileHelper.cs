using System;
using System.IO;

namespace UntappdViewer.Infrastructure
{
    public static class FileHelper
    {
        public static string GetTextForFile(string filePath)
        {
            return File.ReadAllText(filePath);
        }

        public static string GetExtensionWihtoutPoint(string path)
        {
            string extension = Path.GetExtension(path);
            if (String.IsNullOrEmpty(extension))
                return extension;

            return extension.Replace(".", String.Empty).Trim().ToLower();
        }
    }
}