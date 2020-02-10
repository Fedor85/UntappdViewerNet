using System;
using System.IO;
using UntappdViewer.Infrastructure;
using UntappdViewer.Mappers;
using UntappdViewer.Models;

namespace UntappdViewer.Services
{
    public class UntappdService
    {
        public Untappd Untappd { get; set; }

        public event Action<Untappd> InitializeUntappd;

        public string FIlePath { get; private set; }

        public void Initialize(string userName, string filePath)
        {
            FIlePath = filePath;
            Untappd = new Untappd(String.IsNullOrEmpty(userName) ? "NoName" : userName);
            using (FileStream fileStream = File.OpenRead(filePath))
                Untappd.Checkins.AddRange(CheckinCSVMapper.GetCheckins(fileStream));

            if (InitializeUntappd != null)
                InitializeUntappd.Invoke(Untappd);
        }
    }
}