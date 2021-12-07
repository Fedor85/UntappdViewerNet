using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Spire.Xls;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Models;

namespace UntappdViewer.Reporting
{
    public class ReportingService: IReportingService
    {
        public void CreateAllCheckinsReport(List<Checkin> checkins, string outputPath)
        {
            IList<Checkin> checkinsSort = checkins.OrderBy(item => item.Beer.Brewery.Name).ThenBy(item => item.Beer.Name).ToList();
            Workbook workbook = new Workbook();
            Assembly assembly = Assembly.GetExecutingAssembly();
            string templateFileName = $@"{assembly.GetName().Name}.Resources.AllCheckinsTemplate.xlsx";
            using (Stream stream = assembly.GetManifestResourceStream(templateFileName))
                workbook.LoadFromStream(stream);

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
        }
    }
}