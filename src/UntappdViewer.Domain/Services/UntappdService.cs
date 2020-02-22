using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UntappdViewer.Domain.Mappers;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Models;
using UntappdViewer.Utils;

namespace UntappdViewer.Domain.Services
{
    public class UntappdService
    {
        private ISettingService settingService;

        private Untappd untappd;

        public Action<Untappd> InitializeUntappdEvent { get; set; }

        public Action CleanUntappdEvent { get; set; }

        public string FIlePath { get; private set; }

        public UntappdService(ISettingService settingService)
        {
            this.settingService = settingService;
            untappd = new Untappd(String.Empty);
        }

        public void Initialize(string userName, string filePath)
        {
            FIlePath = filePath;
            untappd = new Untappd(String.IsNullOrEmpty(userName) ? settingService.GetDefaultUserName() : userName);
            using (FileStream fileStream = File.OpenRead(filePath))
                untappd.AddCheckins(CheckinCSVMapper.GetCheckins(fileStream));

            untappd.SortDataDescCheckins();
            InitializeUntappdEvent?.Invoke(untappd);
        }

        public void CleanUpUntappd()
        {
            FIlePath = String.Empty;
            untappd = new Untappd(String.Empty);
            CleanUntappdEvent?.Invoke();
        }

        public List<Checkin> GeCheckins(bool isUniqueCheckins = false)
        {
            return isUniqueCheckins ? untappd.GetUniqueCheckins() : untappd.Checkins;
        }

        public Checkin GetCheckin(long checkinId)
        {
            return untappd.Checkins.FirstOrDefault(item => item.Id.Equals(checkinId));
        }

        public string GetTreeViewCheckinDisplayName(Checkin checkin, int number)
        {
            string date = checkin.CreatedDate.ToString("yyyy-MMM-dd");
            string fullName = $"#{number} {date} {StringHelper.GetShortName(checkin.Beer.Name)}";
            return StringHelper.GeBreakForLongName(fullName, settingService.GetTreeItemNameMaxLength(), date.Length * 2);
        }
    }
}