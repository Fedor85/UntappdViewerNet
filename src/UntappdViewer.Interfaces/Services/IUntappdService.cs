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

        void Initialize(string filePath, string userName);

        void SortDataDescCheckins();

        void CleanUpUntappd();

        bool IsUNTPProject();

        bool IsDirtyUntappd();

        bool IsEmptyUntappd();

        void ResetСhanges();

        string GetUntappdUserName();

        void UpdateUntappdUserName(string untappdUserName);

        string GetUntappdProjectFileName();

        string GetFileDataDirectory();

        string GetCheckinPhotoFilePath(Checkin checkin);

        string GetBeerLabelFilePath(Beer beer);

        string GetBreweryLabelFilePath(Brewery brewery);

        string GetBadgeImageFilePath(Badge badge);

        string GetBadgeImageDirectory();

        string GetReportsDirectory();

        void DownloadMediaFiles(IWebDownloader webDownloader, Checkin checkin);

        List<Checkin> GetCheckins(bool isUniqueCheckins = false);

        Checkin GetCheckin(long checkinId);

        List<Beer> GetBeers();

        List<Brewery> GetBreweries();

        List<Brewery> GetFullBreweries();

        List<Badge> GetBadges();

        string GetTreeViewCheckinDisplayName(Checkin checkin, int number, int treeItemNameMaxLength);

        string GetUploadSavePhotoFileName(Checkin checkin);
    }
}