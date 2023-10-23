using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using UntappdViewer.UI.Controls.ViewModel;

namespace UntappdViewer.ViewModels.Controls
{
    public class CheckinViewModel
    {
        public string CheckinHeader { get; set; }

        public string CheckinUrl { get; set; }

        public bool VisibilityCheckinUrl
        {
            get { return !String.IsNullOrEmpty(CheckinUrl); }
        }

        public double? CheckinRating { get; set; }

        public bool VisibilityCheckinRating
        {
            get { return CheckinRating.HasValue; }
        }

        public string CheckinServingType { get; set; }


        public List<string> CheckinVenue { get; }

        public List<ImageItemViewModel>  Badges { get; }

        public bool VisibilityBadges
        {
            get { return Badges.Count > 0; }
        }

        public BitmapSource CheckinPhoto { get; set; }

        public bool VisibilityLikeBeer
        {
            get { return VisibilityCheckinRating && CheckinRating.Value >= DefaultValues.MinCheckinRatingByLikeBeer; }
        }

        public BeerViewModel BeerViewModel { get; set; }

        public CheckinViewModel()
        {
            CheckinVenue = new List<string>();
            Badges = new List<ImageItemViewModel>();
            BeerViewModel = new BeerViewModel(); 
        }

        public void AddVenue(string venue)
        {
            if (!String.IsNullOrEmpty(venue))
                CheckinVenue.Add(venue);
        }
    }
}