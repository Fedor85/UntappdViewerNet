using System;
using System.Reflection;
using UntappdViewer.Infrastructure;

namespace UntappdViewer.Helpers
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

        public static string GetLoadingMessage(int counter, int count, string objectName)
        {
            return GetLoadingMessage($"{counter++}/{count} -> {objectName}");
        }

        public static string GetLoadingMessage(string text)
        {
            return $"{Properties.Resources.LoadingFrom}: {text}";
        }

        public static string GetTitle()
        {
            return GetTitle(String.Empty);
        }

        public static string GetTitle(string userName)
        {
            return $"{Properties.Resources.AppName} {(String.IsNullOrEmpty(userName) ? String.Empty : userName)} ({Assembly.GetEntryAssembly()?.GetName().Version})";
        }
    }
}
