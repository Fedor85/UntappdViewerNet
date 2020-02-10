using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using UntappdViewer.Mappers;
using UntappdViewer.Models;

namespace UntappdViewer.Test
{
    [TestFixture]
    public class CheckinMapperFixture
    {
        [Test]
        public void CheckinTextMapperTest()
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("UntappdViewer.Test.78a7bb4694ba2d4c33ba2dc76d0fc60d.csv"))
            {
                List<Checkin> checkins = CheckinCSVMapper.GetCheckins(stream);
                Assert.IsTrue(checkins.Count > 0);
            }

        }
    }
}