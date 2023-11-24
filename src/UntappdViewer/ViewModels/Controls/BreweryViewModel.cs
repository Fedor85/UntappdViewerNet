using System;
using System.Collections.Generic;
using UntappdViewer.UI.Controls.Maps.BingMap.ViewModel;

namespace UntappdViewer.ViewModels.Controls
{
    public class BreweryViewModel
    {
        public string Url { get; set; }

        public bool VisibilityUrl
        {
            get { return !String.IsNullOrEmpty(Url); }
        }

        public string Name { get; set; }

        public string LabelPath { get; set; }

        public bool VisibilityLabel
        {
            get { return !String.IsNullOrEmpty(LabelPath); }
        }

        public string VenueCountryFlag { get; set; }

        public List<string> Venues { get; }

        public LocationItem LocationItem { get; set; }

        public bool VisibilityLocationItem
        {
            get { return LocationItem!= null; }
        }

        public BreweryViewModel()
        {
            Venues = new List<string>();
        }

        public void AddVenue(string venue)
        {
            if (!String.IsNullOrEmpty(venue))
                Venues.Add(venue);
        }
    }
}