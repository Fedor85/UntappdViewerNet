using System.Collections.Generic;
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
            List<string> extensions = Extensions.GetSupportExtensions();
            string filePathEmpty = string.Empty;

            FileStatus fileStatusEmpty = FileHelper.Check(filePathEmpty, extensions);
            Assert.AreEqual(FileStatus.NotExists, fileStatusEmpty);

            string filePath = TestHelper.GetTempFilePath(Resources.ResourcesTestFileName);
            FileStatus fileStatus1= FileHelper.Check(filePath, extensions);
            Assert.AreEqual(FileStatus.Available, fileStatus1);

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Write))
            {
                FileStatus fileStatus2 = FileHelper.Check(filePath, extensions);
                Assert.AreEqual(FileStatus.IsLocked, fileStatus2);
            }

            string filePathNoSupported = Path.ChangeExtension(filePath, ".xls");
            File.Copy(filePath, filePathNoSupported);
            FileStatus noSupported = FileHelper.Check(filePathNoSupported, extensions);
            Assert.AreEqual(FileStatus.NoSupported, noSupported);

            File.Delete(filePathNoSupported);
            File.Delete(filePath);

            FileStatus fileStatus3 = FileHelper.Check(filePath, extensions);
            Assert.AreEqual(FileStatus.NotExists, fileStatus3);
        }
    }
}