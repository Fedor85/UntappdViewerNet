using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace UntappdViewer.ViewModels.Controls
{
    public class BreweryViewModel
    {
        public string BreweryUrl { get; set; }

        public bool VisibilityBreweryUrl
        {
            get { return !String.IsNullOrEmpty(BreweryUrl); }
        }

        public string BreweryName { get; set; }

        public BitmapSource BreweryLabel { get; set; }

        public bool VisibilityBreweryLabel
        {
            get { return BreweryLabel != null; }           
        }

        public string BreweryVenueCountryFlag { get; set; }

        public List<string> BreweryVenue { get; }

        public BreweryViewModel()
        {
            BreweryVenue = new List<string>();
        }

        public void AddVenue(string venue)
        {
            if (!String.IsNullOrEmpty(venue))
                BreweryVenue.Add(venue);
        }
    }
}