using System;
using System.Threading.Tasks;
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

        private ISettingService settingService;

        private string credentialsProviderBing;

        private StatisticsProjectVM statisticsProject;

        public string CredentialsProviderBing
        {
            get { return credentialsProviderBing; }
            set { SetProperty(ref credentialsProviderBing, value); }
        }

        public StatisticsProjectVM StatisticsProject
        {
            get { return statisticsProject; }
            set { SetProperty(ref statisticsProject, value); }
        }

        public ICommand OkButtonCommand { get; }
 
        public StatisticsProjectViewModel(IRegionManager regionManager, IModuleManager moduleManager,
                                                                        ISettingService settingService,
                                                                        IStatisticsCalculation statisticsCalculation) : base(regionManager)
        {
            this.moduleManager = moduleManager;
            this.settingService = settingService;

            StatisticsProject = new StatisticsProjectVM(statisticsCalculation);
            OkButtonCommand = new DelegateCommand(Exit);
        }

        protected override void Activate()
        {
            base.Activate();
            FillStatisticsAsync(FillStatistics);
            CredentialsProviderBing = settingService.GetCredentialsProviderBing();
        }

        protected override void DeActivate()
        {
            base.DeActivate();
            CredentialsProviderBing = String.Empty;
            StatisticsProject.Clear();
        }

        private void FillStatistics()
        {
            FillStatisticsAsync(StatisticsProject.SetCountsPanel);
            FillStatisticsAsync(StatisticsProject.SetRatingScore);
            FillStatisticsAsync(StatisticsProject.SetDataCheckins);
            FillStatisticsAsync(StatisticsProject.SetAccumulateDataCheckins);
            FillStatisticsAsync(StatisticsProject.SetBeerType);
            FillStatisticsAsync(StatisticsProject.SetBeerCountry);
            FillStatisticsAsync(StatisticsProject.SetServingType);
            FillStatisticsAsync(StatisticsProject.SetIBUToABV);
            FillStatisticsAsync(StatisticsProject.SetIBUAndABVCount);
            FillStatisticsAsync(StatisticsProject.SetBeerCountryCountMap);
            FillStatisticsAsync(StatisticsProject.SetBeerCountryRatingMap);
        }

        private async void FillStatisticsAsync(Action action)
        {
            await Task.Run(() => action.Invoke());
        }

        private void Exit()
        {
            moduleManager.LoadModule(typeof(UntappdModule).Name);
            ActivateView(RegionNames.MainRegion, typeof(Untappd));
        }
    }
}