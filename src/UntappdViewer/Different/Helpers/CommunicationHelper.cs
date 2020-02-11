using UntappdViewer.Infrastructure;

namespace UntappdViewer
{
    public static class CommunicationHelper
    {
        public static string GetFileStatusMessage(FileStatus fileStatus, string filePath)
        {
            string statusMessage;
            switch (fileStatus)
            {
                case FileStatus.IsEmptyPath:
                    statusMessage = Properties.Resources.IsEmptyFilePath;
                    break;
                case FileStatus.NotExists:
                    statusMessage = Properties.Resources.NotExistsFile;
                    break;
                case FileStatus.IsLocked:
                    statusMessage = Properties.Resources.IsLockedFile;
                    break;
                case FileStatus.NoSupported:
                    statusMessage = Properties.Resources.NoSupportedFile;
                    break;
                default:
                    statusMessage = Properties.Resources.OpenFile;
                    break;
            }
            return $"{statusMessage}:\n{filePath}";
        }
    }
}
