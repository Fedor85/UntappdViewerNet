using System.Collections.Generic;
using System.Threading.Tasks;
using UntappdViewer.Models;

namespace UntappdViewer.Interfaces.Services
{
    public interface IReportingService
    {
         Task<string> CreateAllCheckinsReportrAsync(List<Checkin> checkins, string directory, string fileName);
        /// <returns></returns>
        string CreateAllCheckinsReport(List<Checkin> checkins, string directory, string fileName);
    }
}