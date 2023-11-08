using System;
using System.Collections.Generic;
namespace UntappdViewer.ViewModels.Controls
{
    public class BeerViewModel
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

        public string Type { get; set; }

        public string ABV { get; set; }

        public bool VisibilityABV
        {
            get { return !String.IsNullOrEmpty(ABV); }
        }

        public string IBU { get; set; }

        public bool VisibilityIBU
        {
            get { return !String.IsNullOrEmpty(IBU); }
        }

        public double? Rating { get; set; }

        public bool VisibilityRating
        {
            get { return Rating.HasValue; }
        }

        public string Description { get; set; }

        public bool VisibilityDescription
        {
            get { return !String.IsNullOrEmpty(Description); }
        }

        public List<BreweryViewModel> BreweryViewModels { get; }

        public BeerViewModel()
        {
            BreweryViewModels = new List<BreweryViewModel>();
        }
    }
}