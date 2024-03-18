using System;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
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
            webApiClient.LogOn(AccessToken);
        }

        [Test, Ignore(AccessToken)]
        public void TestFillFullCheckins()
        {
            ClassicAssert.True(webApiClient.IsLogOn);
            webApiClient.FillFullCheckins(checkinsContainer);
        }

        [Test, Ignore(AccessToken)]
        public void TestUpdateBeers()
        {
            ClassicAssert.True(webApiClient.IsLogOn);
            long offset = 0;
            webApiClient.UpdateBeers(checkinsContainer.Beers.Where(TestHelper.IsUpdateBeer).ToList(), TestHelper.IsUpdateBeer, ref offset);
            ClassicAssert.AreNotEqual(offset, 0);
        }

        [Test]
        public void TestUpdateServingType()
        {
            foreach (Checkin checkin in checkinsContainer.Checkins)
                checkin.ServingType = String.Empty;

            webApiClient.FillServingType(checkinsContainer.Checkins, DefaultValues.DefaultServingType);
            int servingTypeCount = checkinsContainer.Checkins.Count(item => !String.IsNullOrEmpty(item.ServingType) && !item.ServingType.Equals(DefaultValues.DefaultServingType));
            ClassicAssert.AreNotEqual(servingTypeCount, 0);
        }

        [Test, Ignore(AccessToken)]
        public void FillCollaboration()
        {
            ClassicAssert.True(webApiClient.IsLogOn);
            webApiClient.FillCollaboration(checkinsContainer.Beers, checkinsContainer.Brewerys);
        }

        [Test]
        public void TestDevProfileData()
        {
            ClassicAssert.True(!String.IsNullOrEmpty(webApiClient.GetDevAvatarImageUrl()));
            ClassicAssert.True(!String.IsNullOrEmpty(webApiClient.GetDevProfileHeaderImageUrl()));
        }
    }
}