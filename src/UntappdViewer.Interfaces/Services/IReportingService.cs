using System.Collections.Generic;
using System.Threading.Tasks;
using UntappdViewer.Models;

namespace UntappdViewer.Interfaces.Services
{
    public interface IReportingService
    {
        void SetColorPalette(IColorPalette colorPalette);

        Task<string> CreateAllCheckinsReportrAsync(List<Checkin> checkins, string directory);

        Task<string> CreateStatisticsReportAsync(IStatisticsCalculation statisticsCalculation, string directory);

        string CreateAllCheckinsReport(List<Checkin> checkins, string directory);

        string CreateStatisticsReport(IStatisticsCalculation statisticsCalculation, string directory);
    }
}