using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using UntappdViewer.Domain;
using UntappdViewer.Domain.Mappers;
using UntappdViewer.Domain.Models;
using UntappdViewer.Models;
using UntappdViewer.Test.Properties;
using UntappdViewer.Utils;

namespace UntappdViewer.Test
{
    [TestFixture]
    public class  StatisticsCalculationFixture
    {

        private CheckinsContainer checkinsContainer;

        public StatisticsCalculationFixture()
        {
            checkinsContainer = GetCheckinsContainer();
        }

        private CheckinsContainer GetCheckinsContainer()
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Resources.ResourcesTestFileName))
            {
                CheckinsContainer checkinsContainer = new CheckinsContainer();
                CheckinCSVMapper.InitializeCheckinsContainer(checkinsContainer, stream);
                return checkinsContainer;
            }
        }

        [Test]
        public void TestGetCountrysCount()
        {
            int countrysCount = StatisticsCalculation.GetCountrysCount(checkinsContainer.Checkins);
            Assert.AreEqual(38, countrysCount);
        }

        [Test]
        public void TestGetChekinsRatingByCount()
        {
            List<KeyValue<double, int>> chekinsRating = StatisticsCalculation.GetChekinsRatingByCount(checkinsContainer.Checkins);

            Assert.AreEqual(31, chekinsRating.Count);
            Assert.AreEqual(0.25, chekinsRating[0].Key);
            Assert.AreEqual(4, chekinsRating[0].Value);
            Assert.AreEqual(5, chekinsRating[30].Key);
            Assert.AreEqual(44, chekinsRating[30].Value);
        }

        [Test]
        public void TestGetBeersRatingByCount()
        {
            List<KeyValue<double, int>> beersRating = StatisticsCalculation.GetBeersRatingByCount(checkinsContainer.Beers);

            Assert.AreEqual(11, beersRating.Count);
            Assert.AreEqual(0, beersRating[0].Key);
            Assert.AreEqual(14, beersRating[0].Value);
            Assert.AreEqual(4, beersRating[10].Key);
            Assert.AreEqual(15, beersRating[10].Value);
        }

        [Test]
        public void TesGetDateChekinsByCount()
        {
            List<KeyValue<string, int>> dateChekinsCount = StatisticsCalculation.GetDateChekinsByCount(checkinsContainer.Checkins);

            Assert.AreEqual(65, dateChekinsCount.Count);
            Assert.AreEqual("08.15", dateChekinsCount[0].Key);
            Assert.AreEqual(30, dateChekinsCount[0].Value);
            Assert.AreEqual("12.20", dateChekinsCount[64].Key);
            Assert.AreEqual(0, dateChekinsCount[64].Value);
        }

        [Test]
        public void TestGetTotalDaysByNow()
        {
            Assert.AreEqual(GetTotalDay(), MathHelper.GetTotalDaysByNow(checkinsContainer.Checkins.Select(item => item.CreatedDate).ToList()));
        }

        [Test]
        public void TestGetAverageCountByNow()
        {
            double averageCount = Math.Round(checkinsContainer.Checkins.Count / GetTotalDay(), 2);
            Assert.AreEqual(averageCount, Math.Round(MathHelper.GetAverageCountByNow(checkinsContainer.Checkins.Select(item => item.CreatedDate).ToList()), 2));
        }

        [Test]
        public void TestGetBeerTypesByCheckinIdsGroupByCount()
        {
            List<KeyValue<string, List<long>>> beerTypeCheckinIds = StatisticsCalculation.GetBeerTypesByCheckinIdsGroupByCount(checkinsContainer.Checkins);

            Assert.AreEqual(16, beerTypeCheckinIds.Count);
            Assert.AreEqual("Other", beerTypeCheckinIds[0].Key);
            Assert.AreEqual(175, beerTypeCheckinIds[0].Value.Count);
            Assert.AreEqual("Belgian", beerTypeCheckinIds[15].Key);
            Assert.AreEqual(26, beerTypeCheckinIds[15].Value.Count);
        }

        [Test]
        public void TestGetCountrysByCheckinIds()
        {
            List<KeyValue<string, List<long>>> beerCountryCheckinIds = StatisticsCalculation.GetCountrysByCheckinIds(checkinsContainer.Checkins);

            Assert.AreEqual(38, beerCountryCheckinIds.Count);
            Assert.AreEqual("United States", beerCountryCheckinIds[0].Key);
            Assert.AreEqual(11, beerCountryCheckinIds[0].Value.Count);
            Assert.AreEqual("Armenia", beerCountryCheckinIds[37].Key);
            Assert.AreEqual(2, beerCountryCheckinIds[37].Value.Count);
        }

        [Test]
        public void TestGetListCount()
        {
            List<KeyValue<string, List<long>>> beerTypeCheckinIds = StatisticsCalculation.GetBeerTypesByCheckinIdsGroupByCount(checkinsContainer.Checkins);
            List<KeyValue<string, int>> beerTypesCount = StatisticsCalculation.GetListCount(beerTypeCheckinIds);

            Assert.AreEqual(16, beerTypesCount.Count);
            Assert.AreEqual("Other", beerTypesCount[0].Key);
            Assert.AreEqual(175, beerTypesCount[0].Value);
            Assert.AreEqual("Belgian", beerTypesCount[15].Key);
            Assert.AreEqual(26, beerTypesCount[15].Value);

            List<KeyValue<string, List<long>>> beerCountryCheckinIds = StatisticsCalculation.GetCountrysByCheckinIds(checkinsContainer.Checkins);
            List<KeyValue<string, int>> beerCountrysCount = StatisticsCalculation.GetListCount(beerCountryCheckinIds);

            Assert.AreEqual(38, beerCountrysCount.Count);
            Assert.AreEqual("United States", beerCountrysCount[0].Key);
            Assert.AreEqual(11, beerCountrysCount[0].Value);
            Assert.AreEqual("Armenia", beerCountrysCount[37].Key);
            Assert.AreEqual(2, beerCountrysCount[37].Value);
        }

        [Test]
        public void TestGetAccumulateValues()
        {
            List<KeyValue<string, int>> dataChekins = StatisticsCalculation.GetDateChekinsByCount(checkinsContainer.Checkins);
            List<KeyValue<string, int>> dateChekinsAccumulateCount = StatisticsCalculation.GetAccumulateValues(dataChekins);

            Assert.AreEqual(65, dataChekins.Count);
            Assert.AreEqual(65, dateChekinsAccumulateCount.Count);

            Assert.AreEqual("08.15", dataChekins[0].Key);
            Assert.AreEqual(30, dataChekins[0].Value);
            Assert.AreEqual("08.15", dateChekinsAccumulateCount[0].Key);
            Assert.AreEqual(30, dateChekinsAccumulateCount[0].Value);


            Assert.AreEqual("09.15", dataChekins[1].Key);
            Assert.AreEqual(26, dataChekins[1].Value);
            Assert.AreEqual("09.15", dateChekinsAccumulateCount[1].Key);
            Assert.AreEqual(56, dateChekinsAccumulateCount[1].Value);

            Assert.AreEqual("10.15", dataChekins[2].Key);
            Assert.AreEqual(24, dataChekins[2].Value);
            Assert.AreEqual("10.15", dateChekinsAccumulateCount[2].Key);
            Assert.AreEqual(80, dateChekinsAccumulateCount[2].Value);

            Assert.AreEqual("12.20", dataChekins[64].Key);
            Assert.AreEqual(0, dataChekins[64].Value);
            Assert.AreEqual("12.20", dateChekinsAccumulateCount[64].Key);
            Assert.AreEqual(checkinsContainer.Checkins.Count, dateChekinsAccumulateCount[64].Value);
        }

        [Test]
        public void TestGetAverageRatingByCheckinIds()
        {
            List<KeyValue<string, List<long>>> beerTypeCheckinIds = StatisticsCalculation.GetBeerTypesByCheckinIdsGroupByCount(checkinsContainer.Checkins);
            List<KeyValue<string, double>> beerTypeRating = StatisticsCalculation.GetAverageRatingByCheckinIds(checkinsContainer.Checkins, beerTypeCheckinIds);

            Assert.AreEqual(16, beerTypeRating.Count);
            Assert.AreEqual("Other", beerTypeRating[0].Key);
            Assert.AreEqual(3.41, beerTypeRating[0].Value);
            Assert.AreEqual("Belgian", beerTypeRating[15].Key);
            Assert.AreEqual(3.38, beerTypeRating[15].Value);

            List<KeyValue<string, List<long>>> beerCountryCheckinIds = StatisticsCalculation.GetCountrysByCheckinIds(checkinsContainer.Checkins);
            List<KeyValue<string, double>> beerCountryRating = StatisticsCalculation.GetAverageRatingByCheckinIds(checkinsContainer.Checkins, beerCountryCheckinIds);

            Assert.AreEqual(38, beerCountryRating.Count);
            Assert.AreEqual("United States", beerCountryRating[0].Key);
            Assert.AreEqual(3.35, beerCountryRating[0].Value);
            Assert.AreEqual("Armenia", beerCountryRating[37].Key);
            Assert.AreEqual(4.25, beerCountryRating[37].Value);
        }

        private double GetTotalDay()
        {
            double totalCheckinDays = (checkinsContainer.Checkins.Max(item => item.CreatedDate) - checkinsContainer.Checkins.Min(item => item.CreatedDate)).TotalDays;
            double otherDaysNow = (DateTime.Now - checkinsContainer.Checkins.Max(item => item.CreatedDate)).TotalDays;
            return Math.Ceiling(totalCheckinDays + otherDaysNow);
        }
    }
}