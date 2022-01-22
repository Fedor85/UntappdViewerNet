using NUnit.Framework;
using UntappdViewer.Models;

namespace UntappdViewer.Test
{
    [TestFixture]
    public class CheckinMapperFixture
    {
        [Test]
        public void CheckinTextMapperTest()
        {
            CheckinsContainer checkinsContainer = TestHelper.GetCheckinsContainer();
            Assert.IsTrue(checkinsContainer.Checkins.Count > 0);
        }
    }
}