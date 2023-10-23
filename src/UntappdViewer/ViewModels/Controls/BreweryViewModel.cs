using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

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

        public BitmapSource Label { get; set; }

        public bool VisibilityLabel
        {
            get { return Label != null; }           
        }

        public string VenueCountryFlag { get; set; }

        public List<string> Venues { get; }

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