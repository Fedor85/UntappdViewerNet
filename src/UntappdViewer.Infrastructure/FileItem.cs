using System;
using System.IO;

namespace UntappdViewer.Infrastructure
{
    public class FileItem: IComparable<FileItem>
    {
        public string FileName { get; }

        public string FilePath { get; }

        public int Index { get; set; }

        public FileItem(int index, string filePath)
        {
            FilePath = filePath;
            FileName = Path.GetFileName(filePath);
            Index = index;
        }

        public int CompareTo(FileItem other)
        {
            if (Index < other.Index)
                return -1;

            if (Index > other.Index)
                return 1;

            return 0;
        }
    }
}