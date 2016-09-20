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
using Telerik.Windows.Documents.Flow.FormatProviders.Pdf;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf;
using Telerik.Windows.Documents.Flow.FormatProviders.Txt;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Zip;
using TimeAgo;
using WebMarkupMin.Core;

#endregion Using

namespace SmartAdminMvc.Controllers
{
    public class WordsProcessingController : BaseController
    {
        //[OutputCache(CacheProfile = "MyCache")]
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Download_Document(IEnumerable<HttpPostedFileBase> wordUploads, string docx, string rtf,
            string html, string txt, string pdf)
        {
            if (wordUploads == null)
            {
                return new EmptyResult();
            }

            if ("on".Equals(docx, StringComparison.InvariantCultureIgnoreCase))
            {
                docx = "docx";
            }
            if ("on".Equals(rtf, StringComparison.InvariantCultureIgnoreCase))
            {
                rtf = "rtf";
            }
            if ("on".Equals(html, StringComparison.InvariantCultureIgnoreCase))
            {
                html = "html";
            }
            if ("on".Equals(txt, StringComparison.InvariantCultureIgnoreCase))
            {
                txt = "txt";
            }
            if ("on".Equals(pdf, StringComparison.InvariantCultureIgnoreCase))
            {
                pdf = "pdf";
            }
            //string convertTo = string.Empty;
            List<string> convertToTypes = new List<string>() { docx, rtf, html, txt, pdf }.Where(t => !string.IsNullOrWhiteSpace(t)).ToList();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (ZipArchive archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true, null))
                {

                    foreach (var convertTo in convertToTypes)
                    {
                        foreach (var customDocument in wordUploads)
                        {
                            try
                            {
                                IFormatProvider<RadFlowDocument> fileFormatProvider = null;
                                IFormatProvider<RadFlowDocument> convertFormatProvider = null;
                                RadFlowDocument document = null;
                                string mimeType = string.Empty;
                                string fileDownloadName = "{0}.{1}";

                                if (customDocument != null && Regex.IsMatch(Path.GetExtension(customDocument.FileName), ".docx|.rtf|.html|.txt|.pdf"))
                                {
                                    switch (Path.GetExtension(customDocument.FileName))
                                    {
                                        case ".docx":
                                            fileFormatProvider = new DocxFormatProvider();
                                            break;
                                        case ".rtf":
                                            fileFormatProvider = new RtfFormatProvider();
                                            break;
                                        case ".html":
                                            fileFormatProvider = new HtmlFormatProvider();
                                            break;
                                        case ".txt":
                                            fileFormatProvider = new TxtFormatProvider();
                                            break;
                                        case ".pdf":
                                            fileFormatProvider = new PdfFormatProvider();
                                            break;
                                        default:
                                            fileFormatProvider = null;
                                            break;
                                    }

                                    document = fileFormatProvider.Import(customDocument.InputStream);
                                    fileDownloadName = String.Format(fileDownloadName, customDocument.FileName, convertTo);
                                }
                                else
                                {
                                    fileFormatProvider = new DocxFormatProvider();
                                    string fileName = Server.MapPath("~/Content/web/wordsprocessing/SampleDocument.docx");
                                    using (FileStream input = new FileStream(fileName, FileMode.Open))
                                    {
                                        document = fileFormatProvider.Import(input);
                                    }

                                    fileDownloadName = String.Format(fileDownloadName, "SampleDocument", convertTo);
                                }

                                switch (convertTo)
                                {
                                    case "docx":
                                        convertFormatProvider = new DocxFormatProvider();
                                        mimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                                        break;
                                    case "rtf":
                                        convertFormatProvider = new RtfFormatProvider();
                                        mimeType = "application/rtf";
                                        break;
                                    case "html":
                                        convertFormatProvider = new HtmlFormatProvider();
                                        mimeType = "text/html";
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
                                if (wordUploads.Count() == 1 && convertToTypes.Count == 1)
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
                            catch (Exception ex)
                            {

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