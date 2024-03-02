using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using UntappdViewer.Utils;

namespace UntappdViewer.Test.Utils
{
    [TestFixture]
    public class ExtensionFixture
    {
        [Test]
        public void TestListMove()
        {
            List<string> list1 = new List<string>();
            list1.Add("aaaa");
            list1.Add("bbbb");
            list1.Add("cccc");
            list1.Add("dddd");
            list1.Add("1234");

            list1.MoveToBottom("ffff");
            ClassicAssert.AreEqual("1234", list1[4]);
            list1.MoveToBottom("bbbb");
            ClassicAssert.AreEqual("bbbb", list1[4]);
            list1.Add("cccc");
            list1.Add("----");
            list1.MoveToBottom("cccc");
            ClassicAssert.AreEqual("cccc", list1[4]);
            ClassicAssert.AreEqual("cccc", list1[6]);

            List<int> list2 = new List<int>();
            list2.Add(1);
            list2.Add(2);
            list2.Add(55);
            list2.Add(3);
            list2.Add(4);

            list2.MoveToBottom(55);
            ClassicAssert.AreEqual(55, list2[4]);
        }
   
    }
}
