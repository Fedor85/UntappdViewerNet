using System.Threading.Tasks;

namespace UntappdViewer.Interfaces
{
    public interface IUntappdWindowsServiceClient
    {
        Task SetTempFilesByProcessesIdAsync(int processeId, string tempFilesPath);
    }
}