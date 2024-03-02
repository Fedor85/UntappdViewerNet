using System.Diagnostics;

namespace UntappdViewer.Infrastructure
{
    public static class ProcessStartHelper
    {
        public static void ProcessStart(string startTarget)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo(startTarget);
            processStartInfo.UseShellExecute = true;
            Process.Start(processStartInfo);
        }
    }
}