using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
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

        private string checkinVenueCountrySeparator;

        private string checkinVenueCountry;

        private string checkinVenueStateySeparator;

        private string checkinVenueState;

        private string checkinVenueCitySeparator;

        private string checkinVenueCity;

        private string checkinServingType;

        private BitmapSource checkinPhoto;

        private string beerUrl;

        private string beerName;

        private string beerType;

        private string beerABV;

        private string beerIBU;

        private double beerRating;

        private string beerDescription;

        private BitmapSource beerLabel;

        private bool visibilityBeerLabel;

        private string breweryUrl;

        private string breweryName;

        private string breweryVenueName;

        private string breweryVenueCountrySeparator;

        private string breweryVenueCountry;

        private string breweryVenueStateSeparator;

        private string breweryVenueState;

        private string breweryVenueCitySeparator;

        private string breweryVenueCity;

        private bool visibilityCheckinVenueLocation;

        private BitmapSource breweryLabel;

        private bool visibilityBreweryLabel;

        private IList badges;

        private bool visibilityBadges;

        #region Checkin

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
            }
        }

        public string CheckinVenueCountrySeparator
        {
            get { return checkinVenueCountrySeparator; }
            set
            {
                SetProperty(ref checkinVenueCountrySeparator, value);
            }
        }

        public string CheckinVenueCountry
        {
            get { return checkinVenueCountry; }
            set
            {
                SetProperty(ref checkinVenueCountry, value);
                CheckinVenueCountrySeparator = !String.IsNullOrEmpty(CheckinVenueName) && !String.IsNullOrEmpty(value) ? DefautlValues.Separator : String.Empty;
            }
        }

        public string CheckinVenueStateySeparator
        {
            get { return checkinVenueStateySeparator; }
            set
            {
                SetProperty(ref checkinVenueStateySeparator, value);
            }
        }

        public string CheckinVenueState
        {
            get { return checkinVenueState; }
            set
            {
                SetProperty(ref checkinVenueState, value);
                CheckinVenueStateySeparator = (!String.IsNullOrEmpty(CheckinVenueName) || !String.IsNullOrEmpty(CheckinVenueCountry))
                                                        && !String.IsNullOrEmpty(value) ? DefautlValues.Separator : String.Empty;
            }
        }

        public string CheckinVenueCitySeparator
        {
            get { return checkinVenueCitySeparator; }
            set
            {
                SetProperty(ref checkinVenueCitySeparator, value);
            }
        }

        public string CheckinVenueCity
        {
            get { return checkinVenueCity; }
            set
            {
                SetProperty(ref checkinVenueCity, value);
                CheckinVenueCitySeparator = (!String.IsNullOrEmpty(CheckinVenueName) || !String.IsNullOrEmpty(CheckinVenueCountry) || !String.IsNullOrEmpty(CheckinVenueState))
                                                                                                    && !String.IsNullOrEmpty(value) ? DefautlValues.Separator : String.Empty;
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

        public BitmapSource CheckinPhoto
        {
            get { return checkinPhoto; }
            set
            {
                SetProperty(ref checkinPhoto, value);
            }
        }

        #endregion

        public IList Badges
        {
            get { return badges; }
            set
            {
                SetProperty(ref badges, value);
                VisibilityBadges = value != null && value.Count != 0;
            }
        }

        public bool VisibilityBadges
        {
            get { return visibilityBadges; }
            set
            {
                SetProperty(ref visibilityBadges, value);
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

        public string BeerDescription
        {
            get { return beerDescription; }
            set
            {
                SetProperty(ref beerDescription, value);
            }
        }

        public BitmapSource BeerLabel
        {
            get { return beerLabel; }
            set
            {
                SetProperty(ref beerLabel, value);
                VisibilityBeerLabel = value != null;
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
            }
        }

        public string BreweryVenueCountrySeparator
        {
            get { return breweryVenueCountrySeparator; }
            set
            {
                SetProperty(ref breweryVenueCountrySeparator, value);
            }
        }

        public string BreweryVenueCountry
        {
            get { return breweryVenueCountry; }
            set
            {
                SetProperty(ref breweryVenueCountry, value);
                BreweryVenueCountrySeparator = !String.IsNullOrEmpty(BreweryVenueName) && !String.IsNullOrEmpty(value) 
                                                                        ? DefautlValues.Separator : String.Empty;
            }
        }

        public string BreweryVenueStateSeparator
        {
            get { return breweryVenueStateSeparator; }
            set
            {
                SetProperty(ref breweryVenueStateSeparator, value);
            }
        }

        public string BreweryVenueState
        {
            get { return breweryVenueState; }
            set
            {
                SetProperty(ref breweryVenueState, value);
                BreweryVenueStateSeparator = (!String.IsNullOrEmpty(BreweryVenueName) || !String.IsNullOrEmpty(BreweryVenueCountry)) 
                                                        && !String.IsNullOrEmpty(value) ? DefautlValues.Separator : String.Empty;
            }
        }

        public string BreweryVenueCitySeparator
        {
            get { return breweryVenueCitySeparator; }
            set
            {
                SetProperty(ref breweryVenueCitySeparator, value);
            }
        }

        public string BreweryVenueCity
        {
            get { return breweryVenueCity; }
            set
            {
                SetProperty(ref breweryVenueCity, value);
                BreweryVenueCitySeparator = (!String.IsNullOrEmpty(BreweryVenueName) || !String.IsNullOrEmpty(BreweryVenueCountry) || !String.IsNullOrEmpty(BreweryVenueState))
                                            && !String.IsNullOrEmpty(value) ? DefautlValues.Separator : String.Empty;
            }
        }

        public BitmapSource BreweryLabel
        {
            get { return breweryLabel; }
            set
            {
                SetProperty(ref breweryLabel, value);
                VisibilityBreweryLabel = value != null;
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
            BeerDescription = GetBeerDescription(checkin.Beer.Description);
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
            CheckinPhoto = null;
            Badges = new List<ImageItemViewModel>();

            BeerUrl = DefautlValues.DefaultUrl;
            BeerName = String.Empty;
            BeerType = String.Empty;
            BeerABV = String.Empty;
            BeerIBU = String.Empty;
            BeerRating = 0;
            BeerDescription = String.Empty;
            BeerLabel = null;

            BreweryUrl = DefautlValues.DefaultUrl;
            BreweryName = String.Empty;
            BreweryVenueCountry = String.Empty;
            BreweryVenueState= String.Empty;
            BreweryVenueCity = String.Empty;
            BreweryLabel = null;
        }

        private string GetCheckinHeader(DateTime? checkinCreatedDate)
        {
            return $"{Properties.Resources.Checkin}: {checkinCreatedDate}";
        }

        private string GetBeerIBU(double? beerIBU)
        {
            return beerIBU.HasValue ? beerIBU.Value.ToString() : "No IBU";
        }

        private string GetBeerDescription(string description)
        {
            if (String.IsNullOrEmpty(description))
                return String.Empty;

            return StringHelper.GetSplitByLength(description, 50);
        }

        private void UpdateBeerLabel(Beer beer)
        {
            BeerLabel = null;
            if(!untappdService.IsUNTPProject() || String.IsNullOrEmpty(beer.LabelUrl))
                return;

            string labelPath = untappdService.GetBeerLabelFilePath(beer);
            if (Path.GetFileNameWithoutExtension(labelPath).Equals(DefautlValues.DefaultBeerLabelName))
                return;

            if (!File.Exists(labelPath))
            {
                string directoryName = Path.GetDirectoryName(labelPath);
                if (!Directory.Exists(directoryName))
                    FileHelper.CreateDirectory(directoryName);

                webDownloader.DownloadFile(beer.LabelUrl, labelPath);
            }

            BeerLabel = ImageConverter.GetBitmapSource(labelPath);
        }

        private void UpdateBreweryLabel(Brewery brewery)
        {
            BreweryLabel = null;
            if (!untappdService.IsUNTPProject() || String.IsNullOrEmpty(brewery.LabelUrl))
                return;

            string labelPath = untappdService.GetBreweryLabelFilePath(brewery);
            if (Path.GetFileNameWithoutExtension(labelPath).Equals(DefautlValues.DefaultBreweryLabelName))
                return;

            if (!File.Exists(labelPath))
            {
                string directoryName = Path.GetDirectoryName(labelPath);
                if (!Directory.Exists(directoryName))
                    FileHelper.CreateDirectory(directoryName);

                webDownloader.DownloadFile(brewery.LabelUrl, labelPath);
            }

            BreweryLabel = ImageConverter.GetBitmapSource(labelPath);
        }

        private void UpdateCheckinPhoto(Checkin checkin)
        {
            CheckinPhoto = null;

            if (String.IsNullOrEmpty(checkin.UrlPhoto))
            {
                CheckinPhoto = ImageConverter.GetBitmapSource(Properties.Resources.no_image_icon);
                return;
            }

            string photoPath = untappdService.IsUNTPProject()
                                ? untappdService.GetCheckinPhotoFilePath(checkin)
                                : FileHelper.GetTempFilePathByPath(checkin.UrlPhoto);

            if (File.Exists(photoPath))
            {
                CheckinPhoto = ImageConverter.GetBitmapSource(photoPath);
                return;
            }
            LoadingChangeActivity(true);
            UpdateCheckinPhotoAsunc(checkin, photoPath);
        }

        private async void UpdateCheckinPhotoAsunc(Checkin checkin, string photoPath)
        {
            try
            {
                CheckinPhoto = await Task.Run(() => GetCheckinPhoto(checkin, photoPath));
            }
            catch (Exception ex)
            {
                interactionRequestService.ShowError(Properties.Resources.Error, StringHelper.GetFullExceptionMessage(ex));
            }
            finally
            {
                LoadingChangeActivity(false);
            }
        }

        private BitmapSource GetCheckinPhoto(Checkin checkin, string photoPath)
        {
            string directoryName = Path.GetDirectoryName(photoPath);
            if (!Directory.Exists(directoryName))
                FileHelper.CreateDirectory(directoryName);

            webDownloader.DownloadFile(checkin.UrlPhoto, photoPath);
            BitmapSource photo = ImageConverter.GetBitmapSource(photoPath);
            photo.Freeze();
            return photo;
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
    }
}