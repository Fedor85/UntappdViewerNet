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

        private string beerName;

        private string beerType;

        private string beerABV;

        private string beerIBU;

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

        public CheckinViewModel(IEventAggregator eventAggregator)
        {
            CheckinUrl = defaultUrl;
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
            BeerName = checkin.Beer.Name;
            BeerType = checkin.Beer.Type;
            BeerABV = checkin.Beer.ABV.ToString();
            BeerIBU = checkin.Beer.IBU.ToString();
        }

        private void Clear()
        {
            CheckinHeader = GetCheckinHeader(null);
            CheckinUrl = defaultUrl;
            BeerName = String.Empty;
            BeerType = String.Empty;
            BeerABV = String.Empty;
            BeerIBU = String.Empty;
        }

        private string GetCheckinHeader(DateTime? checkinCreatedDate)
        {
            return $"{Properties.Resources.Checkin}: {checkinCreatedDate}";
        }
    }
}