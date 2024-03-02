using NUnit.Framework;
using NUnit.Framework.Legacy;
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
            ClassicAssert.IsTrue(checkinsContainer.Checkins.Count > 0);
        }
    }
}