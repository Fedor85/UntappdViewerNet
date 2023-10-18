using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Prism.Commands;
using Prism.Events;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Events;
using UntappdViewer.Helpers;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Models;
using UntappdViewer.Modules;
using UntappdViewer.UI.Controls.ViewModel;
using UntappdViewer.UI.Helpers;
using UntappdViewer.Utils;
using UntappdViewer.Views.Controls.ViewModels;

namespace UntappdViewer.ViewModels
{
    public class CheckinViewModel : LoadingBaseModel
    {
        private IUntappdService untappdService;

        private IWebDownloader webDownloader;

        private string checkinHeader;

        private string checkinUrl;

        private bool visibilityCheckinRating;

        private double checkinRating;

        private string checkinVenueName;

        private string checkinVenueCountrySeparator;

        private string checkinVenueCountry;

        private string checkinVenueStateySeparator;

        private string checkinVenueState;

        private string checkinVenueCitySeparator;

        private string checkinVenueCity;

        private bool visibilityCheckinVenueLocation;

        private string checkinServingType;

        private BitmapSource checkinPhoto;

        private bool visibilityLikeBeer;

      
        private IList badges;

        private bool visibilityBadges;

        #region Checkin

        public string CheckinHeader
        {
            get { return checkinHeader; }
            set
            {
                SetProperty(ref checkinHeader, value);
            }
        }

        public string CheckinUrl
        {
            get { return checkinUrl; }
            set
            {
                SetProperty(ref checkinUrl, value);
            }
        }

        public bool VisibilityCheckinRating
        {
            get { return visibilityCheckinRating; }
            set
            {
                SetProperty(ref visibilityCheckinRating, value);
            }
        }

        public double CheckinRating
        {
            get { return checkinRating; }
            set
            {
                SetProperty(ref checkinRating, value);
            }
        }

        public string CheckinVenueName
        {
            get { return checkinVenueName; }
            set
            {
                SetProperty(ref checkinVenueName, value);
            }
        }

        public string CheckinVenueCountrySeparator
        {
            get { return checkinVenueCountrySeparator; }
            set
            {
                SetProperty(ref checkinVenueCountrySeparator, value);
            }
        }

        public string CheckinVenueCountry
        {
            get { return checkinVenueCountry; }
            set
            {
                SetProperty(ref checkinVenueCountry, value);
                CheckinVenueCountrySeparator = !String.IsNullOrEmpty(CheckinVenueName) && !String.IsNullOrEmpty(value) ? DefaultValues.Separator : String.Empty;
            }
        }

        public string CheckinVenueStateySeparator
        {
            get { return checkinVenueStateySeparator; }
            set
            {
                SetProperty(ref checkinVenueStateySeparator, value);
            }
        }

        public string CheckinVenueState
        {
            get { return checkinVenueState; }
            set
            {
                SetProperty(ref checkinVenueState, value);
                CheckinVenueStateySeparator = (!String.IsNullOrEmpty(CheckinVenueName) || !String.IsNullOrEmpty(CheckinVenueCountry))
                                                        && !String.IsNullOrEmpty(value) ? DefaultValues.Separator : String.Empty;
            }
        }

        public string CheckinVenueCitySeparator
        {
            get { return checkinVenueCitySeparator; }
            set
            {
                SetProperty(ref checkinVenueCitySeparator, value);
            }
        }

        public string CheckinVenueCity
        {
            get { return checkinVenueCity; }
            set
            {
                SetProperty(ref checkinVenueCity, value);
                CheckinVenueCitySeparator = (!String.IsNullOrEmpty(CheckinVenueName) || !String.IsNullOrEmpty(CheckinVenueCountry) || !String.IsNullOrEmpty(CheckinVenueState))
                                                                                                    && !String.IsNullOrEmpty(value) ? DefaultValues.Separator : String.Empty;
            }
        }

        public bool VisibilityCheckinVenueLocation
        {
            get { return visibilityCheckinVenueLocation; }
            set
            {
                SetProperty(ref visibilityCheckinVenueLocation, value);
            }
        }

        public string CheckinServingType
        {
            get { return checkinServingType; }
            set
            {
                SetProperty(ref checkinServingType, value);
            }
        }

        public BitmapSource CheckinPhoto
        {
            get { return checkinPhoto; }
            set
            {
                SetProperty(ref checkinPhoto, value);
            }
        }

        #endregion

        public IList Badges
        {
            get { return badges; }
            set
            {
                SetProperty(ref badges, value);
                VisibilityBadges = value != null && value.Count != 0;
            }
        }

        public bool VisibilityBadges
        {
            get { return visibilityBadges; }
            set
            {
                SetProperty(ref visibilityBadges, value);
            }
        }

        public bool VisibilityLikeBeer
        {
            get { return visibilityLikeBeer; }
            set
            {
                SetProperty(ref visibilityLikeBeer, value);
            }
        }

        private BeerViewModel beerViewModel;

        public BeerViewModel BeerViewModel
        {
            get { return beerViewModel; }
            set { SetProperty(ref beerViewModel, value); }
        }

        private IEnumerable<BreweryViewModel> breweryViewModels;

        public IEnumerable<BreweryViewModel> BreweryViewModels
        {
            get { return breweryViewModels; }
            set { SetProperty(ref breweryViewModels, value); }
        }

