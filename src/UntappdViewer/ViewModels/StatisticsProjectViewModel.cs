using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Prism.Commands;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Helpers;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Modules;
using UntappdViewer.Views;
using UntappdViewer.Views.Controls.ViewModel;

namespace UntappdViewer.ViewModels
{
    public class StatisticsProjectViewModel : RegionManagerBaseModel
    {
        private IModuleManager moduleManager;

        private IUntappdService untappdService;

        private IEnumerable ratingScore;

        private int maxYAxis;

        public ICommand OkButtonCommand { get; }

        public IEnumerable RatingScore
        {
            get { return ratingScore; }
            set
            {
                SetProperty(ref ratingScore, value);
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
        }

        protected override void DeActivate()
        {
            base.DeActivate();
            RatingScore = null;
        }

        private void SetRatingScore()
        {
            List<ChartViewModel<double, int>> chekinRatingScore = ConverterHelper.GetChekinRatingScore(untappdService.GetCheckins());
            int maxCount = chekinRatingScore.Select(item => item.Value).Max();
            MaxYAxis = maxCount + 100;
            RatingScore = chekinRatingScore;
        }


        private void Exit()
        {
            moduleManager.LoadModule(typeof(UntappdModule).Name);
            ActivateView(RegionNames.MainRegion, typeof(Untappd));
        }
    }
}