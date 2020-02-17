using UntappdViewer.Infrastructure;

namespace UntappdViewer.Helpers
{
    public static class EnumsHelper
    {
        public static bool IsValidFileStatus(FileStatus fileStatus)
        {
            return fileStatus == FileStatus.Available;
        }
    }
}