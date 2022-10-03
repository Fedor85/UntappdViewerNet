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

namespace UntappdViewer.ViewModels
{
    public class StatisticsProjectViewModel : RegionManagerBaseModel
    {
        private IModuleManager moduleManager;

        private IUntappdService untappdService;

        private IEnumerable chekinRatingScore;

        private IEnumerable beerRatingScore;

        private IEnumerable beerTypeCount;

        private int maxYAxis;

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

        public int MaxYAxis
        {
            get { return maxYAxis; }
            set
            {
                SetProperty(ref maxYAxis, value);
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

            int chekinMaxCount = chekinRatingScore.Select(item => item.Value).Max();
            int beerMaxCount = beerRatingScore.Select(item => item.Value).Max();
            MaxYAxis = Math.Max(chekinMaxCount, beerMaxCount) + 100;

            ChekinRatingScore = chekinRatingScore;
            AverageChekinRating = Math.Round(MathHelper.GetAverageValue(ConverterHelper.KeyValuesToDictionary(chekinRatingScore)), 2);

            BeerRatingScore = beerRatingScore;
            AverageBeerRating = Math.Round(MathHelper.GetAverageValue(ConverterHelper.KeyValuesToDictionary(beerRatingScore)), 2);
        }

        private void SetBeerType()
        {
            BeerTypeCount = StatisticsCalculation.GetBeerTypeCount(untappdService.GetCheckins());
        }

        private void Exit()
        {
            moduleManager.LoadModule(typeof(UntappdModule).Name);
            ActivateView(RegionNames.MainRegion, typeof(Untappd));
        }
    }
}