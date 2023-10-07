using System;
using System.Linq;
using NUnit.Framework;
using UntappdViewer.Models;
using UntappdWebApiClient;

namespace UntappdViewer.Test
{
    [TestFixture]
    public class UntappdWebApiClientFixture
    {
        //access_token
        private const string AccessToken = "access_token";

        private CheckinsContainer checkinsContainer;

        private Client webApiClient;

        public UntappdWebApiClientFixture()
        {
            checkinsContainer = TestHelper.GetCheckinsContainer();
            webApiClient = new Client();
            webApiClient.Initialize(AccessToken);
        }

        [Test, Ignore(AccessToken)]
        public void TestFillFullCheckins()
        {
            Assert.True(webApiClient.Check());
            webApiClient.FillFullCheckins(checkinsContainer);
        }

        [Test, Ignore(AccessToken)]
        public void TestUpdateBeers()
        {
            Assert.True(webApiClient.Check());
            long offset = 0;
            webApiClient.UpdateBeers(checkinsContainer.Beers.Where(TestHelper.IsUpdateBeer).ToList(), TestHelper.IsUpdateBeer, ref offset);
            Assert.AreNotEqual(offset, 0);
        }

        [Test]
        public void TestUpdateServing()
        {
            foreach (Checkin checkin in checkinsContainer.Checkins)
                checkin.ServingType = String.Empty;

            webApiClient.FillServingType(checkinsContainer.Checkins, DefaultValues.DefaultServingType);
            int servingTypeCount = checkinsContainer.Checkins.Count(item => !String.IsNullOrEmpty(item.ServingType) && !item.ServingType.Equals(DefaultValues.DefaultServingType));
            Assert.AreNotEqual(servingTypeCount, 0);
        }

        [Test, Ignore(AccessToken)]
        public void FillCollaboration()
        {
            Assert.True(webApiClient.Check());
            webApiClient.FillCollaboration(checkinsContainer.Beers, checkinsContainer.Brewerys);
        }

        [Test]
        public void TestDevProfileData()
        {
            Assert.True(!String.IsNullOrEmpty(webApiClient.GetDevAvatarImageUrl()));
            Assert.True(!String.IsNullOrEmpty(webApiClient.GetDevProfileHeaderImageUrl()));
        }
    }
}