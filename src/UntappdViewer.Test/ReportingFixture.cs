using System;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UntappdViewer.Domain.Mappers;
using UntappdViewer.Models;
using UntappdViewer.Reporting;
using UntappdViewer.Test.Properties;

namespace UntappdViewer.Test
{
    [TestClass]
    public class ReportingFixture
    {
        [TestMethod]
        public void TestReport()
        {
            ReportingService reportingService = new ReportingService();
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Resources.ResourcesTestFileName))
            {
                CheckinsContainer checkinsContainer = new CheckinsContainer();
                CheckinCSVMapper.InitializeCheckinsContainer(checkinsContainer, stream);
                reportingService.CreateAllCheckinsReport(checkinsContainer.Checkins, String.Empty);
            }
        }
    }
}