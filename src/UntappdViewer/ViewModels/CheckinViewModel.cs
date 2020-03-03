using System;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using UntappdViewer.Events;
using UntappdViewer.Models;

namespace UntappdViewer.ViewModels
{
    public class CheckinViewModel : ActiveAwareBaseModel
    {
        private const string defaultUrl = "http://schemas.microsoft.com/winfx/2006/xaml";

        private const string defaultCheckinPhotoPath = @"..\Resources\no-image-icon.png";

        private IEventAggregator eventAggregator;

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
                OnPropertyChanged();
            }
        }

        public string CheckinUrl
        {
            get { return checkinUrl; }
            set
            {
                checkinUrl = value;
                OnPropertyChanged();
            }
        }

        public bool VsibilityCheckinRating
        {
            get { return vsibilityCheckinRating; }
            set
            {
                vsibilityCheckinRating = value;
                OnPropertyChanged();
            }
        }

        public double CheckinRating
        {
            get { return checkinRating; }
            set
            {
                checkinRating = value;
                OnPropertyChanged();
            }
        }

        public string CheckinVenueName
        {
            get { return checkinVenueName; }
            set
            {
                checkinVenueName = value;
                OnPropertyChanged();
                VisibilityCheckinVenueCountrySeporator = !String.IsNullOrEmpty(value);
            }
        }

        public bool VisibilityCheckinVenueCountrySeporator
        {
            get { return visibilityCheckinVenueCountrySeporator; }
            set
            {
                visibilityCheckinVenueCountrySeporator = value;
                OnPropertyChanged();
            }
        }

        public string CheckinVenueCountry
        {
            get { return checkinVenueCountry; }
            set
            {
                checkinVenueCountry = value;
                OnPropertyChanged();
            }
        }

        public bool VisibilityCheckinVenueStateSeporator
        {
            get { return visibilityСheckinVenueStateSeporator; }
            set
            {
                visibilityСheckinVenueStateSeporator = value;
                OnPropertyChanged();
            }
        }

        public string CheckinVenueState
        {
            get { return checkinVenueState; }
            set
            {
                checkinVenueState = value;
                OnPropertyChanged();
                VisibilityCheckinVenueStateSeporator = !String.IsNullOrEmpty(value);
            }
        }

        public bool VisibilityСheckinVenueCitySeporator
        {
            get { return visibilityСheckinVenueCitySeporator; }
            set
            {
                visibilityСheckinVenueCitySeporator = value;
                OnPropertyChanged();
            }
        }

        public string CheckinVenueCity
        {
            get { return checkinVenueCity; }
            set
            {
                checkinVenueCity = value;
                OnPropertyChanged();
                VisibilityСheckinVenueCitySeporator = !String.IsNullOrEmpty(value);
            }
        }

        public bool VisibilityCheckinVenueLocation
        {
            get { return visibilityCheckinVenueLocation; }
            set
            {
                visibilityCheckinVenueLocation = value;
                OnPropertyChanged();
            }
        }

        public string CheckinPhotoPath
        {
            get { return checkinPhotoPath; }
            set
            {
                checkinPhotoPath = value;
                OnPropertyChanged();
            }
        }

        #region Beer

        public string BeerUrl
        {
            get { return beerUrl; }
            set
            {
                beerUrl = value;
                OnPropertyChanged();
            }
        }

        public string BeerName
        {
            get { return beerName; }
            set
            {
                beerName = value;
                OnPropertyChanged();
            }
        }

        public string BeerType
        {
            get { return beerType; }
            set
            {
                beerType = value;
                OnPropertyChanged();
            }
        }

        public string BeerABV
        {
            get { return beerABV; }
            set
            {
                beerABV = value;
                OnPropertyChanged();
            }
        }

        public string BeerIBU
        {
            get { return beerIBU; }
            set
            {
                beerIBU = value;
                OnPropertyChanged();
            }
        }

        public double BeerRating
        {
            get { return beerRating; }
            set
            {
                beerRating = value;
                OnPropertyChanged();
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
                OnPropertyChanged();
            }
        }

        public string BreweryName
        {
            get { return breweryName; }
            set
            {
                breweryName = value;
                OnPropertyChanged();
            }
        }

        public string BreweryVenueName
        {
            get { return breweryVenueName; }
            set
            {
                breweryVenueName = value;
                OnPropertyChanged();
                VisibilityBreweryVenueCountrySeporator = !String.IsNullOrEmpty(value);
            }
        }

        public bool VisibilityBreweryVenueCountrySeporator
        {
            get { return visibilityBreweryVenueCountrySeporator; }
            set
            {
                visibilityBreweryVenueCountrySeporator = value;
                OnPropertyChanged();
            }
        }

        public string BreweryVenueCountry
        {
            get { return breweryVenueCountry; }
            set
            {
                breweryVenueCountry = value;
                OnPropertyChanged();
            }
        }

        public bool VisibilityBreweryVenueStateSeporator
        {
            get { return visibilityBreweryVenueStateSeporator; }
            set
            {
                visibilityBreweryVenueStateSeporator = value;
                OnPropertyChanged();
            }
        }

        public string BreweryVenueState
        {
            get { return breweryVenueState; }
            set
            {
                breweryVenueState = value;      
                OnPropertyChanged();
                VisibilityBreweryVenueStateSeporator = !String.IsNullOrEmpty(value);
            }
        }

        public bool VisibilityBreweryVenueCitySeporator
        {
            get { return visibilityBreweryVenueCitySeporator; }
            set
            {
                visibilityBreweryVenueCitySeporator = value;
                OnPropertyChanged();
            }
        }

        public string BreweryVenueCity
        {
            get { return breweryVenueCity; }
            set
            {
                breweryVenueCity = value;
                OnPropertyChanged();
                VisibilityBreweryVenueCitySeporator = !String.IsNullOrEmpty(value);
            }
        }

        #endregion

        public ICommand CheckinVenueLocationCommand { get; }


        public CheckinViewModel(IEventAggregator eventAggregator)
        {
            CheckinUrl = defaultUrl;
            CheckinPhotoPath = defaultCheckinPhotoPath;

            BeerUrl = defaultUrl;
            breweryUrl = defaultUrl;
            this.eventAggregator = eventAggregator;
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
            //CheckinPhoto = @"D:\c30b5bc3e73764fc1831f714398d0bbf_raw.jpg";

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
            CheckinUrl = defaultUrl;
            CheckinRating = 0;
            VsibilityCheckinRating = false;
            CheckinVenueCountry = String.Empty;
            CheckinVenueState = String.Empty;
            CheckinVenueCity = String.Empty;
            VisibilityCheckinVenueLocation = false;
            CheckinPhotoPath = defaultCheckinPhotoPath;

            BeerUrl = defaultUrl;
            BeerName = String.Empty;
            BeerType = String.Empty;
            BeerABV = String.Empty;
            BeerIBU = String.Empty;
            BeerRating = 0;

            BreweryUrl = defaultUrl;
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
    }
}