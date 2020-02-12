using System;
using System.IO;
using UntappdViewer.Mappers;
using UntappdViewer.Models;

namespace UntappdViewer.Services
{
    public class UntappdService
    {
        public Untappd Untappd { get; set; }

        public event Action<Untappd> InitializeUntappdEvent;

        public event Action<Untappd> UpdateUntappdEvent;

        public event Action CleanUntappdEvent;

        public string FIlePath { get; private set; }

        public void Initialize(string userName, string filePath)
        {
            FIlePath = filePath;
            Untappd = new Untappd(String.IsNullOrEmpty(userName) ? "NoName" : userName);
            using (FileStream fileStream = File.OpenRead(filePath))
                Untappd.Checkins.AddRange(CheckinCSVMapper.GetCheckins(fileStream));

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
                UpdateUntappdEvent.Invoke(Untappd);
        }
    }
}