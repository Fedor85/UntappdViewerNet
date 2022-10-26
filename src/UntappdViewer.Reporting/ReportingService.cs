using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Spire.Xls;
using Spire.Xls.Core.Spreadsheet;
using UntappdViewer.Infrastructure;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Models;
using UntappdViewer.Models.Different;
using UntappdViewer.Utils;

namespace UntappdViewer.Reporting
{
    public class ReportingService: IReportingService
    {
        public async Task<string> CreateAllCheckinsReportrAsync(List<Checkin> checkins, string directory)
        {
            return await Task.Run(() => CreateAllCheckinsReport(checkins, directory));
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
            workbook.SaveToFile(outputPath);
            return outputPath;
        }

        public string CreateAllCheckinsReport(List<Checkin> checkins, string directory)
        {
            const string reportName = "AllCheckins";

            Workbook workbook = GetWorkbook(reportName);
            string outputPath = Path.Combine(directory, $"{reportName}.xlsx");

            IList<Checkin> checkinsSort = checkins.OrderBy(item => item.Beer.Brewery.Name).ThenBy(item => item.Beer.Name).ToList();
            Worksheet sheet = workbook.Worksheets[0];
            for (int i = 0; i < checkinsSort.Count; i++)
            {
                Checkin currentCheckin = checkinsSort[i];
                int indexRow = i + 2;

                sheet[indexRow, 1].Text = (i + 1).ToString();
                if (currentCheckin.Beer.Brewery.Venue != null)
                    sheet[indexRow, 2].Text = currentCheckin.Beer.Brewery.Venue.Country;

                sheet[indexRow, 3].Text = currentCheckin.Beer.Brewery.Name;
                sheet[indexRow, 4].Text = currentCheckin.Beer.Name;

                sheet[indexRow, 5].Text = currentCheckin.RatingScore.ToString();
                sheet[indexRow, 6].Text = currentCheckin.CreatedDate.ToString();

            }
            sheet.AutoFitColumn(3);
            sheet.AutoFitColumn(4);
            workbook.SaveToFile(outputPath);
            return outputPath;
        }

        private void FillBeerChekinRatingScore(Worksheet sheet, IStatisticsCalculation statisticsCalculation)
        {
            List<KeyValue<double, int>> beerRatingScore = statisticsCalculation.GetBeersRatingByCount();
            List<KeyValue<double, int>> chekinRatingScore = statisticsCalculation.GetChekinsRatingByCount();

            List<double> commonRatingScore = KeyValuesHelper.GetDistinctKeys(KeyValuesHelper.GetMerged(beerRatingScore, chekinRatingScore));
            commonRatingScore.Sort();

            SetValueByNameRanges(sheet.Workbook, "BeerAverageRating", Math.Round(MathHelper.GetAverageValue(KeyValuesHelper.KeyValuesToDictionary(beerRatingScore)), 2));
            SetValueByNameRanges(sheet.Workbook, "ChekinAverageRating", Math.Round(MathHelper.GetAverageValue(KeyValuesHelper.KeyValuesToDictionary(chekinRatingScore)), 2));
            sheet.CalculateAllValue();

            int indexRow = 2;
            foreach (double ratingScore in commonRatingScore)
            {
                sheet.ShowRow(indexRow);
                sheet[indexRow, 1].Value2 = ratingScore;
                sheet[indexRow, 2].Value2 = beerRatingScore.Where(item => MathHelper.DoubleCompare(item.Key, ratingScore)).Sum(item => item.Value);
                sheet[indexRow, 3].Value2 = chekinRatingScore.Where(item => MathHelper.DoubleCompare(item.Key, ratingScore)).Sum(item => item.Value);
                indexRow++;
            }
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