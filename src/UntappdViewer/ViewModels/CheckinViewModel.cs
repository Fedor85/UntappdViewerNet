using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Domain;
using UntappdViewer.Domain.Services;
using UntappdViewer.Events;
using UntappdViewer.Infrastructure;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Models;
using UntappdViewer.Modules;
using UntappdViewer.Services;

namespace UntappdViewer.ViewModels
{
    public class CheckinViewModel : LoadingBaseModel
    {

        private UntappdService untappdService;

        private IInteractionRequestService interactionRequestService;

        private IEventAggregator eventAggregator;

        private IWebDownloader webDownloader;

        private string checkinHeader;

        private string checkinUrl;

        private bool vsibilityCheckinRating;

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

        public string CheckinHeader
        {
            get { return checkinHeader; }
            set
            {
                checkinHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CheckinHeader"));
            }
        }

        public string CheckinUrl
        {
            get { return checkinUrl; }
            set
            {
                checkinUrl = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CheckinUrl"));
            }
        }

        public bool VsibilityCheckinRating
        {
            get { return vsibilityCheckinRating; }
            set
            {
                vsibilityCheckinRating = value;
                OnPropertyChanged(new PropertyChangedEventArgs("VsibilityCheckinRating"));
            }
        }

        public double CheckinRating
        {
            get { return checkinRating; }
            set
            {
                checkinRating = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CheckinRating"));
            }
        }

        public string CheckinVenueName
        {
            get { return checkinVenueName; }
            set
            {
                checkinVenueName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CheckinVenueName"));
                VisibilityCheckinVenueCountrySeporator = !String.IsNullOrEmpty(value);
            }
        }

        public bool VisibilityCheckinVenueCountrySeporator
        {
            get { return visibilityCheckinVenueCountrySeporator; }
            set
            {
                visibilityCheckinVenueCountrySeporator = value;
                OnPropertyChanged(new PropertyChangedEventArgs("VisibilityCheckinVenueCountrySeporator"));
            }
        }

        public string CheckinVenueCountry
        {
            get { return checkinVenueCountry; }
            set
            {
                checkinVenueCountry = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CheckinVenueCountry"));
            }
        }

        public bool VisibilityCheckinVenueStateSeporator
        {
            get { return visibilityСheckinVenueStateSeporator; }
            set
            {
                visibilityСheckinVenueStateSeporator = value;
                OnPropertyChanged(new PropertyChangedEventArgs("VisibilityCheckinVenueStateSeporator"));
            }
        }

        public string CheckinVenueState
        {
            get { return checkinVenueState; }
            set
            {
                checkinVenueState = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CheckinVenueState"));
                VisibilityCheckinVenueStateSeporator = !String.IsNullOrEmpty(value);
            }
        }

        public bool VisibilityСheckinVenueCitySeporator
        {
            get { return visibilityСheckinVenueCitySeporator; }
            set
            {
                visibilityСheckinVenueCitySeporator = value;
                OnPropertyChanged(new PropertyChangedEventArgs("VisibilityСheckinVenueCitySeporator"));
            }
        }

        public string CheckinVenueCity
        {
            get { return checkinVenueCity; }
            set
            {
                checkinVenueCity = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CheckinVenueCity"));
                VisibilityСheckinVenueCitySeporator = !String.IsNullOrEmpty(value);
            }
        }

        public bool VisibilityCheckinVenueLocation
        {
            get { return visibilityCheckinVenueLocation; }
            set
            {
                visibilityCheckinVenueLocation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("VisibilityCheckinVenueLocation"));
            }
        }

        public string CheckinServingType
        {
            get { return checkinServingType; }
            set
            {
                checkinServingType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CheckinServingType"));
            }
        }

        public string CheckinPhotoPath
        {
            get { return checkinPhotoPath; }
            set
            {
                checkinPhotoPath = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CheckinPhotoPath"));
            }
        }

        #region Beer

        public string BeerUrl
        {
            get { return beerUrl; }
            set
            {
                beerUrl = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BeerUrl"));
            }
        }

        public string BeerName
        {
            get { return beerName; }
            set
            {
                beerName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BeerName"));
            }
        }

        public string BeerType
        {
            get { return beerType; }
            set
            {
                beerType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BeerType"));
            }
        }

        public string BeerABV
        {
            get { return beerABV; }
            set
            {
                beerABV = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BeerABV"));
            }
        }

        public string BeerIBU
        {
            get { return beerIBU; }
            set
            {
                beerIBU = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BeerIBU"));
            }
        }

        public double BeerRating
        {
            get { return beerRating; }
            set
            {
                beerRating = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BeerRating"));
            }
        }

        #endregion

        #region Brewery

        public string BreweryUrl
        {
            get { return breweryUrl; }
            set
            {
                breweryUrl = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BreweryUrl"));
            }
        }

        public string BreweryName
        {
            get { return breweryName; }
            set
            {
                breweryName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BreweryName"));
            }
        }

        public string BreweryVenueName
        {
            get { return breweryVenueName; }
            set
            {
                breweryVenueName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BreweryVenueName"));
                VisibilityBreweryVenueCountrySeporator = !String.IsNullOrEmpty(value);
            }
        }

        public bool VisibilityBreweryVenueCountrySeporator
        {
            get { return visibilityBreweryVenueCountrySeporator; }
            set
            {
                visibilityBreweryVenueCountrySeporator = value;
                OnPropertyChanged(new PropertyChangedEventArgs("VisibilityBreweryVenueCountrySeporator"));
            }
        }

