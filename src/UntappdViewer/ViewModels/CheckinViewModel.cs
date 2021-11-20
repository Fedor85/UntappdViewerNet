using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Different;
using UntappdViewer.Events;
using UntappdViewer.Helpers;
using UntappdViewer.Infrastructure;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Models;
using UntappdViewer.Modules;
using UntappdViewer.Utils;

namespace UntappdViewer.ViewModels
{
    public class CheckinViewModel : LoadingBaseModel
    {
        private IUntappdService untappdService;

        private IInteractionRequestService interactionRequestService;

        private IEventAggregator eventAggregator;

        private IWebDownloader webDownloader;

        private string checkinHeader;

        private string checkinUrl;

        private bool visibilityCheckinRating;

        private double checkinRating;

        private string checkinVenueName;

        private bool visibilityCheckinVenueCountrySeporator;

        private string checkinVenueCountry;

        private bool visibilityСheckinVenueStateSeporator;

        private string checkinVenueState;

        private bool visibilityСheckinVenueCitySeporator;

        private string checkinVenueCity;

        private string checkinServingType;

        private string checkinPhotoPath;

        private string beerUrl;

        private string beerName;

        private string beerType;

        private string beerABV;

        private string beerIBU;

        private double beerRating;

        private string beerLabelPath;

        private bool visibilityBeerLabel;

        private string breweryUrl;

        private string breweryName;

        private string breweryVenueName;

        private bool visibilityBreweryVenueCountrySeporator;

        private string breweryVenueCountry;

        private bool visibilityBreweryVenueStateSeporator;

        private string breweryVenueState;

        private bool visibilityBreweryVenueCitySeporator;

        private string breweryVenueCity;

        private bool visibilityCheckinVenueLocation;

        private string breweryLabelPath;

        private bool visibilityBreweryLabel;

        private IList badges;

        public string CheckinHeader
        {
            get { return checkinHeader; }
            set
            {
                SetProperty(ref checkinHeader, value);
            }
        }

        public string CheckinUrl
        {
            get { return checkinUrl; }
            set
            {
                SetProperty(ref checkinUrl, value);
            }
        }

        public bool VisibilityCheckinRating
        {
            get { return visibilityCheckinRating; }
            set
            {
                SetProperty(ref visibilityCheckinRating, value);
            }
        }

        public double CheckinRating
        {
            get { return checkinRating; }
            set
            {
                SetProperty(ref checkinRating, value);
            }
        }

        public string CheckinVenueName
        {
            get { return checkinVenueName; }
            set
            {
                SetProperty(ref checkinVenueName, value);
                VisibilityCheckinVenueCountrySeporator = !String.IsNullOrEmpty(value);
            }
        }

        public bool VisibilityCheckinVenueCountrySeporator
        {
            get { return visibilityCheckinVenueCountrySeporator; }
            set
            {
                SetProperty(ref visibilityCheckinVenueCountrySeporator, value);
            }
        }

        public string CheckinVenueCountry
        {
            get { return checkinVenueCountry; }
            set
            {
                SetProperty(ref checkinVenueCountry, value);
            }
        }

        public bool VisibilityCheckinVenueStateSeporator
        {
            get { return visibilityСheckinVenueStateSeporator; }
            set
            {
                SetProperty(ref visibilityСheckinVenueStateSeporator, value);
            }
        }

        public string CheckinVenueState
        {
            get { return checkinVenueState; }
            set
            {
                SetProperty(ref checkinVenueState, value);
                VisibilityCheckinVenueStateSeporator = !String.IsNullOrEmpty(value);
            }
        }

        public bool VisibilityСheckinVenueCitySeporator
        {
            get { return visibilityСheckinVenueCitySeporator; }
            set
            {
                SetProperty(ref visibilityСheckinVenueCitySeporator, value);
            }
        }

        public string CheckinVenueCity
        {
            get { return checkinVenueCity; }
            set
            {
                SetProperty(ref checkinVenueCity, value);
                VisibilityСheckinVenueCitySeporator = !String.IsNullOrEmpty(value);
            }
        }

        public bool VisibilityCheckinVenueLocation
        {
            get { return visibilityCheckinVenueLocation; }
            set
            {
                SetProperty(ref visibilityCheckinVenueLocation, value);
            }
        }

        public string CheckinServingType
        {
            get { return checkinServingType; }
            set
            {
                SetProperty(ref checkinServingType, value);
            }
        }