        public ICommand CheckinVenueLocationCommand { get; }


        public CheckinViewModel(IUntappdService untappdService, IWebDownloader webDownloader,
                                                                IEventAggregator eventAggregator,
                                                                IModuleManager moduleManager,
                                                                IRegionManager regionManager) : base(moduleManager, regionManager, eventAggregator)
        {
            this.untappdService = untappdService;
            this.webDownloader = webDownloader;

            loadingModuleName = typeof(PhotoLoadingModule).Name;
            loadingRegionName = RegionNames.PhotoLoadingRegion;

            CheckinUrl = DefaultValues.DefaultUrl;
            Badges = new List<ImageItemViewModel>();

            CheckinVenueLocationCommand  = new DelegateCommand(CheckinVenueLocation);
        }

        private void CheckinVenueLocation()
        {

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
            
            untappdService.DownloadMediaFiles(webDownloader, checkin);

            CheckinHeader = GetCheckinHeader(checkin.CreatedDate);
            CheckinUrl = checkin.Url;

            if (checkin.RatingScore.HasValue)
            {
                VisibilityCheckinRating = true;
                CheckinRating = checkin.RatingScore.Value;
                VisibilityLikeBeer = checkin.RatingScore.Value >= DefaultValues.MinCheckinRatingByLikeBeer;
            }
            else
            {
                CheckinRating = 0;
                VisibilityCheckinRating = false;
                VisibilityLikeBeer = false;
            }

            CheckinVenueName = checkin.Venue.Name;
            CheckinVenueCountry = checkin.Venue.Country;
            CheckinVenueState = checkin.Venue.State;
            CheckinVenueCity = checkin.Venue.City;
            VisibilityCheckinVenueLocation = checkin.Venue.Latitude.HasValue && checkin.Venue.Longitude.HasValue;
            CheckinServingType = ConverterHelper.GetServingTypeImagePath(checkin.ServingType);
            UpdateCheckinPhoto(checkin);
            UpdateBadges(checkin.Badges);

            BeerViewModel = GetBeerViewModel(checkin.Beer);
            BreweryViewModels = GetBreweryViewModels(checkin.Beer.GetFullBreweries());
        }

        private void Clear()
        {
            CheckinHeader = GetCheckinHeader(null);
            CheckinUrl = DefaultValues.DefaultUrl;
            CheckinRating = 0;
            VisibilityCheckinRating = false;
            CheckinVenueName = String.Empty;
            CheckinVenueCountry = String.Empty;
            CheckinVenueState = String.Empty;
            CheckinVenueCity = String.Empty;
            VisibilityCheckinVenueLocation = false;
            CheckinServingType = DefaultValues.EmptyImage;
            CheckinPhoto = null;
            Badges = new List<ImageItemViewModel>();
            VisibilityLikeBeer = false;

            BeerViewModel = null;
            BreweryViewModels = null;
        }

        private BeerViewModel GetBeerViewModel(Beer beer)
        {
            string labelPath = untappdService.GetBeerLabelFilePath(beer);
            BitmapSource label = null;
            if (!String.IsNullOrEmpty(beer.LabelUrl) && !Path.GetFileNameWithoutExtension(labelPath).Equals(DefaultValues.DefaultBeerLabelName))
                label = ImageConverter.GetBitmapSource(labelPath);

            return Mapper.GetBeerViewModel(beer, label);
        }

        private IEnumerable<BreweryViewModel> GetBreweryViewModels(List<Brewery> breweries)
        {
            List< BreweryViewModel> breweryViewModels = new List<BreweryViewModel>();
            foreach (Brewery brewery in breweries)
            {
                string labelPath = untappdService.GetBreweryLabelFilePath(brewery);
                BitmapSource label = null;
                if (!String.IsNullOrEmpty(labelPath) && !Path.GetFileNameWithoutExtension(labelPath).Equals(DefaultValues.DefaultBreweryLabelName))
                    label = ImageConverter.GetBitmapSource(labelPath);

                BreweryViewModel breweryViewModel = Mapper.GetBreweryViewModels(brewery, label);
                breweryViewModels.Add(breweryViewModel);
            }
            return breweryViewModels;
        }


        private string GetCheckinHeader(DateTime? checkinCreatedDate)
        {
            return $"{Properties.Resources.Checkin}: {checkinCreatedDate}";
        }

        private void UpdateCheckinPhoto(Checkin checkin)
        {
            string photoPath = untappdService.GetCheckinPhotoFilePath(checkin);

            CheckinPhoto = String.IsNullOrEmpty(photoPath) ? ImageConverter.GetBitmapSource(Properties.Resources.no_image_icon) : 
                                                             ImageConverter.GetBitmapSource(photoPath);
        }

        private void UpdateBadges(List<Badge> checkinBadges)
        {
            if (checkinBadges.Count == 0)
                return;

            List<ImageItemViewModel> currentBadges = new List<ImageItemViewModel>();
            foreach (Badge badge in checkinBadges)
            {
                string badgeImagePath = untappdService.GetBadgeImageFilePath(badge);
                if (!String.IsNullOrEmpty(badgeImagePath))
                    currentBadges.Add(new ImageItemViewModel(badgeImagePath, $"{badge.Name}\n{badge.Description}"));
            }
            Badges = currentBadges;
        }
    }
}