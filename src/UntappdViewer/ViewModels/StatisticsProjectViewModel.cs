using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Prism.Commands;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Helpers;
using UntappdViewer.Infrastructure;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Models.Different;
using UntappdViewer.Modules;
using UntappdViewer.Utils;
using UntappdViewer.Views;

namespace UntappdViewer.ViewModels
{
    public class StatisticsProjectViewModel : RegionManagerBaseModel
    {
        private IModuleManager moduleManager;

        private IStatisticsCalculation statisticsCalculation;

        private IEnumerable chekinRatingScore;

        private IEnumerable beerRatingScore;

        private IEnumerable dateChekinsCount;

        private IEnumerable dateChekinsAccumulateCount;

        private IEnumerable beerTypeCount;

        private IEnumerable beerTypeRating;

        private IEnumerable beerCountryCount;

        private IEnumerable beerCountryCountMap;

        private IEnumerable beerCountryRating;

        private IEnumerable beerCountryRatingMap;

        private IEnumerable countryLanguagePack;

        private IEnumerable servingTypeCount;

        private IEnumerable servingTypeRating;

        private IEnumerable ibuToAbv;

        private IEnumerable aBVCount;

        private IEnumerable iBUCount;

        private int totalCheckinCount;

        private int uniqueCheckinCount;

        private int breweryCount;

        private int countryCount;

        private int maxYAxisRatingScore;

        private double minWidthChartDateChekins;

        private int maxXAxisBeerTypeCount;

        private int heightChartBeerType;

        private int maxXAxisBeerCountryCount;

        private int heightChartBeerCountry;

        private double averageChekinRating;

        private double averageBeerRating;

        private int totalDays;

        private double averageChekinsQuantity;

        private double averageUniqueChekinsQuantity;

        private int maxXAxisServingTypeCount;

        private int heightChartServingType;

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

        public IEnumerable DateChekinsCount
        {
            get { return dateChekinsCount; }
            set
            {
                SetProperty(ref dateChekinsCount, value);
            }
        }

