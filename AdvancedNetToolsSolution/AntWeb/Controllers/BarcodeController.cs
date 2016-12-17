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
        // GET: home/index
        //[OutputCache(CacheProfile = "MyCache")]
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
            barcodePermalink.UserCreated = Request.UserHostAddress;
            barcodePermalink.UserModified = Request.UserHostAddress;
            barcodePermalink.DateCreated = DateTime.Now;
            barcodePermalink.DateModified = DateTime.Now;

            context.BarcodePermalinks.Add(barcodePermalink);
            context.SaveChanges();

            return Json(barcodePermalink.Id);
        }

        public ActionResult ReadPingPermalinks([DataSourceRequest] DataSourceRequest request, string address, bool? allPermalinks = false)
        {
            List<PingPermalink> pingPermalinks;
            if (!allPermalinks.GetValueOrDefault())
            {
                pingPermalinks = GetPermalinksForCurrentUser(address);
            }
            else
            {
                using (AntDbContext context = new AntDbContext())
                {
                    pingPermalinks = context.PingPermalinks.Where(p => p.ShowInHistory == true).OrderByDescending(k => k.Id).Take(count: 100).ToList();
                }
            }

            List<PingPermalinkViewModel> pingPermalinksViewModels = Mapper.Map<List<PingPermalinkViewModel>>(pingPermalinks);
            foreach (var p in pingPermalinksViewModels)
            {
                p.DateCreatedTimeAgo = p.DateCreated.GetValueOrDefault().TimeAgo();
            }

            JsonResult dsResult = Json(pingPermalinksViewModels.ToDataSourceResult(request));
            return dsResult;
        }

        public ActionResult ReadAddressesToPing([DataSourceRequest] DataSourceRequest request)
        {
            List<PingPermalink> pingPermalinks = GetPermalinksForCurrentUser(address: null);
            pingPermalinks = pingPermalinks.GroupBy(pp => pp.DestinationAddress).Select(group => group.First()).ToList();

            var list = new List<AddressHistoryViewModel>();
            list.Add(new AddressHistoryViewModel { Name = Request.UserHostAddress, Category = "My IP", Order = 0 });
            foreach (var pp in pingPermalinks)
            {
                var ahvm = new AddressHistoryViewModel() { Name = pp.DestinationAddress, Category = "History", Order = 1 };
                if (!list.Any(l => l.Name.Equals(ahvm.Name, StringComparison.InvariantCultureIgnoreCase)))
                {
                    list.Add(ahvm);
                }
            }

            foreach (string topSite in Utils.TopSitesGlobal)
            {
                var ahvm = new AddressHistoryViewModel() { Name = topSite, Category = "Top sites", Order = 2 };
                if (!list.Any(l => l.Name.Equals(ahvm.Name, StringComparison.InvariantCultureIgnoreCase)))
                {
                    list.Add(ahvm);
                }
            }
            list = list.OrderBy(l => l.Order).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        private List<PingPermalink> GetPermalinksForCurrentUser(string address)
        {
            using (AntDbContext context = new AntDbContext())
            {
                string userIpAddress = Request.UserHostAddress;
                List<PingPermalink> pingPermalinks;
                if (string.IsNullOrEmpty(address))
                {
                    pingPermalinks = context.PingPermalinks.Where(p => p.UserCreatedIpAddress == userIpAddress && p.ShowInHistory == true).ToList();
                }
                else
                {
                    pingPermalinks = context.PingPermalinks.Where(p => p.DestinationAddress == address && p.ShowInHistory == true).ToList();
                }
                return pingPermalinks;
            }
        }

        public static void GoTo()
        {
            Driver.Instance.Navigate().GoToUrl(url: "https://localhost:44300/ping");
            var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromMinutes(1));
            IWebElement element = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id(idToFind: "btnPing")));
        }

        public static void Ping(string address)
        {
            IWebElement pingInput = Driver.Instance.FindElement(By.Id(idToFind: "ip"));
            pingInput.Clear();
            pingInput.SendKeys(address);

            IWebElement btnPing = Driver.Instance.FindElement(By.Id(idToFind: "btnPing"));
            btnPing.Click();
        }
    }
}