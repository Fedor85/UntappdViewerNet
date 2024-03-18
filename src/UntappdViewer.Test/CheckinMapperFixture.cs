using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework.Legacy;
using UntappdViewer.Models;

namespace UntappdViewer.Test
{
    [TestClass]
    public class CheckinMapperFixture
    {
        [TestMethod]
        public void CheckinTextMapperTest()
        {
            CheckinsContainer checkinsContainer = TestHelper.GetCheckinsContainer();
            ClassicAssert.IsTrue(checkinsContainer.Checkins.Count > 0);
        }
    }
}