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
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web.Mvc;
using TimeAgo;

#endregion Using

namespace SmartAdminMvc.Controllers
{
    public class BarcodeController : BaseController
    {
        [OutputCache(CacheProfile = "MyCache")]
        public ActionResult Index(int? id)
        {
            if (id.HasValue)
            {
                using (AntDbContext context = new AntDbContext())
                {
                    BarcodePermalink pp = context.BarcodePermalinks.FirstOrDefault(d => d.Id == id);
                    if (pp != null)
                    {
                        BarcodePermalinkViewModel ppvm = Mapper.Map<BarcodePermalinkViewModel>(pp);
                        return View(model: ppvm);
                    }
                }
            }
            return View();
        }


        [HttpPost]
        public ActionResult GenerateId(TracerouteRequestViewModel prvm, AntDbContext context)
        {
            string barcodeValue = prvm.Ip;
            bool saveInHistory = prvm.ShowInHistory.GetValueOrDefault();
                   
            var barcodePermalink = new BarcodePermalink();
            barcodePermalink.Value = barcodeValue;
            barcodePermalink.ShowInHistory = prvm.ShowInHistory;
            barcodePermalink.UserCreated = Request?.UserHostAddress;
            barcodePermalink.UserModified = Request?.UserHostAddress;
            barcodePermalink.DateCreated = DateTime.Now;
            barcodePermalink.DateModified = DateTime.Now;

            context.BarcodePermalinks.Add(barcodePermalink);
            context.SaveChanges();

            return Json(barcodePermalink.Id);
        }

        

        public ActionResult ReadBarcodePermalinkValues([DataSourceRequest] DataSourceRequest request)
        {
            var list = new List<AddressHistoryViewModel>();
            using (AntDbContext context = new AntDbContext())
            {
                var barcodePermalinks = context.BarcodePermalinks.Where(b => b.ShowInHistory == true).ToList();
                foreach (var bp in barcodePermalinks)
                {
                    var ahvm = new AddressHistoryViewModel() { Name = bp.Value, Category = "History", Order = 1 };
                    list.Add(ahvm);
                }
            }
            list = list.OrderBy(l => l.Order).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

       
    }
}