﻿using Prism.Events;
using UntappdViewer.Events;
using UntappdViewer.Models;

namespace UntappdViewer.ViewModels
{
    public class CheckinViewModel : ActiveAwareBaseModel
    {
        private IEventAggregator eventAggregator;

        private string checkinName;

        public string CheckinName
        {
            get { return checkinName; }
            set
            {
                checkinName = value;
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
            eventAggregator.GetEvent<ChekinUpdateEvent>().Unsubscribe(ChekinUpdate);
        }

        private void ChekinUpdate(Checkin checkin)
        {
            if (checkin != null)
                CheckinName = checkin.ToString();
        }
    }
}