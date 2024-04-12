using System.Threading.Tasks;
using UntappdViewer.Interfaces;

namespace UntappdViewer.Infrastructure.Services
{
    public class UntappdWindowsServiceClient: IUntappdWindowsServiceClient
    {
        private UntappdWindowsService.Extension.Interfaces.IUntappdWindowsServiceClient client =
                                                new UntappdWindowsService.Client.UntappdWindowsServiceClient();

        public Task SetTempFilesByProcessesIdAsync(int processeId, string tempFilesPath)
        {
            return client.SetTempFilesByProcessesIdAsync(processeId, tempFilesPath);
        }
    }
}