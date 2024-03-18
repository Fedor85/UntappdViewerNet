using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework.Legacy;
using UntappdViewer.Utils;

namespace UntappdViewer.Test.Utils
{
    [TestClass]
    public class MathHelperFixture
    {
        [TestMethod]
        public void TestDoubleCompare()
        {
            ClassicAssert.True(MathHelper.DoubleCompare(234.234, 234.234));
            ClassicAssert.True(MathHelper.DoubleCompare(234, 234));
            ClassicAssert.False(MathHelper.DoubleCompare(234.234, 234.233));
            ClassicAssert.False(MathHelper.DoubleCompare(1, 1.2));
            ClassicAssert.False(MathHelper.DoubleCompare(1, 1.002));
        }

        [TestMethod]
        public void TestCeilingByStep()
        {
            ClassicAssert.AreEqual(250, MathHelper.GetCeilingByStep(53, 250));
            ClassicAssert.AreEqual(1000, MathHelper.GetCeilingByStep(963, 250));
            ClassicAssert.AreEqual(1250, MathHelper.GetCeilingByStep(1001, 250));

            ClassicAssert.AreEqual(100, MathHelper.GetCeilingByStep(53, 100));
            ClassicAssert.AreEqual(1000, MathHelper.GetCeilingByStep(963, 100));
            ClassicAssert.AreEqual(1100, MathHelper.GetCeilingByStep(1001, 100));
        }


        [TestMethod]
        public void TestRoundByStep()
        {
            ClassicAssert.AreEqual(0, MathHelper.GetRoundByStep(0.01, 0.25));
            ClassicAssert.AreEqual(0, MathHelper.GetRoundByStep(0, 0.25));
            ClassicAssert.AreEqual(0.75, MathHelper.GetRoundByStep(0.75, 0.25));

            ClassicAssert.AreEqual(1.25, MathHelper.GetRoundByStep(1.345, 0.25));
            ClassicAssert.AreEqual(3.25, MathHelper.GetRoundByStep(3.234, 0.25));
            ClassicAssert.AreEqual(4.25, MathHelper.GetRoundByStep(4.256, 0.25));
        }

        [TestMethod]
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

            ClassicAssert.AreEqual(1.31, Math.Round(MathHelper.GetAverageValue(dictionary), 2));
        }

        [TestMethod]
        public void TestGetPercentageOf()
        {
            ClassicAssert.AreEqual(0, MathHelper.GetPercentageOf(0, 10));
            ClassicAssert.AreEqual(0, MathHelper.GetPercentageOf(10, 0));
            ClassicAssert.AreEqual(4, MathHelper.GetPercentageOf(100, 4));
            ClassicAssert.AreEqual(2.68, MathHelper.GetPercentageOf(67, 4));
            ClassicAssert.AreEqual(458, MathHelper.GetPercentageOf(458, 100));
            ClassicAssert.AreEqual(132, MathHelper.GetPercentageOf(120, 110));
        }
    }
}