        public string CheckinPhotoPath
        {
            get { return checkinPhotoPath; }
            set
            {
                SetProperty(ref checkinPhotoPath, value);
            }
        }

        public IList Badges
        {
            get { return badges; }
            set
            {
                SetProperty(ref badges, value);
            }
        }

        #region Beer

        public string BeerUrl
        {
            get { return beerUrl; }
            set
            {
                SetProperty(ref beerUrl, value);
            }
        }

        public string BeerName
        {
            get { return beerName; }
            set
            {
                SetProperty(ref beerName, value);
            }
        }

        public string BeerType
        {
            get { return beerType; }
            set
            {
                SetProperty(ref beerType, value);
            }
        }

        public string BeerABV
        {
            get { return beerABV; }
            set
            {
                SetProperty(ref beerABV, value);
            }
        }

        public string BeerIBU
        {
            get { return beerIBU; }
            set
            {
                SetProperty(ref beerIBU, value);
            }
        }

        public double BeerRating
        {
            get { return beerRating; }
            set
            {
                SetProperty(ref beerRating, value);
            }
        }

        public string BeerLabelPath
        {
            get { return beerLabelPath; }
            set
            {
                SetProperty(ref beerLabelPath, value);
                VisibilityBeerLabel = IsVisibilityLabel(value, DefautlValues.DefaultBeerLabelName);
            }
        }

        public bool VisibilityBeerLabel
        {
            get { return visibilityBeerLabel; }
            set
            {
                SetProperty(ref visibilityBeerLabel, value);
            }
        }

        #endregion

        #region Brewery

        public string BreweryUrl
        {
            get { return breweryUrl; }
            set
            {
                SetProperty(ref breweryUrl, value);
            }
        }

        public string BreweryName
        {
            get { return breweryName; }
            set
            {
                SetProperty(ref breweryName, value);
            }
        }

        public string BreweryVenueName
        {
            get { return breweryVenueName; }
            set
            {
                SetProperty(ref breweryVenueName, value);
                VisibilityBreweryVenueCountrySeporator = !String.IsNullOrEmpty(value);
            }
        }

        public bool VisibilityBreweryVenueCountrySeporator
        {
            get { return visibilityBreweryVenueCountrySeporator; }
            set
            {
                SetProperty(ref visibilityBreweryVenueCountrySeporator, value);
            }
        }

        public string BreweryVenueCountry
        {
            get { return breweryVenueCountry; }
            set
            {
                SetProperty(ref breweryVenueCountry, value);
            }
        }

        public bool VisibilityBreweryVenueStateSeporator
        {
            get { return visibilityBreweryVenueStateSeporator; }
            set
            {
                SetProperty(ref visibilityBreweryVenueStateSeporator, value);
            }
        }

        public string BreweryVenueState
        {
            get { return breweryVenueState; }
            set
            {
                SetProperty(ref breweryVenueState, value);
                VisibilityBreweryVenueStateSeporator = !String.IsNullOrEmpty(value);
            }
        }

        public bool VisibilityBreweryVenueCitySeporator
        {
            get { return visibilityBreweryVenueCitySeporator; }
            set
            {
                SetProperty(ref visibilityBreweryVenueCitySeporator, value);
            }
        }

        public string BreweryVenueCity
        {
            get { return breweryVenueCity; }
            set
            {
                SetProperty(ref breweryVenueCity, value);
                VisibilityBreweryVenueCitySeporator = !String.IsNullOrEmpty(value);
            }
        }

        public string BreweryLabelPath
        {
            get { return breweryLabelPath; }
            set
            {
                SetProperty(ref breweryLabelPath, value);
                VisibilityBreweryLabel = IsVisibilityLabel(value, DefautlValues.DefaultBreweryLabelName);
            }
        }

        public bool VisibilityBreweryLabel
        {
            get { return visibilityBreweryLabel; }
            set
            {
                SetProperty(ref visibilityBreweryLabel, value);
            }
        }

        #endregion

        public ICommand CheckinVenueLocationCommand { get; }


