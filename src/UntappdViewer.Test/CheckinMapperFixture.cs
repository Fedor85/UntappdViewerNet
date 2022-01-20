using System.IO;
using System.Reflection;
using NUnit.Framework;
using UntappdViewer.Domain.Mappers;
using UntappdViewer.Models;
using UntappdViewer.Test.Properties;

namespace UntappdViewer.Test
{
    [TestFixture]
    public class CheckinMapperFixture
    {
        [Test]
        public void CheckinTextMapperTest()
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Resources.ResourcesTestFileName))
            {
                CheckinsContainer checkinsContainer = CheckinCSVMapper.GetCheckinsContainer(stream);
                Assert.IsTrue(checkinsContainer.Checkins.Count > 0);
            }
        }
    }
}