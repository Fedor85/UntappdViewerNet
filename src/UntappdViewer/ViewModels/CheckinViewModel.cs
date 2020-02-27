using System;
using Prism.Events;
using UntappdViewer.Events;
using UntappdViewer.Models;

namespace UntappdViewer.ViewModels
{
    public class CheckinViewModel : ActiveAwareBaseModel
    {
        private IEventAggregator eventAggregator;

        private string checkinHeader;

        private string beerName;

        private string beerType;

        public string CheckinHeader
        {
            get { return checkinHeader; }
            set
            {
                checkinHeader = value;
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


        public CheckinViewModel(IEventAggregator eventAggregator)
        {
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
            BeerName = checkin.Beer.Name;
            BeerType = checkin.Beer.Type;
        }

        private void Clear()
        {
            CheckinHeader = GetCheckinHeader(null);
            BeerName = String.Empty;
            BeerType = String.Empty;
        }

        private string GetCheckinHeader(DateTime? checkinCreatedDate)
        {
            return $"{Properties.Resources.Checkin}: {checkinCreatedDate}";
        }
    }
}