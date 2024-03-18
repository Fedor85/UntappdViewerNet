using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework.Legacy;
using UntappdViewer.Utils;

namespace UntappdViewer.Test.Utils
{
    [TestClass]
    public class StringHelperFixture
    {
        [TestMethod]
        public void TestGetShortName()
        {
            List<string> fullNames = new List<string>();
            fullNames.Add("LIDSKAE Nulevachka (ЛІДСКАЕ Нулёвачка)");
            fullNames.Add("Golden Bumblebee Blackberry Ale (Черничный Эль)");
            fullNames.Add("Stary Melnik Iz Bochonka Myagkoe (Старый Мельник Из Бочонка Мягкое))");
            fullNames.Add("Seven Brewers (Семь Пивоваров) Cheshskoe Original (Чешское Оригинальное)");
            fullNames.Add("Lidski Gingerbread kvass (Квас Лидский Пряничный (медово-имбирный))");
            fullNames.Add("Paulaner Hefe-Weizen / Hefe-Weißbier Naturtrüb / Natural Wheat");
            fullNames.Add("Hoegaarden Wit / Blanche 0,0 %");
            fullNames.Add("Spaten München / Münchner Hell / (Premium Lager)");

            List<string> shortNames = new List<string>();
            foreach (string fullName in fullNames)
                shortNames.Add(StringHelper.GetShortName(fullName));

            ClassicAssert.AreEqual("LIDSKAE Nulevachka", shortNames[0]);
            ClassicAssert.AreEqual("Golden Bumblebee Blackberry Ale", shortNames[1]);
            ClassicAssert.AreEqual("Stary Melnik Iz Bochonka Myagkoe", shortNames[2]);
            ClassicAssert.AreEqual("Seven Brewers Cheshskoe Original", shortNames[3]);
            ClassicAssert.AreEqual("Lidski Gingerbread kvass", shortNames[4]);
            ClassicAssert.AreEqual("Paulaner Hefe-Weizen", shortNames[5]);
            ClassicAssert.AreEqual("Hoegaarden Wit", shortNames[6]);
            ClassicAssert.AreEqual("Spaten München", shortNames[7]);
        }

        [TestMethod]
        public void TestGeBreakForLongName()
        {
            string name1 = "Imperial Rye Porter Aged In Jack Daniels Barrels Batch #2";
            string resultName11 = StringHelper.GeBreakForLongName(name1, 30, 5);
            ClassicAssert.AreEqual("Imperial Rye Porter Aged In\n     Jack Daniels Barrels Batch #2", resultName11);

            string resultName12 = StringHelper.GeBreakForLongName(name1, 30, 0);
            ClassicAssert.AreEqual("Imperial Rye Porter Aged In\nJack Daniels Barrels Batch #2", resultName12);

            string name2 = "LIDSKAE Nulevachka";
            string resultName2 = StringHelper.GeBreakForLongName(name2, 30, 5);
            ClassicAssert.AreEqual(name2, resultName2);
        }

        [TestMethod]
        public void TestGetGroupByList()
        {
            List<string> beerTypes = TestHelper.GetBeerTypes();
            ClassicAssert.AreEqual(beerTypes.Count, 205);
            Dictionary<string, List<string>> group = StringHelper.GetGroupByList(TestHelper.GetBeerTypes(), "/", "-");
            ClassicAssert.AreEqual(group.Count, 66);

            KeyValuePair<string, List<string>> val1 = group.ElementAt(0);
            ClassicAssert.AreEqual(val1.Key, "Altbier");
            ClassicAssert.AreEqual(val1.Value.Count, 1);

            KeyValuePair<string, List<string>> val2 = group.ElementAt(2);
            ClassicAssert.AreEqual(val2.Key, "Barleywine");
            ClassicAssert.AreEqual(val2.Value.Count, 3);

            KeyValuePair<string, List<string>> val3 = group.ElementAt(10);
            ClassicAssert.AreEqual(val3.Key, "Brown Ale");
            ClassicAssert.AreEqual(val3.Value.Count, 5);
        }

        [TestMethod]
        public void TestGetCutByFirstChars()
        {
            ClassicAssert.AreEqual("China / People's Republic of China", StringHelper.GetCutByFirstChars("China / People's Republic of China"));
            ClassicAssert.AreEqual("China / People's Republic of China", StringHelper.GetCutByFirstChars("China / People's Republic of China", ";"));
            ClassicAssert.AreEqual("China", StringHelper.GetCutByFirstChars("China / People's Republic of China", "/"));
            ClassicAssert.AreEqual("Chi", StringHelper.GetCutByFirstChars("China / People's Republic of China", "/", "n"));
            ClassicAssert.AreEqual("", StringHelper.GetCutByFirstChars("China / People's Republic of China", "/", "C"));
        }
    }
}