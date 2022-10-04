using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Prism.Commands;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Domain;
using UntappdViewer.Domain.Models;
using UntappdViewer.Helpers;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Modules;
using UntappdViewer.Utils;
using UntappdViewer.Views;
using Checkin = UntappdViewer.Models.Checkin;

namespace UntappdViewer.ViewModels
{
    public class StatisticsProjectViewModel : RegionManagerBaseModel
    {
        private IModuleManager moduleManager;

        private IUntappdService untappdService;

        private IEnumerable chekinRatingScore;

        private IEnumerable beerRatingScore;

        private IEnumerable beerTypeCount;

        private IEnumerable beerTypeRating;

        private IEnumerable beerCountryCount;

        private IEnumerable beerCountryRating;

        private int maxYAxisRatingScore;

        private int maxXAxisBeerTypeCount;

        private double maxXAxisBeerTypeRating;

        private int maxXAxisBeerCountryCount;

        private double maxXAxisBeerCountryRating;

        private double averageChekinRating;

        private double averageBeerRating;

        public ICommand OkButtonCommand { get; }

        public IEnumerable ChekinRatingScore
        {
            get { return chekinRatingScore; }
            set
            {
                SetProperty(ref chekinRatingScore, value);
            }
        }

        public IEnumerable BeerRatingScore
        {
            get { return beerRatingScore; }
            set
            {
                SetProperty(ref beerRatingScore, value);
            }
        }

        public IEnumerable BeerTypeCount
        {
            get { return beerTypeCount; }
            set
            {
                SetProperty(ref beerTypeCount, value);
            }
        }

        public IEnumerable BeerTypeRating
        {
            get { return beerTypeRating; }
            set
            {
                SetProperty(ref beerTypeRating, value);
            }
        }

        public IEnumerable BeerCountryCount
        {
            get { return beerCountryCount; }
            set
            {
                SetProperty(ref beerCountryCount, value);
            }
        }

        public IEnumerable BeerCountryRating
        {
            get { return beerCountryRating; }
            set
            {
                SetProperty(ref beerCountryRating, value);
            }
        }

        public int MaxYAxisRatingScore
        {
            get { return maxYAxisRatingScore; }
            set
            {
                SetProperty(ref maxYAxisRatingScore, value);
            }
        }
        public int MaxXAxisBeerTypeCount
        {
            get { return maxXAxisBeerTypeCount; }
            set
            {
                SetProperty(ref maxXAxisBeerTypeCount, value);
            }
        }
        public double MaxXAxisBeerTypeRating
        {
            get { return maxXAxisBeerTypeRating; }
            set
            {
                SetProperty(ref maxXAxisBeerTypeRating, value);
            }
        }

        public int MaxXAxisBeerCountryCount
        {
            get { return maxXAxisBeerCountryCount; }
            set
            {
                SetProperty(ref maxXAxisBeerCountryCount, value);
            }
        }
        public double MaxXAxisBeerCountryRating
        {
            get { return maxXAxisBeerCountryRating; }
            set
            {
                SetProperty(ref maxXAxisBeerCountryRating, value);
            }
        }

        public double AverageChekinRating
        {
            get { return averageChekinRating; }
            set
            {
                SetProperty(ref averageChekinRating, value);
            }
        }

        public double AverageBeerRating
        {
            get { return averageBeerRating; }
            set
            {
                SetProperty(ref averageBeerRating, value);
            }
        }

        public StatisticsProjectViewModel(IRegionManager regionManager, IModuleManager moduleManager,
                                                                        IUntappdService untappdService) : base(regionManager)
        {
            this.moduleManager = moduleManager;
            this.untappdService = untappdService;

            OkButtonCommand = new DelegateCommand(Exit);
        }

        protected override void Activate()
        {
            base.Activate();
            SetRatingScore();
            SetBeerType();
            SetBeerCountry();
        }

        protected override void DeActivate()
        {
            base.DeActivate();
            ChekinRatingScore = null;
            AverageChekinRating = 0;
            BeerRatingScore = null;
            AverageBeerRating = 0;
            BeerTypeCount = null;
        }

        private void SetRatingScore()
        {
            List<KeyValue<double, int>> chekinRatingScore = StatisticsCalculation.GetChekinRatingScore(untappdService.GetCheckins());
            List<KeyValue<double, int>> beerRatingScore = StatisticsCalculation.GetBeerRatingScore(untappdService.GetBeers());

            int chekinMaxCount = chekinRatingScore.Count >0 ? chekinRatingScore.Select(item => item.Value).Max() : 0;
            int beerMaxCount = beerRatingScore.Count > 0 ? beerRatingScore.Select(item => item.Value).Max() : 0;
            MaxYAxisRatingScore = Math.Max(chekinMaxCount, beerMaxCount) + 100;

            ChekinRatingScore = chekinRatingScore;
            AverageChekinRating = Math.Round(MathHelper.GetAverageValue(ConverterHelper.KeyValuesToDictionary(chekinRatingScore)), 2);

            BeerRatingScore = beerRatingScore;
            AverageBeerRating = Math.Round(MathHelper.GetAverageValue(ConverterHelper.KeyValuesToDictionary(beerRatingScore)), 2);
        }

        private void SetBeerType()
        {
            List<Checkin> chekin = untappdService.GetCheckins();
            List<KeyValue<string, List<long>>> beerTypeCheckinIds = StatisticsCalculation.GetBeerTypeCheckinIdGroupByCount(chekin);

            List<KeyValue<string, int>> beerTypeCount = StatisticsCalculation.GetListCount(beerTypeCheckinIds);
            MaxXAxisBeerTypeCount = beerTypeCount.Count > 0 ? beerTypeCount.Max(item => item.Value) + 20 : 0;
            BeerTypeCount = beerTypeCount;

            List<KeyValue<string, double>> beerTypeRating = StatisticsCalculation.GetCheckinRatingByIds(chekin, beerTypeCheckinIds);
            MaxXAxisBeerTypeRating = beerTypeRating.Count > 0 ? beerTypeRating.Max(item => item.Value) + 0.1 : 0;
            BeerTypeRating = beerTypeRating;
        }

        private void SetBeerCountry()
        {
            List<Checkin> chekin = untappdService.GetCheckins();
            List<KeyValue<string, List<long>>> beerTypeCheckinIds = StatisticsCalculation.GetCountryCheckinIds(chekin);

            List<KeyValue<string, int>> beerCountryCount = StatisticsCalculation.GetListCount(beerTypeCheckinIds);
            MaxXAxisBeerCountryCount = beerCountryCount.Count > 0 ? beerCountryCount.Max(item => item.Value) + 20 : 0;
            BeerCountryCount = beerCountryCount;

            List<KeyValue<string, double>> beerCountryRating = StatisticsCalculation.GetCheckinRatingByIds(chekin, beerTypeCheckinIds);
            MaxXAxisBeerCountryRating = beerCountryRating.Count > 0 ? beerCountryRating.Max(item => item.Value) + 0.1 : 0;
            BeerCountryRating = beerCountryRating;
        }

        private void Exit()
        {
            moduleManager.LoadModule(typeof(UntappdModule).Name);
            ActivateView(RegionNames.MainRegion, typeof(Untappd));
        }
    }
}