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

        void UpdateUntappdUserName(string untappdUserName);

        string GetUntappdProjectFileName();

        string GetFullUntappdProjectPhotoFilesDirectory();

        string GetFullCheckinPhotoFilePath(Checkin checkin);

        List<Checkin> GeCheckins(bool isUniqueCheckins = false);

        Checkin GetCheckin(long checkinId);

        string GetTreeViewCheckinDisplayName(Checkin checkin, int number);

        string GetUploadSavePhotoFileName(Checkin checkin);
    }
}
