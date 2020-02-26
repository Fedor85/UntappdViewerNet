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
    public class UntappdService
    {
        private ISettingService settingService;

        public Untappd Untappd { get; private set; }

        public Action<Untappd> InitializeUntappdEvent { get; set; }

        public Action<string> UpdateUntappdUserNameEvent { get; set; }

        public Action CleanUntappdEvent { get; set; }

        public string FIlePath { get;  set; }

        public UntappdService(ISettingService settingService)
        {
            this.settingService = settingService;
            Untappd = new Untappd(String.Empty);
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

            InitializeUntappdEvent?.Invoke(Untappd);
            UpdateUntappdUserNameEvent?.Invoke(Untappd.UserName);
        }

        private void InitializeToCSV(string filePath, string userName)
        {
            FIlePath = filePath;
            Untappd = new Untappd(GetUntappdUserName(userName));
            using (FileStream fileStream = File.OpenRead(filePath))
                Untappd.AddCheckins(CheckinCSVMapper.GetCheckins(fileStream));

            Untappd.SortDataDescCheckins();
        }

        private void InitializeToUNTP(string filePath)
        {
            FIlePath = filePath;
            Untappd = FileHelper.OpenFile<Untappd>(filePath);
        }

        public void CleanUpUntappd()
        {
            FIlePath = String.Empty;
            Untappd = new Untappd(String.Empty);
            CleanUntappdEvent?.Invoke();
        }

        public string GetUntappdUserName()
        {
            return Untappd.UserName;
        }

        public void UpdateUntappdUserName(string untappdUserName)
        {
            if (Untappd.UserName.Equals(untappdUserName))
                return;

            Untappd.UserName = GetUntappdUserName(untappdUserName);
            UpdateUntappdUserNameEvent?.Invoke(Untappd.UserName);
        }

        public string GetUntappdProjectFileName()
        {
            return $"{Untappd.CreatedDate:yyyy_MMM_dd}_{Untappd.UserName}";
        }

        public string GetUntappdProjectPhotoFilesDirectory()
        {
            return $"{GetUntappdProjectFileName()}_Photos";
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
            string date = checkin.CreatedDate.ToString("yyyy-MMM-dd");
            string fullName = $"#{number} {date} {StringHelper.GetShortName(checkin.Beer.Name)}";
            return StringHelper.GeBreakForLongName(fullName, settingService.GetTreeItemNameMaxLength(), date.Length * 2);
        }

        private string GetUntappdUserName(string userName)
        {
            return String.IsNullOrEmpty(userName) ? settingService.GetDefaultUserName() : userName;
        }
    }
}