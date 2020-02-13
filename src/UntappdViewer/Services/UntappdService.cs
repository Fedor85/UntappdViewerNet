using System;
using System.Collections.Generic;
using System.IO;
using UntappdViewer.Different;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Mappers;
using UntappdViewer.Models;

namespace UntappdViewer.Services
{
    public class UntappdService
    {
        private ISettingService settingService;
        public Untappd Untappd { get; set; }

        public event Action<Untappd> InitializeUntappdEvent;

        public event Action UpdateUntappdEvent;

        public event Action CleanUntappdEvent;

        public string FIlePath { get; private set; }

        public UntappdService(ISettingService settingService)
        {
            this.settingService = settingService;
            Untappd = new Untappd(settingService.GetDefaultUserName());
        }

        public void Initialize(string userName, string filePath)
        {
            FIlePath = filePath;
            Untappd = new Untappd(String.IsNullOrEmpty(userName) ? settingService.GetDefaultUserName() : userName);
            using (FileStream fileStream = File.OpenRead(filePath))
                Untappd.Checkins.AddRange(CheckinCSVMapper.GetCheckins(fileStream));

            Untappd.SortDataDescCheckins();
            if (InitializeUntappdEvent != null)
                InitializeUntappdEvent.Invoke(Untappd);
        }

        public void CleanUpUntappd()
        {
            FIlePath = String.Empty;
            Untappd = new Untappd(String.Empty);

            if (CleanUntappdEvent != null)
                CleanUntappdEvent.Invoke();
        }

        public void RunUpdateUntappd()
        {
            if (UpdateUntappdEvent != null)
                UpdateUntappdEvent.Invoke();
        }

        public List<TreeViewItem> GeTreeViewItems()
        {
            List<TreeViewItem> treeViewItems = new List<TreeViewItem>();
            foreach (Checkin checkin in Untappd.Checkins)
                treeViewItems.Add(new TreeViewItem(checkin.Id, checkin.GetDisplayName()));

            return treeViewItems;
        }
    }
}