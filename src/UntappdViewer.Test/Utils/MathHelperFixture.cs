using System;
using System.Collections.Generic;
using NUnit.Framework;
using UntappdViewer.Utils;

namespace UntappdViewer.Test.Utils
{
    [TestFixture]
    public class MathHelperFixture
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


        [Test]
        public void TestAverageValue()
        {
            Dictionary<double, int> dictionary = new Dictionary<double, int>();

            dictionary.Add(0, 15);
            dictionary.Add(0.25, 78);
            dictionary.Add(0.5, 3);
            dictionary.Add(0.75, 123);

            dictionary.Add(1, 76);
            dictionary.Add(1.25, 9);
            dictionary.Add(1.5, 1);
            dictionary.Add(1.75, 456);

            Assert.AreEqual(1.31, Math.Round(MathHelper.GetAverageValue(dictionary), 2));
        }

        [Test]
        public void TestGetPercentageOf()
        {
            Assert.AreEqual(0, MathHelper.GetPercentageOf(0, 10));
            Assert.AreEqual(0, MathHelper.GetPercentageOf(10, 0));
            Assert.AreEqual(4, MathHelper.GetPercentageOf(100, 4));
            Assert.AreEqual(2.68, MathHelper.GetPercentageOf(67, 4));
            Assert.AreEqual(458, MathHelper.GetPercentageOf(458, 100));
            Assert.AreEqual(132, MathHelper.GetPercentageOf(120, 110));
        }
    }
}
