using System;
using System.Windows.Media.Imaging;

namespace UntappdViewer.Views.Controls.ViewModels
{
    public class BeerViewModel
    {
        public string BeerUrl { get; set; }

        public bool VisibilityBeerUrl
        {
            get { return !String.IsNullOrEmpty(BeerUrl); }
        }

        public string BeerName { get; set; }

        public BitmapSource BeerLabel { get; set; }

        public bool VisibilityBeerLabel
        {
            get { return BeerLabel != null; }
        }

        public string BeerType { get; set; }

        public string BeerABV { get; set; }

        public string BeerIBU { get; set; }

        public double BeerRating { get; set; }

        public string BeerDescription { get; set; }

        public bool VisibilityBeerDescription
        {
            get { return !String.IsNullOrEmpty(BeerDescription); }
        }
    }
}