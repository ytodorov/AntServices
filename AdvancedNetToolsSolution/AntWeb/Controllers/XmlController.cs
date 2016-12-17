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
using System.Xml;
using SmartAdminMvc.Extensions;

#endregion Using

namespace SmartAdminMvc.Controllers
{
    public class XmlController : BaseController
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
        public ActionResult Download_Document(IEnumerable<HttpPostedFileBase> xmlUploads)
        {
            if (xmlUploads == null)
            {
                return File(new byte[0], "text/plain", "noFilesToDownload.txt");
            }
            var jsMinifier = new CrockfordJsMinifier();


            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (ZipArchive archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true, Encoding.UTF8))
                {
                    foreach (var xmlUploadedFile in xmlUploads)
                    {
                        try
                        {
                            using (StreamReader reader = new StreamReader(xmlUploadedFile.InputStream, Encoding.UTF8))
                            {
                                var xml = reader.ReadToEnd();

                                XmlDocument doc = new XmlDocument();
                                doc.LoadXml(xml);
                                string beautyfiedXml = doc.Beautify();



                                using (ZipArchiveEntry entry = archive.CreateEntry(xmlUploadedFile.FileName))
                                {
                                    BinaryWriter writer = new BinaryWriter(entry.Open(), Encoding.UTF8);
                                    writer.Write(beautyfiedXml);
                                    writer.Flush();

                                    if (xmlUploads.Count() == 1)
                                    {
                                        using (MemoryStream ms1 = new MemoryStream())
                                        {
                                            using (StreamWriter wr1 = new StreamWriter(ms1))
                                            {
                                                wr1.Write(beautyfiedXml);
                                                wr1.Flush();
                                                return File(ms1.ToArray(), "application/xml", entry.FullName);
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
                return File(arr, "application/zip", "AllBeautifiedXmlFiles.zip");
            }
        }
    }
}