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
    public class UntappdService : IUntappdService
    {
        public Untappd Untappd { get; private set; }

        public Action<Untappd> InitializeUntappdEvent { get; set; }

        public Action<string> UpdateUntappdUserNameEvent { get; set; }

        public Action CleanUntappdEvent { get; set; }

        public string FilePath { get; private set; }

        public UntappdService()
        {
            Untappd = new Untappd(String.Empty);
        }

        public void Create(string userName)
        {
            Untappd = new Untappd(userName);
            InitializeUntappdEvent?.Invoke(Untappd);
            UpdateUntappdUserNameEvent?.Invoke(Untappd.UserName);
            ResetСhanges();
        }

        public void Initialize(string filePath, string userName)
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
            FilePath = filePath;
            InitializeUntappdEvent?.Invoke(Untappd);
            UpdateUntappdUserNameEvent?.Invoke(Untappd.UserName);
            ResetСhanges();
        }

        private void InitializeToCSV(string filePath, string userName)
        {     
            Untappd = new Untappd(userName);
            using (FileStream fileStream = File.OpenRead(filePath))
                CheckinCSVMapper.InitializeCheckinsContainer(Untappd.CheckinsContainer, fileStream);

            SortDataDescCheckins();
        }

        private void InitializeToUNTP(string filePath)
        {
            try
            {
                Untappd = FileHelper.OpenFile<Untappd>(filePath);
                Untappd.CheckinsContainer.AttachedEvents();
            }
            catch (Exception e)
            {
                throw new ArgumentException(String.Format(Properties.Resources.ArgumentExceptionInitializeUntappd, $"{filePath}\n{e.Message}"));
            }

            if(!Untappd.IsValidVersion())
                throw new ArgumentException($"{Properties.Resources.ArgumentExceptioValidUNTPProjectVersion}\n{filePath}");
        }

        public void SortDataDescCheckins()
        {
            GetCheckins().Sort(SortCheckinsDataDesc);
        }

        public void CleanUpUntappd()
        {
            FilePath = String.Empty;
            Untappd = new Untappd(String.Empty);
            CleanUntappdEvent?.Invoke();
        }

        public bool IsUNTPProject()
        {
            return !String.IsNullOrEmpty(FilePath) && GetProjectExtensions() == Extensions.UNTP;
        }

        private string GetProjectExtensions()
        {
            switch (FileHelper.GetExtensionWihtoutPoint(FilePath))
            {
                case Extensions.CSV: return Extensions.CSV;
                case Extensions.UNTP: return Extensions.UNTP;
                default:
                    throw new ArgumentException(String.Format(Properties.Resources.ArgumentExceptionInitializeUntappd, FilePath));
            }
        }

        public bool IsDirtyUntappd()
        {
            return Untappd.IsСhanges();
        }

        public bool IsEmptyUntappd()
        {
            return Untappd.CheckinsContainer.Checkins.Count == 0;
        }

        public void ResetСhanges()
        {
            Untappd.ResetСhanges();
        }

        public string GetUntappdUserName()
        {
            return Untappd.UserName;
        }

        public void UpdateUntappdUserName(string untappdUserName)
        {
            if (Untappd.UserName.Equals(untappdUserName))
                return;

            Untappd.SetUserName(untappdUserName);
            UpdateUntappdUserNameEvent?.Invoke(Untappd.UserName);
        }

        public string GetUntappdProjectFileName()
        {
            return $"{Untappd.CreatedDate:yyyy_MMM_dd}_{Untappd.UserName}";
        }

        public string GetFileDataDirectory()
        {
            return Path.Combine(Path.GetDirectoryName(FilePath), $"{Path.GetFileNameWithoutExtension(FilePath)}_Data");
        }

        public string GetCheckinPhotoFilePath(Checkin checkin)
        {
            return GetImageFilePath(checkin.UrlPhoto, DefaultValues.CheckinPhotos);
        }

        public string GetBeerLabelFilePath(Beer beer)
        {
            return GetImageFilePath(beer.LabelUrl, DefaultValues.BeerLabels);
        }

        public string GetBreweryLabelFilePath(Brewery brewery)
        {
            return GetImageFilePath(brewery.LabelUrl, DefaultValues.BreweryLabels);
        }

        public string GetBadgeImageFilePath(Badge badge)
        {
            return GetImageFilePath(badge.ImageUrl, DefaultValues.BadgeImages);
        }

        private string GetImageFilePath(string url, string prefix)
        {
            if (String.IsNullOrEmpty(url))
                return String.Empty;

            string imageUrl = StringHelper.GetNormalizedJPGPath(url);
            return IsUNTPProject() ? Path.Combine(GetFileDataDirectory(), prefix, Path.GetFileName(imageUrl)) : FileHelper.GetTempFilePathByPath(imageUrl, prefix);
        }

        public string GetBadgeImageDirectory()
        {
            return Path.Combine(IsUNTPProject() ? GetFileDataDirectory() : FileHelper.TempDirectory, DefaultValues.BadgeImages);
        }

        public string GetReportsDirectory()
        {
            return Path.Combine(GetFileDataDirectory(), "Reports");
        }

        public void DownloadMediaFiles(IWebDownloader webDownloader, Checkin checkin)
        {
            DownloadFile(webDownloader, checkin.UrlPhoto, GetCheckinPhotoFilePath(checkin));
            DownloadFile(webDownloader, checkin.Beer.LabelUrl,GetBeerLabelFilePath(checkin.Beer));

            foreach (Brewery brewery in checkin.Beer.GetFullBreweries())
                DownloadFile(webDownloader, brewery.LabelUrl, GetBreweryLabelFilePath(brewery));

            foreach (Badge badge in checkin.Badges)
                DownloadFile(webDownloader, badge.ImageUrl, GetBadgeImageFilePath(badge));
        }

        public List<Checkin> GetCheckins(bool isUniqueCheckins = false)
        {
            return isUniqueCheckins ? GetUniqueCheckins() : Untappd.CheckinsContainer.Checkins;
        }

        public List<Checkin> GetCheckins(long breweryId)
        {
            return GetCheckins().Where(item => item.Beer.GetFullBreweries().Any(bw => bw.Id == breweryId)).ToList();
        }

        public Checkin GetCheckin(long checkinId)
        {
            return GetCheckins().FirstOrDefault(item => item.Id.Equals(checkinId));
        }

        public List<Beer> GetBeers()
        {
            return Untappd.CheckinsContainer.Beers;
        }

        public List<Brewery> GetBreweries()
        {
            return Untappd.CheckinsContainer.GetBreweries();
        }

        public List<Brewery> GetFullBreweries()
        {
            return Untappd.CheckinsContainer.GetFullBreweries();
        }

        public List<Badge> GetBadges()
        {
            List<Badge> badges = new List<Badge>();
            foreach (Checkin checkin in GetCheckins())
            {
                foreach (Badge badge in checkin.Badges)
                {
                    if (badges.All(item => item.Id != badge.Id))
                        badges.Add(badge);
                }
            }
            return badges;
        }

        public string GetTreeViewCheckinDisplayName(Checkin checkin, int number, int treeItemNameMaxLength)
        {
            string prefix = $"#{number} {checkin.CreatedDate.ToString("yyyy-MMM-dd")} ";
            string fullName = $"{prefix}{StringHelper.GetShortName(checkin.Beer.Name)}";
            return StringHelper.GeBreakForLongName(fullName, treeItemNameMaxLength, prefix.Length * 2 - 2);
        }

        public string GetUploadSavePhotoFileName(Checkin checkin)
        {
            return $"{checkin.CreatedDate.ToString("yyyy_MM_dd")}_{checkin.Id}.{FileHelper.GetExtensionWihtoutPoint(checkin.UrlPhoto)}";
        }

        private List<Checkin> GetUniqueCheckins()
        {
            return GetCheckins().GroupBy(item => item.Beer).Select(grp => grp.First()).ToList();
        }

        private int SortCheckinsDataDesc(Checkin x, Checkin y)
        {
            if (x.CreatedDate < y.CreatedDate)
                return 1;

            if (x.CreatedDate > y.CreatedDate)
                return -1;

            return 1;
        }

        private void DownloadFile(IWebDownloader webDownloader, string webPath, string filePath)
        {
            if (String.IsNullOrEmpty(webPath) || File.Exists(filePath))
                return;

            string directoryName = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryName))
                FileHelper.CreateDirectory(directoryName);

            webDownloader.DownloadToFile(webPath, filePath);
        }
    }
}