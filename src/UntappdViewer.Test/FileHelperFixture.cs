using NUnit.Framework;
using UntappdViewer.Infrastructure;

namespace UntappdViewer.Test
{
    [TestFixture]
    public class FileHelperFixture
    {
        [Test]
        public void Test()
        {
            string filePathEmpty = string.Empty;
            FileStatus fileStatusEmpty = FileHelper.Check(filePathEmpty);
            Assert.AreEqual(FileStatus.NotExists, fileStatusEmpty);
        }
    }
}