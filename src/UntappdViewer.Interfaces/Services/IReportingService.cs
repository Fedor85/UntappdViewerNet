using System.Collections.Generic;
using UntappdViewer.Models;

namespace UntappdViewer.Interfaces.Services
{
    public interface IReportingService
    {
        /// <returns></returns>
        string CreateAllCheckinsReport(List<Checkin> checkins, string directory, string fileName);
    }
}