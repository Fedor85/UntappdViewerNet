using System;
using Prism.Events;
using UntappdViewer.Events;
using UntappdViewer.Models;

namespace UntappdViewer.ViewModels
{
    public class CheckinViewModel : ActiveAwareBaseModel
    {
        private const string defaultUrl = "http://schemas.microsoft.com/winfx/2006/xaml";

        private IEventAggregator eventAggregator;

        private string checkinHeader;

        private string checkinUrl;

        private bool vsibilityCheckinRating;

        private double checkinRating;

        private string beerUrl;

        private string beerName;

        private string beerType;

        private string beerABV;

        private string beerIBU;

        private double beerRating;

        private string breweryUrl;

        private string breweryName;

        private string breweryVenueCountry;

        private bool visibilityBreweryVenueStateSeporator;

        private string breweryVenueState;

        private string breweryVenueCity;

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

        public string BreweryVenueCity
        {
            get { return breweryVenueCity; }
            set
            {
                breweryVenueCity = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public CheckinViewModel(IEventAggregator eventAggregator)
        {
            CheckinUrl = defaultUrl;
            BeerUrl = defaultUrl;
            breweryUrl = defaultUrl;
            this.eventAggregator = eventAggregator;
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
                VsibilityCheckinRating = false;
            }

            BeerUrl = checkin.Beer.Url;
            BeerName = checkin.Beer.Name;
            BeerType = checkin.Beer.Type;
            BeerABV = checkin.Beer.ABV.ToString();
            BeerIBU = GetBeerIBU(checkin.Beer.IBU);
            BeerRating = checkin.Beer.GlobalRatingScore;

            BreweryUrl = checkin.Beer.Brewery.Url;
            BreweryName = checkin.Beer.Brewery.Name;
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