        public CheckinViewModel(IUntappdService untappdService, IInteractionRequestService interactionRequestService,
                                                                IWebDownloader webDownloader,
                                                                IEventAggregator eventAggregator,
                                                                IModuleManager moduleManager,
                                                                IRegionManager regionManager) : base(moduleManager, regionManager)
        {
            this.interactionRequestService = interactionRequestService;
            this.untappdService = untappdService;
            this.eventAggregator = eventAggregator;
            this.webDownloader = webDownloader;

            loadingModuleName = typeof(PhotoLoadingModule).Name;
            loadingRegionName = RegionNames.PhotoLoadingRegion;

            CheckinUrl = DefautlValues.DefaultUrl;
            BeerUrl = DefautlValues.DefaultUrl;
            BreweryUrl = DefautlValues.DefaultUrl;
            Badges = new List<ImageItemViewModel>();

            CheckinVenueLocationCommand  = new DelegateCommand(CheckinVenueLocation);
        }

        private void CheckinVenueLocation()
        {

        }

        protected override void Activate()
        {
            base.Activate();
            eventAggregator.GetEvent<ChekinUpdateEvent>().Subscribe(ChekinUpdate);
        }

        protected override void DeActivate()
        {
            base.DeActivate();
            Clear();
            eventAggregator.GetEvent<ChekinUpdateEvent>().Unsubscribe(ChekinUpdate);
        }

        private void ChekinUpdate(Checkin checkin)
        {
            if (checkin == null)
            {
                Clear();
                return;
            }
            CheckinHeader = GetCheckinHeader(checkin.CreatedDate);
            CheckinUrl = checkin.Url;
            if (checkin.RatingScore.HasValue)
            {
                VisibilityCheckinRating = true;
                CheckinRating = checkin.RatingScore.Value;
            }
            else
            {
                CheckinRating = 0;
                VisibilityCheckinRating = false;
            }
            CheckinVenueName = checkin.Venue.Name;
            CheckinVenueCountry = checkin.Venue.Country;
            CheckinVenueState = checkin.Venue.State;
            CheckinVenueCity = checkin.Venue.City;
            VisibilityCheckinVenueLocation = checkin.Venue.Latitude.HasValue && checkin.Venue.Longitude.HasValue;
            CheckinServingType = ConverterHelper.GetServingTypeImagePath(checkin.ServingType);
            UpdateCheckinPhoto(checkin);
            UpdateBadges(checkin.Badges);

            BeerUrl = checkin.Beer.Url;
            BeerName = checkin.Beer.Name;
            BeerType = checkin.Beer.Type;
            BeerABV = checkin.Beer.ABV.ToString();
            BeerIBU = GetBeerIBU(checkin.Beer.IBU);
            BeerRating = checkin.Beer.GlobalRatingScore;
            UpdateBeerLabel(checkin.Beer);

            BreweryUrl = checkin.Beer.Brewery.Url;
            BreweryName = checkin.Beer.Brewery.Name;
            BreweryVenueName = checkin.Beer.Brewery.Venue.Name;
            BreweryVenueCountry = checkin.Beer.Brewery.Venue.Country;
            BreweryVenueState = checkin.Beer.Brewery.Venue.State;
            BreweryVenueCity = checkin.Beer.Brewery.Venue.City;
            UpdateBreweryLabel(checkin.Beer.Brewery);
        }

        private void Clear()
        {
            CheckinHeader = GetCheckinHeader(null);
            CheckinUrl = DefautlValues.DefaultUrl;
            CheckinRating = 0;
            VisibilityCheckinRating = false;
            CheckinVenueName = String.Empty;
            CheckinVenueCountry = String.Empty;
            CheckinVenueState = String.Empty;
            CheckinVenueCity = String.Empty;
            VisibilityCheckinVenueLocation = false;
            CheckinServingType = DefautlValues.EmptyImage;
            CheckinPhotoPath = DefautlValues.EmptyImage;
            Badges = new List<ImageItemViewModel>();

            BeerUrl = DefautlValues.DefaultUrl;
            BeerName = String.Empty;
            BeerType = String.Empty;
            BeerABV = String.Empty;
            BeerIBU = String.Empty;
            BeerRating = 0;
            BeerLabelPath = DefautlValues.EmptyImage;

            BreweryUrl = DefautlValues.DefaultUrl;
            BreweryName = String.Empty;
            BreweryVenueCountry = String.Empty;
            BreweryVenueState= String.Empty;
            BreweryVenueCity = String.Empty;
            BreweryLabelPath = DefautlValues.EmptyImage;
        }

        private string GetCheckinHeader(DateTime? checkinCreatedDate)
        {
            return $"{Properties.Resources.Checkin}: {checkinCreatedDate}";
        }

