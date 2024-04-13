using System.Threading.Tasks;

namespace UntappdViewer.Interfaces
{
    public interface IUntappdWindowsServiceClient
    {
        Task SetTempDirectoryByProcessIdAsync(int processId, string tempDirectory);
    }
}