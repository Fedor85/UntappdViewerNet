using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework.Legacy;
using UntappdViewer.Interfaces;
using UntappdViewer.Models;
using UntappdWebApiClient;

namespace UntappdViewer.Test
{
    [TestClass]
    public class UntappdWebApiClientFixture
    {
        //access_token
        private const string AccessToken = "access_token";

        //clientID
        private const string ClientID = "clientID";

        //clientSecret
        private const string ClientSecret = "clientSecret";

        private const string RedirectUrl = "http://localhost";

        private CheckinsContainer checkinsContainer;

        private Client webApiClient;

        public UntappdWebApiClientFixture()
        {
            checkinsContainer = TestHelper.GetCheckinsContainer();
            webApiClient = new Client();
            webApiClient.LogOn(AccessToken);
        }

        [TestMethod, Ignore(AccessToken)]
        public void TestFillFullCheckins()
        {
            ClassicAssert.True(webApiClient.IsLogOn);
            webApiClient.FillFullCheckins(checkinsContainer);
        }

        [TestMethod, Ignore(AccessToken)]
        public void TestUpdateBeers()
        {
            ClassicAssert.True(webApiClient.IsLogOn);
            long offset = 0;
            webApiClient.UpdateBeers(checkinsContainer.Beers.Where(TestHelper.IsUpdateBeer).ToList(), TestHelper.IsUpdateBeer, ref offset);
            ClassicAssert.AreNotEqual(offset, 0);
        }

        [TestMethod]
        public void TestUpdateServingType()
        {
            foreach (Checkin checkin in checkinsContainer.Checkins)
                checkin.ServingType = String.Empty;

            webApiClient.FillServingType(checkinsContainer.Checkins, DefaultValues.DefaultServingType);
            int servingTypeCount = checkinsContainer.Checkins.Count(item => !String.IsNullOrEmpty(item.ServingType) && !item.ServingType.Equals(DefaultValues.DefaultServingType));
            ClassicAssert.AreNotEqual(servingTypeCount, 0);
        }

        [TestMethod]
        public void FillCollaboration()
        {
            webApiClient.FillCollaboration(checkinsContainer.Beers, checkinsContainer.Brewerys);
        }

        [TestMethod]
        public void TestDevProfileData()
        {
            ClassicAssert.True(!String.IsNullOrEmpty(webApiClient.GetDevAvatarImageUrl()));
            ClassicAssert.True(!String.IsNullOrEmpty(webApiClient.GetDevProfileHeaderImageUrl()));
        }

        [TestMethod, Ignore(AccessToken)]
        public void TestGetAccessToken()
        {
            IResponseMessage authenticaResponseMessage = webApiClient.CheckAuthenticateUrl(ClientID, RedirectUrl);
            ClassicAssert.AreEqual(200, authenticaResponseMessage.Code);

            IResponseMessage authenticaAccessToken = webApiClient.GetAccessToken(ClientID, ClientSecret, RedirectUrl, "");
        }
    }
}