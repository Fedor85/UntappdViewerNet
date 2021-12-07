using System.Collections.Generic;
using UntappdViewer.Models;

namespace UntappdViewer.Interfaces.Services
{
    public interface IReportingService
    {
        void CreateAllCheckinsReport(List<Checkin> checkins, string outputPath);
    }
}