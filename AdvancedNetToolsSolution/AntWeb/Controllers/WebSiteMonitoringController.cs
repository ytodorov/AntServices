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
    public class WebSiteMonitoringController : BaseController
    {
        //[OutputCache(CacheProfile = "MyCache")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult WebSiteMonitoring_Read([DataSourceRequest]DataSourceRequest request)
        {
            using (AntDbContext context = new AntDbContext())
            {
                List<WebSiteMonitoringViewModel> res = Mapper.Map<List<WebSiteMonitoringViewModel>>(context.WebSiteMonitorings);
                return Json(res.ToDataSourceResult(request));
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult WebSiteMonitoring_Create([DataSourceRequest] DataSourceRequest request, WebSiteMonitoringViewModel webSiteMonitoring)
        {
            if (webSiteMonitoring != null && ModelState.IsValid)
            {
                using (AntDbContext context = new AntDbContext())
                {
                    WebSiteMonitoring res = Mapper.Map<WebSiteMonitoring>(webSiteMonitoring);
                    context.WebSiteMonitorings.Add(res);
                    context.SaveChanges();
                }
            }

            return Json(new[] { webSiteMonitoring }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult WebSiteMonitoring_Update([DataSourceRequest] DataSourceRequest request,
            [Bind(Prefix = "models")]IEnumerable<WebSiteMonitoringViewModel> products)
        {
            //if (products != null && ModelState.IsValid)
            //{
            //    foreach (var product in products)
            //    {
            //        productService.Update(product);
            //    }
            //}

            //return Json(products.ToDataSourceResult(request, ModelState));

            return null;
        }

    }
}