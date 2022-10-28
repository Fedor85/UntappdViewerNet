using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Spire.Xls;
using Spire.Xls.Core;
using Spire.Xls.Core.Spreadsheet;
using UntappdViewer.Infrastructure;
using UntappdViewer.Interfaces;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Models;
using UntappdViewer.Models.Different;
using UntappdViewer.Utils;
using MediaColor = System.Windows.Media.Color;

namespace UntappdViewer.Reporting
{
    public class ReportingService: IReportingService
    {
        private IGradientHelper pieGradientHelper { get; set; }

        public void SetPieGradien(IGradientHelper gradientHelper)
        {
            pieGradientHelper = gradientHelper;
        }

        public async Task<string> CreateAllCheckinsReportrAsync(List<Checkin> checkins, string directory)
        {
            return await Task.Run(() => CreateAllCheckinsReport(checkins, directory));
        }

        public async Task<string> CreateStatisticsReportAsync(IStatisticsCalculation statisticsCalculation, string directory)
        {
            return await Task.Run(() => CreateStatisticsReport(statisticsCalculation, directory));
        }

        public string CreateAllCheckinsReport(List<Checkin> checkins, string directory)
        {
            const string reportName = "AllCheckins";

            Workbook workbook = GetWorkbook(reportName);
            string outputPath = Path.Combine(directory, $"{reportName}.xlsx");

            IList<Checkin> checkinsSort = checkins.OrderBy(item => item.Beer.Brewery.Name).ThenBy(item => item.Beer.Name).ToList();
            Worksheet sheet = workbook.Worksheets[0];
            int indexRow = 5;
            for (int i = 0; i < checkinsSort.Count; i++)
            {
                Checkin currentCheckin = checkinsSort[i];

                sheet[indexRow, 1].Text = (i + 1).ToString();
                if (currentCheckin.Beer.Brewery.Venue != null)
                    sheet[indexRow, 2].Text = currentCheckin.Beer.Brewery.Venue.Country;

                sheet[indexRow, 3].Text = currentCheckin.Beer.Brewery.Name;
                sheet[indexRow, 4].Text = currentCheckin.Beer.Name;

                sheet[indexRow, 5].Text = currentCheckin.RatingScore.ToString();
                sheet[indexRow, 6].Text = currentCheckin.CreatedDate.ToString();
                indexRow++;
            }

            SetValueByNameRanges(sheet.Workbook, "Today", DateTime.Now);
            workbook.SaveToFile(outputPath);
            return outputPath;
        }

        public string CreateStatisticsReport(IStatisticsCalculation statisticsCalculation, string directory)
        {
            const string reportName = "Statistics";
            Workbook workbook = GetWorkbook(reportName);
            string outputPath = Path.Combine(directory, $"{reportName}.xlsx");

            FillBeerChekinRatingScore(workbook.Worksheets["BeerChekinRatingScore"], statisticsCalculation);
            FillChekinCountDate(workbook.Worksheets["ChekinCountDate"], statisticsCalculation);
            FillStyleCountRating(workbook.Worksheets["StyleCountRating"], statisticsCalculation);
            FillCountryCountRating(workbook.Worksheets["CountryCountRating"], statisticsCalculation);
            FillServingTypeCountRating(workbook.Worksheets["ServingTypeCountRating"], statisticsCalculation);
            FillIBUToABV(workbook.Worksheets["IBUToABV"], statisticsCalculation);

            Dictionary<string, int> pieCharts = new Dictionary<string, int>();
            int aBVCount = FillABVCount(workbook.Worksheets["ABVCount"], statisticsCalculation);
            if (aBVCount > 0)
                pieCharts.Add("PieABV", aBVCount);

            int iBUCount = FillIBUCount(workbook.Worksheets["IBUCount"], statisticsCalculation);
            if (aBVCount > 0)
                pieCharts.Add("PieIBU", iBUCount);

            Worksheet mainSheet = workbook.Worksheets["Statistics"];
            UpdatePieGradien(mainSheet, pieCharts);
            mainSheet.CalculateAllValue();

            workbook.SaveToFile(outputPath);
            return outputPath;
        }

