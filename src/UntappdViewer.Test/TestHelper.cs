using System.IO;
using System.Reflection;
using UntappdViewer.Domain.Mappers;
using UntappdViewer.Models;
using UntappdViewer.Test.Properties;

namespace UntappdViewer.Test
{
    public static class TestHelper
    {
        public static string GetTempFilePath(string resourcesTestFileName)
        {
            string tempFilePath;
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcesTestFileName))
            {
                string tempPathDirectory = Path.GetTempPath();
                string extension = Path.GetExtension(resourcesTestFileName);
                tempFilePath = GetTempFilePath(tempPathDirectory, extension);
                while (File.Exists(tempFilePath))
                    tempFilePath = GetTempFilePath(tempPathDirectory, extension);

                using (FileStream fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write))
                    stream.CopyTo(fileStream);
            }
            return tempFilePath;
        }

        public static CheckinsContainer GetCheckinsContainer()
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Resources.ResourcesTestFileName))
            {
                CheckinsContainer checkinsContainer = new CheckinsContainer();
                CheckinCSVMapper.InitializeCheckinsContainer(checkinsContainer, stream);
                return checkinsContainer;
            }
        }

        private static string GetTempFilePath(string tempPathDirectory, string extension)
        {
            string randomFileName = Path.GetRandomFileName();
            string randomFileNameExtension = Path.ChangeExtension(randomFileName, extension);
            return Path.Combine(tempPathDirectory, randomFileNameExtension);
        }
    }
}