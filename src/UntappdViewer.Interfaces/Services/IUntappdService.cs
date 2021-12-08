using System;
using System.Collections.Generic;
using UntappdViewer.Models;

namespace UntappdViewer.Interfaces.Services
{
    public interface IUntappdService
    {
        Untappd Untappd { get; }

        Action<Untappd> InitializeUntappdEvent { get; set; }

        Action<string> UpdateUntappdUserNameEvent { get; set; }

        Action CleanUntappdEvent { get; set; }

        string FilePath { get; }

        void Create(string userName);

        void Initialize(string filePath, string userName = null);

        void CleanUpUntappd();

        bool IsUNTPProject();

        bool IsDirtyUntappd();

        void ResetСhanges();

        string GetUntappdUserName();

        void AddCheckins(List<Checkin> checkins);

        void UpdateUntappdUserName(string untappdUserName);

        string GetUntappdProjectFileName();

        string GetFileDataDirectory();

        string GetCheckinPhotoFilePath(Checkin checkin);

        string GetBeerLabelFilePath(Beer beer);

        string GetBreweryLabelFilePath(Brewery brewery);

        string GetBadgeImageFilePath(Badge badge);

        string GetReportsDirectory();

        List<Checkin> GetCheckins(bool isUniqueCheckins = false);

        Checkin GetCheckin(long checkinId);

        string GetTreeViewCheckinDisplayName(Checkin checkin, int number);

        string GetUploadSavePhotoFileName(Checkin checkin);
    }
}
