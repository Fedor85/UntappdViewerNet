﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using UntappdViewer.Domain.Mappers;
using UntappdViewer.Models;
using UntappdViewer.Reporting;
using UntappdViewer.Test.Properties;

namespace UntappdViewer.Test
{
    [TestFixture]
    public class ReportingFixture
    {
        [Test]
        public void TestReport()
        {
            ReportingService reportingService = new ReportingService();
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Resources.ResourcesTestFileName))
            {
                List<Checkin> checkins = CheckinCSVMapper.GetCheckins(stream);
                reportingService.CreateAllCheckinsReport(checkins, @"", "result");
            }
        }
    }
}