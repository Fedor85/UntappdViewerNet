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
        }

        [Test, Ignore(DefaultValues.DefaultServingType)]
        public void TestUpdateServing()
        {
            webApiClient.FillServingType(checkinsContainer.Checkins, DefaultValues.DefaultServingType);
        }
    }
}