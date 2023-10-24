using System.Windows.Input;
using Prism.Commands;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Modules;
using UntappdViewer.Views;
using StatisticsProjectVM = UntappdViewer.ViewModels.Controls.StatisticsProjectViewModel;

namespace UntappdViewer.ViewModels
{
    public class StatisticsProjectViewModel : RegionManagerBaseModel
    {
        private IModuleManager moduleManager;

        private StatisticsProjectVM statisticsProject;

        public StatisticsProjectVM StatisticsProject
        {
            get { return statisticsProject; }
            set { SetProperty(ref statisticsProject, value); }
        }

        public ICommand OkButtonCommand { get; }
 
        public StatisticsProjectViewModel(IRegionManager regionManager, IModuleManager moduleManager,
                                                                        IStatisticsCalculation statisticsCalculation) : base(regionManager)
        {
            this.moduleManager = moduleManager;

            StatisticsProject = new StatisticsProjectVM(statisticsCalculation);
            OkButtonCommand = new DelegateCommand(Exit);
        }
        protected override void Activate()
        {
            base.Activate();

            StatisticsProject.SetCountsPanel();
            StatisticsProject.SetRatingScore();
            StatisticsProject.SetDataCheckins();
            StatisticsProject.SetBeerType();
            StatisticsProject.SetBeerCountry();
            StatisticsProject.SetServingType();
            StatisticsProject.SetIBUToABV();
        }

        protected override void DeActivate()
        {
            base.DeActivate();
            StatisticsProject.Clear();
        }

        private void Exit()
        {
            moduleManager.LoadModule(typeof(UntappdModule).Name);
            ActivateView(RegionNames.MainRegion, typeof(Untappd));
        }
    }
}