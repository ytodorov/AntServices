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
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Telerik.Windows.Zip;
using TimeAgo;
using WebMarkupMin.Core;

#endregion Using

namespace SmartAdminMvc.Controllers
{
    public class JsController : BaseController
    {
        //[OutputCache(CacheProfile = "MyCache")]
        public ActionResult Index()
        {
            return View();
        }
             

        [HttpPost]
        public ActionResult Download_Document(IEnumerable<HttpPostedFileBase> jsUploads)
        {
            if (jsUploads == null)
            {
                return File(new byte[0], "text/plain", "noFilesToDownload.txt");
            }
            var jsMinifier = new CrockfordJsMinifier();


            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (ZipArchive archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true, null))
                {
                    foreach (var jsUploadedFile in jsUploads)
                    {
                        try
                        {
                            using (StreamReader reader = new StreamReader(jsUploadedFile.InputStream))
                            {
                                var css = reader.ReadToEnd();
                                var cssMinified = jsMinifier.Minify(css, false);


                                using (ZipArchiveEntry entry = archive.CreateEntry(jsUploadedFile.FileName.Replace(".js",string.Empty) + ".min.js"))
                                {
                                    BinaryWriter writer = new BinaryWriter(entry.Open());
                                    writer.Write(cssMinified.MinifiedContent);
                                    writer.Flush();
                                }
                            }
                        }

                        catch (Exception ex)
                        {

                        }
                    }



                }
                var arr = memoryStream.ToArray();
                return File(arr, "application/zip", "AllMinifiedCssFiles.zip");
            }
        }
    }
}