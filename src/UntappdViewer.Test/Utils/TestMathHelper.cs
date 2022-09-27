using NUnit.Framework;
using UntappdViewer.Utils;

namespace UntappdViewer.Test.Utils
{
    [TestFixture]
    public class TestMathHelper
    {
        [Test]
        public void TestRoundByStep()
        {
            Assert.AreEqual(250, MathHelper.RoundByStep(53, 250));
            Assert.AreEqual(1000, MathHelper.RoundByStep(963, 250));
            Assert.AreEqual(1250, MathHelper.RoundByStep(1001, 250));

            Assert.AreEqual(100, MathHelper.RoundByStep(53, 100));
            Assert.AreEqual(1000, MathHelper.RoundByStep(963, 100));
            Assert.AreEqual(1100, MathHelper.RoundByStep(1001, 100));
        }
    }
}
