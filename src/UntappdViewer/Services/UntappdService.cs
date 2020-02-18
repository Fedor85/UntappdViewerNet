using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Prism.Events;
using UntappdViewer.Events;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Mappers;
using UntappdViewer.Models;
using UntappdViewer.Utils;

namespace UntappdViewer.Services
{
    public class UntappdService
    {
        private ISettingService settingService;

        private IEventAggregator eventAggregator;

        private Untappd Untappd { get; set; }

        public string FIlePath { get; private set; }

        public UntappdService(ISettingService settingService, IEventAggregator eventAggregator)
        {
            this.settingService = settingService;
            this.eventAggregator = eventAggregator;
            Untappd = new Untappd(String.Empty);
        }

        public void Initialize(string userName, string filePath)
        {
            FIlePath = filePath;
            Untappd = new Untappd(String.IsNullOrEmpty(userName) ? settingService.GetDefaultUserName() : userName);
            using (FileStream fileStream = File.OpenRead(filePath))
                Untappd.AddCheckins(CheckinCSVMapper.GetCheckins(fileStream));

            Untappd.SortDataDescCheckins();
            eventAggregator.GetEvent<InitializeUntappdEvent>().Publish(Untappd);
        }

        public void CleanUpUntappd()
        {
            FIlePath = String.Empty;
            Untappd = new Untappd(String.Empty);
            eventAggregator.GetEvent<CleanUntappdEvent>().Publish();
        }

        public List<TreeViewItem> GeTreeViewItems(bool isUniqueCheckins = false)
        {
            List<TreeViewItem> treeViewItems = new List<TreeViewItem>();
            foreach (Checkin checkin in isUniqueCheckins ? Untappd.GetUniqueCheckins() : Untappd.Checkins)
                treeViewItems.Add(new TreeViewItem(checkin.Id, GetTreeViewCheckinDisplayName(checkin)));

            return treeViewItems;
        }

        public Checkin GetCheckin(long checkinId)
        {
            return Untappd.Checkins.FirstOrDefault(item => item.Id.Equals(checkinId));
        }

        private string GetTreeViewCheckinDisplayName(Checkin checkin)
        {
            string date = checkin.CreatedDate.ToString("yyyy-MMM-dd");
            string fullName = $"{date} {StringHelper.GetShortName(checkin.Beer.Name)}";
            return StringHelper.GeBreakForLongName(fullName, settingService.GetTreeItemNameMaxLength(), date.Length * 2);
        }
    }
}