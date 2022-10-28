﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UntappdViewer.Domain.Mappers;
using UntappdViewer.Domain.Services;
using UntappdViewer.Infrastructure;
using UntappdViewer.Infrastructure.Services;
using UntappdViewer.Interfaces.Services;
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

        public static IUntappdService GetUntappdService()
        {
            string filePath = $"Temp{Path.GetFileName(Resources.ResourcesTestFileName)}";
            SaveResourceToFile(Resources.ResourcesTestFileName, filePath);
            IUntappdService untappdService = new UntappdService(new SettingService());
            untappdService.Initialize(filePath);
            File.Delete(filePath);
            return untappdService;
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

        public static bool IsUpdateBeer(Beer beer)
        {
            return beer.GlobalRatingScore == 0;
        }

        public static List<string> GetBeerTypes()
        {
            List<string> beerTypes = new List<string>();
            foreach (string beerType in Consts.BeerTypsText.Split('*'))
            {
                string currentBeerType = beerType.Trim();
                if (String.IsNullOrEmpty(currentBeerType))
                    continue;

                beerTypes.Add(currentBeerType);
            }
            return beerTypes;
        }

        private static string GetTempFilePath(string tempPathDirectory, string extension)
        {
            string randomFileName = Path.GetRandomFileName();
            string randomFileNameExtension = Path.ChangeExtension(randomFileName, extension);
            return Path.Combine(tempPathDirectory, randomFileNameExtension);
        }

        private static void SaveResourceToFile(string resourcesName, string filePath)
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcesName))
                FileHelper.SaveStreamToFile(stream, filePath);
        }
    }
}