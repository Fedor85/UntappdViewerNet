using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UntappdViewer.Domain;
using UntappdViewer.Infrastructure;
using UntappdViewer.Models;
using UntappdViewer.Test.Properties;

namespace UntappdViewer.Test
{
    [TestFixture]
    public class FileHelperFixture
    {
        [Test]
        public void TestFile()
        {
            List<string> extensions = Extensions.GetSupportExtensions();
            string filePathEmpty = string.Empty;

            FileStatus fileStatusEmpty = FileHelper.Check(filePathEmpty, extensions);
            Assert.AreEqual(FileStatus.IsEmptyPath, fileStatusEmpty);

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

        [Test]
        public void ParseFilePaths()
        {
            List<FileItem> fileEmptyItems = FileHelper.GetParseFilePaths(String.Empty);
            Assert.AreEqual(0, fileEmptyItems.Count);

            List<string> filePaths = new List<string>();
            filePaths.Add(@"1*C:\Windows\System32\AERTAC64.dll");
            filePaths.Add(@"2*C:\Windows\System32\3082\vsjitdebuggerui.dll");
            filePaths.Add(@"3*C:\Windows\System32\DAX2\DAX2.sdf");
            string allPath = String.Empty;
            foreach (string filePath in filePaths)
                allPath += filePath + "|";

            allPath = allPath.Remove(allPath.Length - 1, 1);
            List<FileItem> fileItems = FileHelper.GetParseFilePaths(allPath);
            Assert.AreEqual(3, fileItems.Count);
            Assert.AreEqual("AERTAC64.dll", fileItems[0].FileName);
            Assert.AreEqual("vsjitdebuggerui.dll", fileItems[1].FileName);
            Assert.AreEqual("DAX2.sdf", fileItems[2].FileName);

            Assert.AreEqual(String.Empty, FileHelper.GetMergedFilePaths(new List<FileItem>()));
            Assert.AreEqual(allPath, FileHelper.GetMergedFilePaths(fileItems));

            FileHelper.AddFile(fileItems, @"C:\Windows\System32\DAX2\DRTAIODAT2.DAT", 3);
            Assert.AreEqual(3, fileItems.Count);
            Assert.AreEqual("DRTAIODAT2.DAT", fileItems[0].FileName);
            Assert.AreEqual("AERTAC64.dll", fileItems[1].FileName);
            Assert.AreEqual("vsjitdebuggerui.dll", fileItems[2].FileName);

            string file = @"C:\Windows\System32\AERTAC64.dll";
            FileHelper.AddFile(fileItems, file, 3);
            Assert.AreEqual(3, fileItems.Count);
            Assert.AreEqual("AERTAC64.dll", fileItems[0].FileName);
            Assert.AreEqual("DRTAIODAT2.DAT", fileItems[1].FileName);
            Assert.AreEqual("vsjitdebuggerui.dll", fileItems[2].FileName);

            Assert.AreEqual(file, FileHelper.GetFirstFileItemPath(FileHelper.GetMergedFilePaths(fileItems)));
        }

        [Test, Ignore("для удаления чекинов")]
        public void CheckinsRemove()
        {
            string filePath = @"";
            Untappd untappd = FileHelper.OpenFile<Untappd>(filePath);
            untappd.CheckinsContainer.Checkins.RemoveRange(0, 25);
            FileHelper.SaveFile(filePath, untappd);
        }
    }
}