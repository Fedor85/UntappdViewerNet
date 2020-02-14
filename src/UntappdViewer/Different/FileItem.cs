using System.IO;

namespace UntappdViewer
{
    public class FileItem
    {
        public string FileName { get; }

        public string FilePath { get; }

        public FileItem(string filePath)
        {
            FilePath = filePath;
            FileName = Path.GetFileName(filePath);
        }
    }
}