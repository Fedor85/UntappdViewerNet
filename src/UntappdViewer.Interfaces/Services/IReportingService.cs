using System.Collections.Generic;
using System.Threading.Tasks;
using UntappdViewer.Models;

namespace UntappdViewer.Interfaces.Services
{
    public interface IReportingService
    {
         Task<string> CreateAllCheckinsReportrAsync(List<Checkin> checkins, string directory);

        string CreateAllCheckinsReport(List<Checkin> checkins, string directory);

        string CreateStatisticsReport(Untappd untappd, string directory);
    }
}