        private void FillBeerChekinRatingScore(Worksheet sheet, IStatisticsCalculation statisticsCalculation)
        {
            List<KeyValue<double, int>> beerRatingScore = statisticsCalculation.GetBeersRatingByCount();
            List<KeyValue<double, int>> chekinRatingScore = statisticsCalculation.GetChekinsRatingByCount();

            List<double> commonRatingScore = KeyValuesHelper.GetDistinctKeys(KeyValuesHelper.GetMerged(beerRatingScore, chekinRatingScore));
            commonRatingScore.Sort();

            int indexRow = 2;
            foreach (double ratingScore in commonRatingScore)
            {
                sheet.ShowRow(indexRow);
                sheet[indexRow, 1].Value2 = ratingScore;
                sheet[indexRow, 2].Value2 = beerRatingScore.Where(item => MathHelper.DoubleCompare(item.Key, ratingScore)).Sum(item => item.Value);
                sheet[indexRow, 3].Value2 = chekinRatingScore.Where(item => MathHelper.DoubleCompare(item.Key, ratingScore)).Sum(item => item.Value);
                indexRow++;
            }

            SetValueByNameRanges(sheet.Workbook, "BeerAverageRating", Math.Round(MathHelper.GetAverageValue(KeyValuesHelper.KeyValuesToDictionary(beerRatingScore)), 2));
            SetValueByNameRanges(sheet.Workbook, "ChekinAverageRating", Math.Round(MathHelper.GetAverageValue(KeyValuesHelper.KeyValuesToDictionary(chekinRatingScore)), 2));
            sheet.CalculateAllValue();

            SetValueByNameRanges(sheet.Workbook, "TotalCheckinCount", statisticsCalculation.GetCheckinCount());
            SetValueByNameRanges(sheet.Workbook, "UniqueCheckinCount", statisticsCalculation.GetCheckinCount(true));
            SetValueByNameRanges(sheet.Workbook, "BreweryCount", statisticsCalculation.GetBreweryCount());
            SetValueByNameRanges(sheet.Workbook, "CountryCount", statisticsCalculation.GetCountrysCount());
        }

        private void FillChekinCountDate(Worksheet sheet, IStatisticsCalculation statisticsCalculation)
        {
            List<KeyValue<string, int>> dateChekinsCount = statisticsCalculation.GetDateChekinsByCount();
            List<KeyValue<string, int>> accumulateCountValues = KeyValuesHelper.GetAccumulateValues(dateChekinsCount);
            int indexRow = 2;
            foreach (KeyValue<string, int> dateChekinCount in dateChekinsCount)
            {
                sheet.ShowRow(indexRow);
                sheet[indexRow, 1].Value2 = dateChekinCount.Key;
                sheet[indexRow, 2].Value2 = dateChekinCount.Value;
                KeyValue<string, int> accumulateCountValue = accumulateCountValues.FirstOrDefault(item => item.Key.Equals(dateChekinCount.Key));
                if (accumulateCountValue != null)
                    sheet[indexRow, 3].Value2 = accumulateCountValue.Value;

                indexRow++;
            }

            SetValueByNameRanges(sheet.Workbook, "Today", DateTime.Now);
            SetValueByNameRanges(sheet.Workbook, "TotalDays", statisticsCalculation.GetTotalDaysByNow());
            SetValueByNameRanges(sheet.Workbook, "AverageChekinsQuantity", Math.Round(statisticsCalculation.GetAverageCountByNow(), 2));
            SetValueByNameRanges(sheet.Workbook, "AverageUniqueChekinsQuantity", Math.Round(statisticsCalculation.GetAverageCountByNow(true), 2));
            sheet.CalculateAllValue();
        }

        private void FillStyleCountRating(Worksheet sheet, IStatisticsCalculation statisticsCalculation)
        {
            List<KeyValue<string, List<long>>> beerTypeCheckinIds = statisticsCalculation.GetBeerTypesByCheckinIdsGroupByCount(statisticsCalculation.BeerTypeCountByOther);
            List<KeyValue<string, int>> beerTypesCount = KeyValuesHelper.GetListCount(beerTypeCheckinIds);
            List<KeyValue<string, double>> beerTypesRating = statisticsCalculation.GetAverageRatingByCheckinIds(beerTypeCheckinIds);
            int indexRow = 2;
            foreach (KeyValue<string, int> beerTypeCount in beerTypesCount)
            {
                sheet.ShowRow(indexRow);
                sheet[indexRow, 1].Value2 = beerTypeCount.Key;
                sheet[indexRow, 2].Value2 = beerTypeCount.Value;
                KeyValue<string, double> beerTypeRating = beerTypesRating.FirstOrDefault(item => item.Key.Equals(beerTypeCount.Key));
                if (beerTypeRating != null)
                    sheet[indexRow, 3].Value2 = beerTypeRating.Value;

                indexRow++;
            }
        }

        private void FillCountryCountRating(Worksheet sheet, IStatisticsCalculation statisticsCalculation)
        {
            List<KeyValue<string, List<long>>> beerCountryCheckinIds = statisticsCalculation.GetCountrysByCheckinIds();
            List<KeyValue<string, int>> beerCountrysCount = KeyValuesHelper.GetListCount(beerCountryCheckinIds);
            List<KeyValue<string, double>> beerCountrysRating = statisticsCalculation.GetAverageRatingByCheckinIds(beerCountryCheckinIds);
            int indexRow = 2;
            foreach (KeyValue<string, int> beerCountryCount in beerCountrysCount)
            {
                sheet.ShowRow(indexRow);
                sheet[indexRow, 1].Value2 = beerCountryCount.Key;
                sheet[indexRow, 2].Value2 = beerCountryCount.Value;
                KeyValue<string, double> beerTypeRating = beerCountrysRating.FirstOrDefault(item => item.Key.Equals(beerCountryCount.Key));
                if (beerTypeRating != null)
                    sheet[indexRow, 3].Value2 = beerTypeRating.Value;

                indexRow++;
            }
        }