        private string GetBeerIBU(double? beerIBU)
        {
            return beerIBU.HasValue ? beerIBU.Value.ToString() : "No IBU";
        }

        private void UpdateBeerLabel(Beer beer)
        {
            BeerLabelPath = DefautlValues.EmptyImage;
            if(!untappdService.IsUNTPProject() || String.IsNullOrEmpty(beer.LabelUrl))
                return;

            string labelPath = untappdService.GetBeerLabelFilePath(beer);
            if (!File.Exists(labelPath))
            {
                string directoryName = Path.GetDirectoryName(labelPath);
                if (!Directory.Exists(directoryName))
                    FileHelper.CreateDirectory(directoryName);

                webDownloader.DownloadFile(beer.LabelUrl, labelPath);
            }

            BeerLabelPath = labelPath;
        }

        private void UpdateBreweryLabel(Brewery brewery)
        {
            BreweryLabelPath = DefautlValues.EmptyImage;
            if (!untappdService.IsUNTPProject() || String.IsNullOrEmpty(brewery.LabelUrl))
                return;

            string labelPath = untappdService.GetBreweryLabelFilePath(brewery);
            if (!File.Exists(labelPath))
            {
                string directoryName = Path.GetDirectoryName(labelPath);
                if (!Directory.Exists(directoryName))
                    FileHelper.CreateDirectory(directoryName);

                webDownloader.DownloadFile(brewery.LabelUrl, labelPath);
            }

            BreweryLabelPath = labelPath;
        }

        private void UpdateCheckinPhoto(Checkin checkin)
        {
            CheckinPhotoPath = DefautlValues.DefaultCheckinPhotoPath;
            LoadingChangeActivity(true);
            UpdateCheckinPhotoAsunc(checkin);
        }

        private async void UpdateCheckinPhotoAsunc(Checkin checkin)
        {
            try
            {
                CheckinPhotoPath = await Task.Run(() => GetCheckinPhotoPath(checkin));
            }
            catch (Exception ex)
            {
                interactionRequestService.ShowError(Properties.Resources.Error, StringHelper.GetFullExceptionMessage(ex));
                CheckinPhotoPath = DefautlValues.DefaultCheckinPhotoPath;
            }
            LoadingChangeActivity(false);
        }

        private string GetCheckinPhotoPath(Checkin checkin)
        {
            if (!untappdService.IsUNTPProject())
                return DefautlValues.EmptyImage;

            if (String.IsNullOrEmpty(checkin.UrlPhoto))
                return DefautlValues.DefaultCheckinPhotoPath;

            string photoPath = untappdService.GetCheckinPhotoFilePath(checkin);
            if (!File.Exists(photoPath))
            {
                string directoryName = Path.GetDirectoryName(photoPath);
                if (!Directory.Exists(directoryName))
                    FileHelper.CreateDirectory(directoryName);

                webDownloader.DownloadFile(checkin.UrlPhoto, photoPath);
            }

            return photoPath;
        }

        private void UpdateBadges(List<Badge> checkinBadges)
        {
            Badges = new List<ImageItemViewModel>();
            if (!untappdService.IsUNTPProject() || checkinBadges == null || checkinBadges.Count == 0)
                return;

            List<ImageItemViewModel> currentBadges = new List<ImageItemViewModel>();
            foreach (Badge badge in checkinBadges.Where(item => !String.IsNullOrEmpty(item.ImageUrl)))
            {
                string badgeImagePath = GetBadgeImagePath(badge);
                currentBadges.Add(new ImageItemViewModel(badgeImagePath, $"{badge.Name}\n{badge.Description}"));
            }
            Badges = currentBadges;
        }

        private string GetBadgeImagePath(Badge badge)
        {
            string badgeImagePath = untappdService.GetBadgeImageFilePath(badge);
            if (!File.Exists(badgeImagePath))
            {
                string directoryName = Path.GetDirectoryName(badgeImagePath);
                if (!Directory.Exists(directoryName))
                    FileHelper.CreateDirectory(directoryName);

                webDownloader.DownloadFile(badge.ImageUrl, badgeImagePath);
            }
            return badgeImagePath;
        }

        private bool IsVisibilityLabel(string lableUrl, string defaultLabelName)
        {
            return !String.IsNullOrEmpty(lableUrl) &&
                   !lableUrl.Equals(DefautlValues.EmptyImage) &&
                   !Path.GetFileNameWithoutExtension(lableUrl).Equals(defaultLabelName);
        }
    }
}