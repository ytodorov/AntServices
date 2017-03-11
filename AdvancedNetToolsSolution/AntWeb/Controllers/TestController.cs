using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace SmartAdminMvc.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            Workbook workbook = new Workbook();
            IWorkbookFormatProvider fileFormatProvider = new XlsxFormatProvider();
            var fs = System.IO.File.Open(@"C:\Test\1.xlsx", System.IO.FileMode.Open);
            workbook = fileFormatProvider.Import(fs);
            
            foreach (Worksheet sheet in workbook.Sheets)
            {
                List<double> values = new List<double>();

                var maxRowCount = 0;
                for (int rowNumber = 0; rowNumber < 1000; rowNumber++)
                {
                    var cellRanges = sheet.Rows[rowNumber].CellRanges;

                    bool isRowEmpty = true;

                    for (int cellIndex = 0; cellIndex < 200; cellIndex++)
                    {
                        var theVal = sheet.Cells[rowNumber, cellIndex].GetValue().Value.RawValue;
                        if (!string.IsNullOrEmpty(theVal))
                        {
                            isRowEmpty = false;
                            break;
                        }
                    }
                    if (!isRowEmpty)
                    {
                        maxRowCount++;
                    }
                }

                var maxCellCount = 0;
                for (int cellIndex = 0; cellIndex < 200; cellIndex++)
                    
                {

                    bool isCellEmpty = true;

                    for (int rowNumber = 0; rowNumber < 1000; rowNumber++)
                    {
                        var theVal = sheet.Cells[rowNumber, cellIndex].GetValue().Value.RawValue;
                        if (!string.IsNullOrEmpty(theVal))
                        {
                            isCellEmpty = false;
                            break;
                        }
                    }
                    if (!isCellEmpty)
                    {
                        maxCellCount++;
                    }
                }


                for (int i = 0; i < maxCellCount; i++)
                {
                    values = GetCellValues(i, maxRowCount, sheet);
                }
               

                //for (int rowNumber = 0; rowNumber < maxRowCount; rowNumber++)
                //{
                //    var theVal = sheet.Cells[rowNumber, 2];
                //    double dValue;
                //    double.TryParse(theVal.GetValue().Value.RawValue, out dValue);

                //    values.Add(dValue);
                //}

                //values = values.OrderBy(d => d).ToList();
            }
            string fileName = @"C:\Test\SampleFile.xlsx";
            IWorkbookFormatProvider formatProvider = new XlsxFormatProvider();

            using (Stream output = new FileStream(fileName, FileMode.Create))
            {
                formatProvider.Export(workbook, output);
            }

            return View();
        }

        public List<double> GetCellValues(int cellNumber, int maxRows, Worksheet sheet)
        {
            List<double> values = new List<double>(maxRows);

            for (int i = 2; i < maxRows; i++)
            {
                var cellSelection = sheet.Cells[i, cellNumber];
                double dValue;


                double.TryParse(cellSelection.GetValue().Value.RawValue, out dValue);
                values.Add(dValue);
            }

            var maxTake = maxRows / 5; // 20%

            var topWorstValues = values.OrderBy(d => d).Take(maxTake).ToList();
            var topGoodValues = values.OrderByDescending(d => d).Take(maxTake).ToList();

            var columnTitle = sheet.Cells[0, cellNumber].GetValue().Value.RawValue?.ToUpperInvariant();
            if (columnTitle?.Contains("Unlike") == true)
            {
                topWorstValues = values.OrderByDescending(d => d).Take(maxTake).ToList();
                topGoodValues = values.OrderBy(d => d).Take(maxTake).ToList();
            }


            PatternFill worstPatternFill = new PatternFill(PatternType.Solid,
                    System.Windows.Media.Colors.LightPink, System.Windows.Media.Colors.Transparent);

            PatternFill goodPatternFill = new PatternFill(PatternType.Solid,
                   System.Windows.Media.Colors.LightSeaGreen, System.Windows.Media.Colors.Transparent);

            var distinctValuesCount = values.Distinct().Count();

            if (distinctValuesCount != 1)
            {
                for (int i = 2; i < maxRows; i++)
                {
                    var cellSelection = sheet.Cells[i, cellNumber];
                    double dValue;


                    double.TryParse(cellSelection.GetValue().Value.RawValue, out dValue);

                    if (topWorstValues.Contains(dValue))
                    {
                        cellSelection.SetFill(worstPatternFill);
                    }
                    else if (topGoodValues.Contains(dValue))
                    {
                        cellSelection.SetFill(goodPatternFill);
                    }
                }
            }


            return values;
        }

        //public List<string> LowerIsBetterColumnNames()
        //{
        //    List<string> list = new List<string>();
        //    list.Add("Daily Unlikes");
        //    list.Add("Daily Unlikes");

        //}
    }
}