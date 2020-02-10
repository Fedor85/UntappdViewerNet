using System.Collections.Generic;
using NUnit.Framework;
using UntappdViewer.Mappers;
using UntappdViewer.Models;

namespace UntappdViewer.Test
{
    [TestFixture]
    public class CheckinMapperFixture
    {
        [Test]
        public void CheckinTextMapperTest()
        {
            List<Checkin> checkins = CheckinTextMapper.GetCheckins(Properties.Resources._78a7bb4694ba2d4c33ba2dc76d0fc60d);
            Assert.IsTrue(checkins.Count > 0);
        }
    }
}