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

        [Test]
        public void TestCheckins()
        {
            Client untappdClient = new Client();
            untappdClient.Initialize(AccessToken);

            Assert.True(untappdClient.Check());
            CheckinsContainer checkinsContainer = new CheckinsContainer();
            untappdClient.FillFullCheckins(checkinsContainer);
        }

        [Test]
        public void TestBeers()
        {
            Client untappdClient = new Client();
            untappdClient.Initialize(AccessToken);

            Assert.True(untappdClient.Check());
            CheckinsContainer checkinsContainer = TestHelper.GetCheckinsContainer();
            untappdClient.UpdateBeers(checkinsContainer.Beers.Where(TestHelper.IsUpdateBeer).ToList(), TestHelper.IsUpdateBeer);
        }
    }
}