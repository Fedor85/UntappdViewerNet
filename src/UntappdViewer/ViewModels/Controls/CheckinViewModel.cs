﻿using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using UntappdViewer.UI.Controls.ViewModel;

namespace UntappdViewer.ViewModels.Controls
{
    public class CheckinViewModel
    {
        public bool IsNotEmpty { get; private set; }

        public string Header { get; set; }

        public string Url { get; set; }

        public bool VisibilityUrl
        {
            get { return !String.IsNullOrEmpty(Url); }
        }

        public double? Rating { get; set; }

        public bool VisibilityRating
        {
            get { return Rating.HasValue; }
        }

        public string ServingType { get; set; }


        public List<string> Venues { get; }

        public List<ImageItemViewModel>  Badges { get; }

        public bool VisibilityBadges
        {
            get { return Badges.Count > 0; }
        }

        public BitmapSource Photo { get; set; }

        public bool VisibilityLikeBeer
        {
            get { return VisibilityRating && Rating.Value >= DefaultValues.MinCheckinRatingByLikeBeer; }
        }

        public BeerViewModel BeerViewModel { get; set; }

        public CheckinViewModel()
        {
            IsNotEmpty = true;
            Venues = new List<string>();
            Badges = new List<ImageItemViewModel>();
            BeerViewModel = new BeerViewModel(); 
        }

        public void AddVenue(string venue)
        {
            if (!String.IsNullOrEmpty(venue))
                Venues.Add(venue);
        }

        public static CheckinViewModel GetEmpty()
        {
            CheckinViewModel checkin = new CheckinViewModel();
            checkin.IsNotEmpty = false;
            checkin.BeerViewModel.BreweryViewModels.Add(new BreweryViewModel());
            return checkin;
        }
    }
}