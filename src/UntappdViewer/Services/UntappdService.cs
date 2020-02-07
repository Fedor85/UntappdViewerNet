using System;
using UntappdViewer.Infrastructure;
using UntappdViewer.Mappers;
using UntappdViewer.Models;

namespace UntappdViewer.Services
{
    public class UntappdService
    {
        public Untappd Untappd { get; set; }

        public event Action<Untappd> InitializeUntappd;

        public void Initialize(string userName, string filePath)
        {
            Untappd = new Untappd(String.IsNullOrEmpty(userName) ? "NoName" : userName);
            Untappd.Checkins.AddRange(CheckinTextMapper.GetCheckins(FileHelper.GetTextForFile(filePath)));
            if (InitializeUntappd != null)
                InitializeUntappd.Invoke(Untappd);
        }
    }
}