        public IEnumerable DateChekinsAccumulateCount
        {
            get { return dateChekinsAccumulateCount; }
            set
            {
                SetProperty(ref dateChekinsAccumulateCount, value);
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

        public IEnumerable BeerCountryCountMap
        {
            get { return beerCountryCountMap; }
            set
            {
                SetProperty(ref beerCountryCountMap, value);
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

        public IEnumerable BeerCountryRatingMap
        {
            get { return beerCountryRatingMap; }
            set
            {
                SetProperty(ref beerCountryRatingMap, value);
            }
        }

        public IEnumerable CountryLanguagePack
        {
            get { return countryLanguagePack; }
            set
            {
                SetProperty(ref countryLanguagePack, value);
            }
        }

        public IEnumerable ServingTypeCount
        {
            get { return servingTypeCount; }
            set
            {
                SetProperty(ref servingTypeCount, value);
            }
        }

        public IEnumerable ServingTypeRating
        {
            get { return servingTypeRating; }
            set
            {
                SetProperty(ref servingTypeRating, value);
            }
        }

        public IEnumerable IBUToABV
        {
            get { return ibuToAbv; }
            set
            {
                SetProperty(ref ibuToAbv, value);
            }
        }

        public IEnumerable ABVCount
        {
            get { return aBVCount; }
            set
            {
                SetProperty(ref aBVCount, value);
            }
        }

        public IEnumerable IBUCount
        {
            get { return iBUCount; }
            set
            {
                SetProperty(ref iBUCount, value);
            }
        }

        public int TotalCheckinCount
        {
            get { return totalCheckinCount; }
            set
            {
                SetProperty(ref totalCheckinCount, value);
            }
        }

        public int UniqueCheckinCount
        {
            get { return uniqueCheckinCount; }
            set
            {
                SetProperty(ref uniqueCheckinCount, value);
            }
        }

        public int BreweryCount
        {
            get { return breweryCount; }
            set
            {
                SetProperty(ref breweryCount, value);
            }
        }

        public int CountryCount
        {
            get { return countryCount; }
            set
            {
                SetProperty(ref countryCount, value);
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

        public double MinWidthChartDateChekins
        {
            get { return minWidthChartDateChekins; }
            set
            {
                SetProperty(ref minWidthChartDateChekins, value);
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

        public int HeightChartBeerType
        {
            get { return heightChartBeerType; }
            set
            {
                SetProperty(ref heightChartBeerType, value);
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

        public int HeightChartBeerCountry
        {
            get { return heightChartBeerCountry; }
            set
            {
                SetProperty(ref heightChartBeerCountry, value);
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

        public int TotalDays
        {
            get { return totalDays; }
            set
            {
                SetProperty(ref totalDays, value);
            }
        }

        public double AverageChekinsQuantity
        {
            get { return averageChekinsQuantity; }
            set
            {
                SetProperty(ref averageChekinsQuantity, value);
            }
        }

        public double AverageUniqueChekinsQuantity
        {
            get { return averageUniqueChekinsQuantity; }
            set
            {
                SetProperty(ref averageUniqueChekinsQuantity, value);
            }
        }
        public int MaxXAxisServingTypeCount
        {
            get { return maxXAxisServingTypeCount; }
            set
            {
                SetProperty(ref maxXAxisServingTypeCount, value);
            }
        }

        public int HeightChartServingType
        {
            get { return heightChartServingType; }
            set
            {
                SetProperty(ref heightChartServingType, value);
            }
        }

        public StatisticsProjectViewModel(IRegionManager regionManager, IModuleManager moduleManager,
                                                                        IStatisticsCalculation statisticsCalculation) : base(regionManager)
        {
            this.moduleManager = moduleManager;
            this.statisticsCalculation = statisticsCalculation;

            OkButtonCommand = new DelegateCommand(Exit);
        }

        protected override void Activate()
        {
            base.Activate();
            SetCountsPanel();
            SetRatingScore();
            SetDataCheckins();
            SetBeerType();
            SetBeerCountry();
            SetServingType();
            SetIBUToABV();
        }

        protected override void DeActivate()
        {
            base.DeActivate();

            TotalCheckinCount =0;
            UniqueCheckinCount = 0;
            BreweryCount = 0;
            CountryCount = 0;

            MaxYAxisRatingScore = 0;
            ChekinRatingScore = null;
            AverageChekinRating = 0;
            BeerRatingScore = null;
            AverageBeerRating = 0;

            MinWidthChartDateChekins = 0;
            DateChekinsCount = null;
            TotalDays = 0;
            AverageChekinsQuantity = 0;
            AverageUniqueChekinsQuantity = 0;

            DateChekinsAccumulateCount = null;

            HeightChartBeerType = 0;
            MaxXAxisBeerTypeCount = 0;
            BeerTypeCount = null;
            BeerTypeRating = null;

            HeightChartBeerCountry = 0;
            MaxXAxisBeerCountryCount = 0;
            BeerCountryCount = null;
            BeerCountryRating = null;
            BeerCountryCountMap = null;
            BeerCountryRatingMap = null;
            CountryLanguagePack = null;

            MaxXAxisServingTypeCount = 0;
            ServingTypeCount = null;
            ServingTypeRating = null;

            IBUToABV = null;
            ABVCount = null;
            IBUCount = null;
        }

        private void SetCountsPanel()
        {
            TotalCheckinCount = statisticsCalculation.GetCheckinCount();
            UniqueCheckinCount = statisticsCalculation.GetCheckinCount(true);
            BreweryCount = statisticsCalculation.GetBreweryCount();
            CountryCount = statisticsCalculation.GetCountrysCount();
        }

        private void SetRatingScore()
        {
            List<KeyValue<double, int>> chekinRatingScore = statisticsCalculation.GetChekinsRatingByCount();
            List<KeyValue<double, int>> beerRatingScore = statisticsCalculation.GetBeersRatingByCount();

            int chekinMaxCount = chekinRatingScore.Count >0 ? chekinRatingScore.Select(item => item.Value).Max() : 0;
            int beerMaxCount = beerRatingScore.Count > 0 ? beerRatingScore.Select(item => item.Value).Max() : 0;
            MaxYAxisRatingScore = Math.Max(chekinMaxCount, beerMaxCount) + 100;

            ChekinRatingScore = chekinRatingScore;
            AverageChekinRating = Math.Round(MathHelper.GetAverageValue(KeyValuesHelper.KeyValuesToDictionary(chekinRatingScore)), 2);

            BeerRatingScore = beerRatingScore;
            AverageBeerRating = Math.Round(MathHelper.GetAverageValue(KeyValuesHelper.KeyValuesToDictionary(beerRatingScore)), 2);
        }

        private void SetDataCheckins()
        {
            List<KeyValue<string, int>> dateChekinsCount = statisticsCalculation.GetDateChekinsByCount();
            MinWidthChartDateChekins = MathHelper.GetCeilingByStep(dateChekinsCount.Count * 15, 100);
            DateChekinsCount = dateChekinsCount;

            TotalDays = statisticsCalculation.GetTotalDaysByNow();
            AverageChekinsQuantity = Math.Round(statisticsCalculation.GetAverageCountByNow(), 2);
            AverageUniqueChekinsQuantity = Math.Round(statisticsCalculation.GetAverageCountByNow(true), 2);

            DateChekinsAccumulateCount = KeyValuesHelper.GetAccumulateValues(dateChekinsCount);
        }

        private void SetBeerType()
        {
            List<KeyValue<string, List<long>>> beerTypeCheckinIds = statisticsCalculation.GetBeerTypesByCheckinIdsGroupByCount(statisticsCalculation.BeerTypeCountByOther);
            List <KeyValue<string, int>> beerTypeCount = KeyValuesHelper.GetListCount(beerTypeCheckinIds);

            HeightChartBeerType = GetHeightChart(beerTypeCount.Count);
            MaxXAxisBeerTypeCount = GetMaxXAxis(beerTypeCount.Count > 0 ? beerTypeCount.Max(item => item.Value): 0);

            BeerTypeCount = beerTypeCount;
            BeerTypeRating = statisticsCalculation.GetAverageRatingByCheckinIds(beerTypeCheckinIds);
        }

        private void SetBeerCountry()
        {
            List<KeyValue<string, List<long>>> beerCountryCheckinIds = statisticsCalculation.GetCountrysByCheckinIds();
            List<KeyValue<string, int>> beerCountryCount = KeyValuesHelper.GetListCount(beerCountryCheckinIds);

            HeightChartBeerCountry = GetHeightChart(beerCountryCount.Count);
            MaxXAxisBeerCountryCount = GetMaxXAxis(beerCountryCount.Count > 0 ? beerCountryCount.Max(item => item.Value) : 0);

            BeerCountryCount = beerCountryCount;
            List<KeyValue<string, double>> beerCountryRating = statisticsCalculation.GetAverageRatingByCheckinIds(beerCountryCheckinIds);
            BeerCountryRating = beerCountryRating;

            Dictionary<string, double> beerCountryNameCountMap = ConverterHelper.KeyValueToDirectory<string, int, double>(beerCountryCount);
            Dictionary<string, double> beerCountryCodeCountMap = ConverterHelper.ConvertCountryNameToCode(beerCountryNameCountMap);
            BeerCountryCountMap = beerCountryCodeCountMap;

            Dictionary<string, double> beerCountryNameRatingtMap = ConverterHelper.KeyValueToDirectory<string, double, double>(beerCountryRating);
            Dictionary<string, double> beerCountryCodeRatingtMap = ConverterHelper.ConvertCountryNameToCode(beerCountryNameRatingtMap);
            BeerCountryRatingMap = beerCountryCodeRatingtMap;

            CountryLanguagePack = ConverterHelper.GetCountryNameByCode(beerCountryNameCountMap.Keys.ToList());
        }

        private void SetServingType()
        {
            List<KeyValue<string, List<long>>> servingTypeByCheckinIds = statisticsCalculation.GetServingTypeByCheckinIds(statisticsCalculation.DefaultServingType);
            List<KeyValue<string, int>> servingTypeCount = KeyValuesHelper.GetListCount(servingTypeByCheckinIds);

            HeightChartServingType = GetHeightChart(servingTypeCount.Count);
            MaxXAxisServingTypeCount = GetMaxXAxis(servingTypeCount.Count > 0 ? servingTypeCount.Max(item => item.Value): 0);

            ServingTypeCount = servingTypeCount;
            ServingTypeRating = statisticsCalculation.GetAverageRatingByCheckinIds(servingTypeByCheckinIds);
        }

        private void SetIBUToABV()
        {
            IBUToABV = statisticsCalculation.GetABVToIBU();
            ABVCount = statisticsCalculation.GetRangeABVByCount(statisticsCalculation.RangeABVByCount, statisticsCalculation.MaxABVByCount);
            IBUCount = statisticsCalculation.GetRangeIBUByCount(statisticsCalculation.RangeIBUByCount, statisticsCalculation.MaxIBUByCount);
        }

        private int GetHeightChart(int count)
        {
            return count * 23 + 20;
        }
        private int GetMaxXAxis(int maxCount)
        {
            double percentage = MathHelper.GetPercentageOf(maxCount, 2);
            return maxCount + Convert.ToInt32(Math.Ceiling(percentage));
        }

        private void Exit()
        {
            moduleManager.LoadModule(typeof(UntappdModule).Name);
            ActivateView(RegionNames.MainRegion, typeof(Untappd));
        }
    }
}