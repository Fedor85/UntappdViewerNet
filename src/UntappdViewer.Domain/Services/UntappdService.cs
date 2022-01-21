﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UntappdViewer.Domain.Mappers;
using UntappdViewer.Infrastructure;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Models;
using UntappdViewer.Utils;

namespace UntappdViewer.Domain.Services
{
    public class UntappdService : IUntappdService
    {
        private ISettingService settingService;

        public Untappd Untappd { get; private set; }

        public Action<Untappd> InitializeUntappdEvent { get; set; }

        public Action<string> UpdateUntappdUserNameEvent { get; set; }

        public Action CleanUntappdEvent { get; set; }

        public string FilePath { get; private set; }

        public UntappdService(ISettingService settingService)
        {
            this.settingService = settingService;
            Untappd = new Untappd(String.Empty);
        }

        public void Create(string userName)
        {
            Untappd = new Untappd(GetUntappdUserName(userName));
            InitializeUntappdEvent?.Invoke(Untappd);
            UpdateUntappdUserNameEvent?.Invoke(Untappd.UserName);
            ResetСhanges();
        }

        public void Initialize(string filePath, string userName = null)
        {
            switch (FileHelper.GetExtensionWihtoutPoint(filePath))
            {
                case Extensions.CSV:
                    InitializeToCSV(filePath, userName);
                    break;
                case Extensions.UNTP:
                    InitializeToUNTP(filePath);
                    break;
                default:
                    throw new ArgumentException(String.Format(Properties.Resources.ArgumentExceptionInitializeUntappd, filePath));
            }
            FilePath = filePath;
            InitializeUntappdEvent?.Invoke(Untappd);
            UpdateUntappdUserNameEvent?.Invoke(Untappd.UserName);
            ResetСhanges();
        }

        private void InitializeToCSV(string filePath, string userName)
        {     
            Untappd = new Untappd(GetUntappdUserName(userName));
            using (FileStream fileStream = File.OpenRead(filePath))
                CheckinCSVMapper.InitializeCheckinsContainer(Untappd.CheckinsContainer, fileStream);

            Untappd.SortDataDescCheckins();
        }

        private void InitializeToUNTP(string filePath)
        {
            Untappd = FileHelper.OpenFile<Untappd>(filePath);
        }

        public void CleanUpUntappd()
        {
            FilePath = String.Empty;
            Untappd = new Untappd(String.Empty);
            CleanUntappdEvent?.Invoke();
        }

        public bool IsUNTPProject()
        {
            return !String.IsNullOrEmpty(FilePath) && GetProjectExtensions() == Extensions.UNTP;
        }

        private string GetProjectExtensions()
        {
            switch (FileHelper.GetExtensionWihtoutPoint(FilePath))
            {
                case Extensions.CSV: return Extensions.CSV;
                case Extensions.UNTP: return Extensions.UNTP;
                default:
                    throw new ArgumentException(String.Format(Properties.Resources.ArgumentExceptionInitializeUntappd, FilePath));
            }
        }

        public bool IsDirtyUntappd()
        {
            return Untappd.IsСhanges();
        }

        public void ResetСhanges()
        {
            Untappd.ResetСhanges();
        }

        public string GetUntappdUserName()
        {
            return Untappd.UserName;
        }

        public void UpdateUntappdUserName(string untappdUserName)
        {
            if (Untappd.UserName.Equals(untappdUserName))
                return;

            Untappd.SetUserName(GetUntappdUserName(untappdUserName));
            UpdateUntappdUserNameEvent?.Invoke(Untappd.UserName);
        }

        public string GetUntappdProjectFileName()
        {
            return $"{Untappd.CreatedDate:yyyy_MMM_dd}_{Untappd.UserName}";
        }

        public string GetFileDataDirectory()
        {
            return Path.Combine(Path.GetDirectoryName(FilePath), $"{Path.GetFileNameWithoutExtension(FilePath)}_Data");
        }

        public string GetCheckinPhotoFilePath(Checkin checkin)
        {
            return Path.Combine(GetFileDataDirectory(), "CheckinPhotos", Path.GetFileName(StringHelper.GetNormalizedJPGPath(checkin.UrlPhoto)));
        }

        public string GetBeerLabelFilePath(Beer beer)
        {
            return Path.Combine(GetFileDataDirectory(), "BeerLabels", Path.GetFileName(StringHelper.GetNormalizedJPGPath(beer.LabelUrl)));
        }

        public string GetBreweryLabelFilePath(Brewery brewery)
        {
            return Path.Combine(GetFileDataDirectory(), "BreweryLabels", Path.GetFileName(StringHelper.GetNormalizedJPGPath(brewery.LabelUrl)));
        }

        public string GetBadgeImageFilePath(Badge badge)
        {
            return Path.Combine(GetBadgeImageDirectory(), Path.GetFileName(StringHelper.GetNormalizedJPGPath(badge.ImageUrl)));
        }

        public string GetBadgeImageDirectory()
        {
            return Path.Combine(GetFileDataDirectory(), "BadgeImages");
        }

        public string GetReportsDirectory()
        {
            return Path.Combine(GetFileDataDirectory(), "Reports");
        }

        public List<Checkin> GetCheckins(bool isUniqueCheckins = false)
        {
            return isUniqueCheckins ? Untappd.GetUniqueCheckins() : Untappd.Checkins;
        }

        public Checkin GetCheckin(long checkinId)
        {
            return Untappd.Checkins.FirstOrDefault(item => item.Id.Equals(checkinId));
        }

        public string GetTreeViewCheckinDisplayName(Checkin checkin, int number)
        {
            string prefix = $"#{number} {checkin.CreatedDate.ToString("yyyy-MMM-dd")} ";
            string fullName = $"{prefix}{StringHelper.GetShortName(checkin.Beer.Name)}";
            return StringHelper.GeBreakForLongName(fullName, settingService.GetTreeItemNameMaxLength(), prefix.Length * 2 - 2);
        }

        public string GetUploadSavePhotoFileName(Checkin checkin)
        {
            return $"{checkin.CreatedDate.ToString("yyyy_MM_dd")}_{checkin.Id}.{FileHelper.GetExtensionWihtoutPoint(checkin.UrlPhoto)}";
        }

        private string GetUntappdUserName(string userName)
        {
            return String.IsNullOrEmpty(userName) ? settingService.GetDefaultUserName() : userName;
        }
    }
}