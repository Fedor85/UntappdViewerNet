﻿using System;
using System.IO;

namespace UntappdViewer.Infrastructure
{
    public static class FileHelper
    {
        public static string GetTextForFile(string filePath)
        {
            return File.ReadAllText(filePath);
        }

        public static string GetExtensionWihtoutPoint(string filePath)
        {
            string extension = Path.GetExtension(filePath);
            if (String.IsNullOrEmpty(extension))
                return extension;

            return extension.Replace(".", String.Empty).Trim().ToLower();
        }

        public static bool FileExists(string filePat)
        {
            return File.Exists(filePat);
        }
    }
}