        private void FillServingTypeCountRating(Worksheet sheet, IStatisticsCalculation statisticsCalculation)
        {
            List<KeyValue<string, List<long>>> servingTypeByCheckinIds = statisticsCalculation.GetServingTypeByCheckinIds(statisticsCalculation.DefaultServingType);
            List<KeyValue<string, int>> servingTypesCount = KeyValuesHelper.GetListCount(servingTypeByCheckinIds);
            List<KeyValue<string, double>> servingTypesRating = statisticsCalculation.GetAverageRatingByCheckinIds(servingTypeByCheckinIds);
            int indexRow = 2;
            foreach (KeyValue<string, int> servingTypeCount in servingTypesCount)
            {
                sheet.ShowRow(indexRow);
                sheet[indexRow, 1].Value2 = servingTypeCount.Key;
                sheet[indexRow, 2].Value2 = servingTypeCount.Value;
                KeyValue<string, double> beerTypeRating = servingTypesRating.FirstOrDefault(item => item.Key.Equals(servingTypeCount.Key));
                if (beerTypeRating != null)
                    sheet[indexRow, 3].Value2 = beerTypeRating.Value;

                indexRow++;
            }
        }

        private void FillIBUToABV(Worksheet sheet, IStatisticsCalculation statisticsCalculation)
        {
            List<KeyValue<double, double>> iBUToABV = statisticsCalculation.GetABVToIBU();
            int indexRow = 2;
            foreach (KeyValue<double, double> keyValue in iBUToABV)
            {
                sheet.ShowRow(indexRow);
                sheet[indexRow, 1].Value2 = keyValue.Key;
                sheet[indexRow, 2].Value2 = keyValue.Value;
                indexRow++;
            }
        }

        private int FillABVCount(Worksheet sheet, IStatisticsCalculation statisticsCalculation)
        {
            List<KeyValueParam<string, int>> aBVCount = statisticsCalculation.GetRangeABVByCount(statisticsCalculation.RangeABVByCount, statisticsCalculation.MaxABVByCount);
            int indexRow = 2;
            foreach (KeyValueParam<string, int> keyValue in aBVCount)
            {
                sheet.ShowRow(indexRow);
                sheet[indexRow, 1].Value2 = $"'{keyValue.Key}";
                sheet[indexRow, 2].Value2 = keyValue.Value;
                indexRow++;
            }
            return aBVCount.Count;
        }

        private int FillIBUCount(Worksheet sheet, IStatisticsCalculation statisticsCalculation)
        {
            List<KeyValueParam<string, int>> iBUCount = statisticsCalculation.GetRangeIBUByCount(statisticsCalculation.RangeIBUByCount, statisticsCalculation.MaxIBUByCount);
            int indexRow = 2;
            foreach (KeyValueParam<string, int> keyValue in iBUCount)
            {
                sheet.ShowRow(indexRow);
                sheet[indexRow, 1].Value2 = $"'{keyValue.Key}";
                sheet[indexRow, 2].Value2 = keyValue.Value;
                indexRow++;
            }
            return iBUCount.Count;
        }

        private void UpdatePieGradien(Worksheet sheet, Dictionary<string, int> pieCharts)
        {
            if (pieGradientHelper == null || !pieCharts.Any())
                return;

            foreach (KeyValuePair<string, int> pieChart in pieCharts.Where(item => item.Value > 0))
            {
                Chart chart = sheet.Charts.OfType<Chart>().FirstOrDefault(item => item.Name.Equals(pieChart.Key));
                if (chart == null)
                    continue;

                chart.RefreshChart();
                foreach (IChartSerie chartSeries in chart.Series)
                {
                    for (int i = 0; i <= pieChart.Value; i++)
                    {
                        MediaColor relativeColorv = pieGradientHelper.GetRelativeColor(i, pieChart.Value);
                        chartSeries.DataPoints[i].DataFormat.Fill.ForeColor = Color.FromArgb(relativeColorv.A, relativeColorv.R, relativeColorv.G, relativeColorv.B);
                    }
                }
            }
        }

        private void SetValueByNameRanges(Workbook workbook, string nameRange, object value)
        {
            XlsName range = workbook.NameRanges.GetByName(nameRange) as XlsName;
            if (range == null || range.Count == 0)
                return;

            range.CellList[0].Value2 = value;
        }

        private Workbook GetWorkbook(string reportName)
        {
            Workbook workbook = new Workbook();
            Assembly assembly = Assembly.GetExecutingAssembly();
            string templateFileName = $@"{assembly.GetName().Name}.Resources.{reportName}Template.xlsx";
            using (Stream stream = assembly.GetManifestResourceStream(templateFileName))
                workbook.LoadFromStream(stream);

            return workbook;
        }
    }
}