        public string BreweryVenueCountry
        {
            get { return breweryVenueCountry; }
            set
            {
                breweryVenueCountry = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BreweryVenueCountry"));
            }
        }

        public bool VisibilityBreweryVenueStateSeporator
        {
            get { return visibilityBreweryVenueStateSeporator; }
            set
            {
                visibilityBreweryVenueStateSeporator = value;
                OnPropertyChanged(new PropertyChangedEventArgs("VisibilityBreweryVenueStateSeporator"));
            }
        }

        public string BreweryVenueState
        {
            get { return breweryVenueState; }
            set
            {
                breweryVenueState = value;      
                OnPropertyChanged(new PropertyChangedEventArgs("BreweryVenueState"));
                VisibilityBreweryVenueStateSeporator = !String.IsNullOrEmpty(value);
            }
        }

        public bool VisibilityBreweryVenueCitySeporator
        {
            get { return visibilityBreweryVenueCitySeporator; }
            set
            {
                visibilityBreweryVenueCitySeporator = value;
                OnPropertyChanged(new PropertyChangedEventArgs("VisibilityBreweryVenueCitySeporator"));
            }
        }

        public string BreweryVenueCity
        {
            get { return breweryVenueCity; }
            set
            {
                breweryVenueCity = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BreweryVenueCity"));
                VisibilityBreweryVenueCitySeporator = !String.IsNullOrEmpty(value);
            }
        }

        #endregion

        public ICommand CheckinVenueLocationCommand { get; }


        public CheckinViewModel(UntappdService untappdService, IInteractionRequestService interactionRequestService,
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
                VsibilityCheckinRating = true;
                CheckinRating = checkin.RatingScore.Value;
            }
            else
            {
                CheckinRating = 0;
                VsibilityCheckinRating = false;
            }
            CheckinVenueName = checkin.Venue.Name;
            CheckinVenueCountry = checkin.Venue.Country;
            CheckinVenueState = checkin.Venue.State;
            CheckinVenueCity = checkin.Venue.City;
            VisibilityCheckinVenueLocation = checkin.Venue.Latitude.HasValue && checkin.Venue.Longitude.HasValue;
            CheckinServingType = ConverterHelper.GetServingTypeImagePath(checkin.ServingType);
            UpdateCheckinPhoto(checkin);

            BeerUrl = checkin.Beer.Url;
            BeerName = checkin.Beer.Name;
            BeerType = checkin.Beer.Type;
            BeerABV = checkin.Beer.ABV.ToString();
            BeerIBU = GetBeerIBU(checkin.Beer.IBU);
            BeerRating = checkin.Beer.GlobalRatingScore;

            BreweryUrl = checkin.Beer.Brewery.Url;
            BreweryName = checkin.Beer.Brewery.Name;
            BreweryVenueName = checkin.Beer.Brewery.Venue.Name;
            BreweryVenueCountry = checkin.Beer.Brewery.Venue.Country;
            BreweryVenueState = checkin.Beer.Brewery.Venue.State;
            BreweryVenueCity = checkin.Beer.Brewery.Venue.City;
        }

        private void Clear()
        {
            CheckinHeader = GetCheckinHeader(null);
            CheckinUrl = DefautlValues.DefaultUrl;
            CheckinRating = 0;
            VsibilityCheckinRating = false;
            CheckinVenueName = String.Empty;
            CheckinVenueCountry = String.Empty;
            CheckinVenueState = String.Empty;
            CheckinVenueCity = String.Empty;
            VisibilityCheckinVenueLocation = false;
            CheckinServingType = DefautlValues.EmptyImage;
            CheckinPhotoPath = DefautlValues.EmptyImage;

            BeerUrl = DefautlValues.DefaultUrl;
            BeerName = String.Empty;
            BeerType = String.Empty;
            BeerABV = String.Empty;
            BeerIBU = String.Empty;
            BeerRating = 0;

            BreweryUrl = DefautlValues.DefaultUrl;
            BreweryName = String.Empty;
            BreweryVenueCountry = String.Empty;
            BreweryVenueState= String.Empty;
            BreweryVenueCity = String.Empty;
        }

        private string GetCheckinHeader(DateTime? checkinCreatedDate)
        {
            return $"{Properties.Resources.Checkin}: {checkinCreatedDate}";
        }

        private string GetBeerIBU(double? beerIBU)
        {
            return beerIBU.HasValue ? beerIBU.Value.ToString() : "No IBU";
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
                interactionRequestService.ShowError(Properties.Resources.Error, ex.Message);
                CheckinPhotoPath = DefautlValues.DefaultCheckinPhotoPath;
            }
            LoadingChangeActivity(false);
        }

        private string GetCheckinPhotoPath(Checkin checkin)
        {
            if (untappdService.GetProjectExtensions() == Extensions.CSV)
                return DefautlValues.EmptyImage;

            if (String.IsNullOrEmpty(checkin.UrlPhoto))
                return DefautlValues.DefaultCheckinPhotoPath;

            string photoPath = untappdService.GetFullCheckinPhotoFilePath(checkin);
            if (!File.Exists(photoPath))
            {
                string directoryName = Path.GetDirectoryName(photoPath);
                if (!Directory.Exists(directoryName))
                    FileHelper.CreateDirectory(directoryName);

                webDownloader.DownloadFile(checkin.UrlPhoto, photoPath);
            }

            return photoPath;
        }
    }
}