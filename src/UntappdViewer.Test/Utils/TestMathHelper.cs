using NUnit.Framework;
using UntappdViewer.Utils;

namespace UntappdViewer.Test.Utils
{
    [TestFixture]
    public class TestMathHelper
    {
        [Test]
        public void TestDoubleCompare()
        {
            Assert.True(MathHelper.DoubleCompare(234.234, 234.234));
            Assert.True(MathHelper.DoubleCompare(234, 234));
            Assert.False(MathHelper.DoubleCompare(234.234, 234.233));
            Assert.False(MathHelper.DoubleCompare(1, 1.2));
            Assert.False(MathHelper.DoubleCompare(1, 1.002));
        }

        [Test]
        public void TestCeilingByStep()
        {
            Assert.AreEqual(250, MathHelper.GetCeilingByStep(53, 250));
            Assert.AreEqual(1000, MathHelper.GetCeilingByStep(963, 250));
            Assert.AreEqual(1250, MathHelper.GetCeilingByStep(1001, 250));

            Assert.AreEqual(100, MathHelper.GetCeilingByStep(53, 100));
            Assert.AreEqual(1000, MathHelper.GetCeilingByStep(963, 100));
            Assert.AreEqual(1100, MathHelper.GetCeilingByStep(1001, 100));
        }


        [Test]
        public void TestRoundByStep()
        {
            Assert.AreEqual(0, MathHelper.GetRoundByStep(0.01, 0.25));
            Assert.AreEqual(0, MathHelper.GetRoundByStep(0, 0.25));
            Assert.AreEqual(0.75, MathHelper.GetRoundByStep(0.75, 0.25));

            Assert.AreEqual(1.25, MathHelper.GetRoundByStep(1.345, 0.25));
            Assert.AreEqual(3.25, MathHelper.GetRoundByStep(3.234, 0.25));
            Assert.AreEqual(4.25, MathHelper.GetRoundByStep(4.256, 0.25));
        }
    }
}
