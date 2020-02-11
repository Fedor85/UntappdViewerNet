using System.IO;
using NUnit.Framework;
using UntappdViewer.Infrastructure;
using UntappdViewer.Test.Properties;

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

            string filePath = TestHelper.GetTempFilePath(Resources.ResourcesTestFileName);
            FileStatus fileStatus1= FileHelper.Check(filePath);
            Assert.AreEqual(FileStatus.Available, fileStatus1);

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Write))
            {
                FileStatus fileStatus2 = FileHelper.Check(filePath);
                Assert.AreEqual(FileStatus.IsLocked, fileStatus2);
            }

            File.Delete(filePath);

            FileStatus fileStatus3 = FileHelper.Check(filePath);
            Assert.AreEqual(FileStatus.NotExists, fileStatus3);
        }
    }
}