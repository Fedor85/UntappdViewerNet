using System.Collections.Generic;
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
        public void Test()
        {
            Client untappdClient = new Client(AccessToken);

            string message;
            Assert.True(untappdClient.Check(out message));

            List<Checkin> checkins = untappdClient.GetCheckins();
        }
    }
}