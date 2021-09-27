using System;
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

        private bool isСhanges;

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
                Untappd.AddCheckins(CheckinCSVMapper.GetCheckins(fileStream));

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
            return isСhanges;
        }

        public void ResetСhanges()
        {
            isСhanges = false;
        }

        public string GetUntappdUserName()
        {
            return Untappd.UserName;
        }

        public void UpdateUntappdUserName(string untappdUserName)
        {
            if (Untappd.UserName.Equals(untappdUserName))
                return;

            isСhanges = true;
            Untappd.UserName = GetUntappdUserName(untappdUserName);
            UpdateUntappdUserNameEvent?.Invoke(Untappd.UserName);
        }

        public string GetUntappdProjectFileName()
        {
            return $"{Untappd.CreatedDate:yyyy_MMM_dd}_{Untappd.UserName}";
        }

        public string GetFullUntappdProjectPhotoFilesDirectory()
        {
            return Path.Combine(Path.GetDirectoryName(FilePath), GetUntappdProjectPhotoFilesDirectory());
        }


        public string GetFullCheckinPhotoFilePath(Checkin checkin)
        {
            return Path.Combine(GetFullUntappdProjectPhotoFilesDirectory(), Path.GetFileName(checkin.UrlPhoto));
        }

        public List<Checkin> GeCheckins(bool isUniqueCheckins = false)
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

        private string GetUntappdProjectPhotoFilesDirectory()
        {
            return $"{Path.GetFileNameWithoutExtension(FilePath)}_Photos";
        }
    }
}