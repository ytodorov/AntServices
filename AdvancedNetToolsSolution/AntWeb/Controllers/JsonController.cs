#region Using

using AntDal;
using AntDal.Entities;
using AutoMapper;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Telerik.Windows.Zip;
using TimeAgo;
using WebMarkupMin.Core;
using Newtonsoft.Json.Schema;

#endregion Using

namespace SmartAdminMvc.Controllers
{
    public class JsonController : BaseController
    {
        //[OutputCache(CacheProfile = "MyCache")]
        public ActionResult Index()
        {
            return View();

//            JObject person = JObject.Parse(@"{
//17  'name': 'James',
//18  'hobbies': ['.NET', 'Blogging', 'Reading', 'Xbox', 'LOLCATS']
//19}");

//            ValidationError err; err.
//            person.IsValid()

        }

        [HttpPost]
        public ActionResult Beautify(string json)
        {
            object deserializedObject = JsonConvert.DeserializeObject(json);
            var jsonBeautify = JsonConvert.SerializeObject(deserializedObject, Formatting.Indented);
            return Json(jsonBeautify);
        }


        [HttpPost]
        public ActionResult Download_Document(IEnumerable<HttpPostedFileBase> jsonUploads)
        {
            if (jsonUploads == null)
            {
                return File(new byte[0], "text/plain", "noFilesToDownload.txt");
            }
            var jsMinifier = new CrockfordJsMinifier();


            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (ZipArchive archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true, Encoding.UTF8))
                {
                    foreach (var jsUploadedFile in jsonUploads)
                    {
                        try
                        {
                            using (StreamReader reader = new StreamReader(jsUploadedFile.InputStream, Encoding.UTF8))
                            {
                                var json = reader.ReadToEnd();

                                object deserializedObject = JsonConvert.DeserializeObject(json);
                                var jsonMinified = JsonConvert.SerializeObject(deserializedObject, Formatting.Indented);

                                using (ZipArchiveEntry entry = archive.CreateEntry(jsUploadedFile.FileName))
                                {
                                    BinaryWriter writer = new BinaryWriter(entry.Open(), Encoding.UTF8);
                                    writer.Write(jsonMinified);
                                    writer.Flush();

                                    if (jsonUploads.Count() == 1)
                                    {
                                        using (MemoryStream ms1 = new MemoryStream())
                                        {
                                            using (StreamWriter wr1 = new StreamWriter(ms1))
                                            {
                                                wr1.Write(jsonMinified);
                                                wr1.Flush();
                                                return File(ms1.ToArray(), "application/json", entry.FullName);
                                            }

                                        }
                                    }
                                }
                            }
                        }

                        catch (Exception)
                        {

                        }
                    }



                }
                var arr = memoryStream.ToArray();
                return File(arr, "application/zip", "AllMinifiedJsonFiles.zip");
            }
        }
    }
}