using System.Collections.Generic;
using NUnit.Framework;
using UntappdViewer.Utils;

namespace UntappdViewer.Test
{
    [TestFixture]
    public class TestStringHelper
    {
        [Test]
        public void TestGetShortName()
        {
            List<string> fullNames = new List<string>();
            fullNames.Add("LIDSKAE Nulevachka (ЛІДСКАЕ Нулёвачка)");
            fullNames.Add("Golden Bumblebee Blackberry Ale (Черничный Эль)");
            fullNames.Add("Stary Melnik Iz Bochonka Myagkoe (Старый Мельник Из Бочонка Мягкое))");
            fullNames.Add("Seven Brewers (Семь Пивоваров) Cheshskoe Original (Чешское Оригинальное)");
            fullNames.Add("Lidski Gingerbread kvass (Квас Лидский Пряничный (медово-имбирный))");

            List<string> shortNames = new List<string>();
            foreach (string fullName in fullNames)
                shortNames.Add(StringHelper.GetShortName(fullName));

            Assert.AreEqual("LIDSKAE Nulevachka", shortNames[0]);
            Assert.AreEqual("Golden Bumblebee Blackberry Ale", shortNames[1]);
            Assert.AreEqual("Stary Melnik Iz Bochonka Myagkoe", shortNames[2]);
            Assert.AreEqual("Seven Brewers", shortNames[3]);
            Assert.AreEqual("Lidski Gingerbread kvass", shortNames[4]);
        }
    }
}