#region Using

using AntDal;
using AntDal.Entities;
using AutoMapper;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SmartAdminMvc.Infrastructure;
using SmartAdminMvc.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.Caching;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Telerik.Windows.Documents.Common.FormatProviders;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx;
using Telerik.Windows.Documents.Flow.FormatProviders.Html;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.TextBased.Csv;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.TextBased.Txt;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Zip;
using TimeAgo;
using WebMarkupMin.Core;

#endregion Using

namespace SmartAdminMvc.Controllers
{
    public class SpreadController : BaseController
    {
        //[OutputCache(CacheProfile = "MyCache")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Download_Document(IEnumerable<HttpPostedFileBase> spreadUploads, string xlsx, string csv,
            string txt, string pdf)
        {
            if (spreadUploads == null)
            {
                return new EmptyResult();
            }

            if ("on".Equals(xlsx, StringComparison.InvariantCultureIgnoreCase))
            {
                xlsx = "xlsx";
            }
            if ("on".Equals(csv, StringComparison.InvariantCultureIgnoreCase))
            {
                csv = "csv";
            }
            if ("on".Equals(txt, StringComparison.InvariantCultureIgnoreCase))
            {
                txt = "txt";
            }
            if ("on".Equals(pdf, StringComparison.InvariantCultureIgnoreCase))
            {
                pdf = "pdf";
            }
            List<string> convertToTypes = new List<string>() { xlsx, csv, txt, pdf }.Where(t => !string.IsNullOrWhiteSpace(t)).ToList();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (ZipArchive archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true, null))
                {
                    foreach (var convertTo in convertToTypes)
                    {
                        foreach (var customDocument in spreadUploads)
                        {

                            IWorkbookFormatProvider fileFormatProvider = null;
                            IWorkbookFormatProvider convertFormatProvider = null;
                            Workbook document = null;
                            string mimeType = String.Empty;
                            string fileDownloadName = "{0}.{1}";

                            if (customDocument != null && Regex.IsMatch(Path.GetExtension(customDocument.FileName), ".xlsx|.csv|.txt"))
                            {
                                switch (Path.GetExtension(customDocument.FileName))
                                {
                                    case ".xlsx":
                                        fileFormatProvider = new XlsxFormatProvider();
                                        break;
                                    case ".csv":
                                        fileFormatProvider = new CsvFormatProvider();
                                        break;
                                    case ".txt":
                                        fileFormatProvider = new TxtFormatProvider();
                                        break;
                                    default:
                                        fileFormatProvider = null;
                                        break;
                                }

                                document = fileFormatProvider.Import(customDocument.InputStream);
                                fileDownloadName = string.Format(fileDownloadName, customDocument.FileName, convertTo);
                            }
                            else
                            {
                                fileFormatProvider = new XlsxFormatProvider();
                                string fileName = Server.MapPath("~/Content/web/spread/SampleDocument.xlsx");
                                using (FileStream input = new FileStream(fileName, FileMode.Open))
                                {
                                    document = fileFormatProvider.Import(input);
                                }

                                fileDownloadName = String.Format(fileDownloadName, "SampleDocument", convertTo);
                            }

                            switch (convertTo)
                            {
                                case "xlsx":
                                    convertFormatProvider = new XlsxFormatProvider();
                                    mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                    break;
                                case "csv":
                                    convertFormatProvider = new CsvFormatProvider();
                                    mimeType = "text/csv";
                                    break;
                                case "txt":
                                    convertFormatProvider = new TxtFormatProvider();
                                    mimeType = "text/plain";
                                    break;
                                case "pdf":
                                    convertFormatProvider = new PdfFormatProvider();
                                    mimeType = "application/pdf";
                                    break;
                                default:
                                    convertFormatProvider = new TxtFormatProvider();
                                    mimeType = "text/plain";
                                    break;
                            }

                            byte[] renderedBytes = null;

                            using (MemoryStream ms = new MemoryStream())
                            {
                                convertFormatProvider.Export(document, ms);
                                renderedBytes = ms.ToArray();
                            }
                            if (spreadUploads.Count() == 1)
                            {
                                return File(renderedBytes, mimeType, fileDownloadName);
                            }
                            else
                            {
                                if (archive.Entries.Any(e => e.Name.Equals(fileDownloadName, StringComparison.InvariantCultureIgnoreCase)))
                                {
                                    continue;
                                }
                                using (ZipArchiveEntry entry = archive.CreateEntry(fileDownloadName))
                                {
                                    BinaryWriter writer = new BinaryWriter(entry.Open());
                                    writer.Write(renderedBytes);
                                    writer.Flush();
                                }
                            }
                        }
                    }
                }

                var arr = memoryStream.ToArray();
                return File(arr, "application/zip", "AllFiles.zip");
            }
        }

    }
}