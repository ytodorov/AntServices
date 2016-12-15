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
        public ActionResult GenerateId(PingRequestViewModel prvm, AntDbContext context)
        {
            string addressToPing = prvm.Ip;
            Uri uriResult;
            bool isUri = Uri.TryCreate(prvm.Ip, UriKind.Absolute, out uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps || uriResult.Scheme == Uri.UriSchemeFtp);
            if (isUri)
            {
                addressToPing = uriResult.Host;
            }

            List<string> urls = Utils.GetDeployedServicesUrlAddresses.ToList();

            string errorMessage = Utils.CheckIfHostIsUp(addressToPing);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                var result = new { error = errorMessage };
                return Json(result);
            }

            if (string.IsNullOrEmpty(prvm.Ip))
            {
                return Json(string.Empty);
            }

            IPAddress dummy;
            bool isIpAddress = IPAddress.TryParse(prvm.Ip, out dummy);
            string firstOpenPort = "80";

            if (!isIpAddress)
            {
                if (prvm.Ip.Trim().StartsWith(value: "https://"))
                {
                    //Uri uriHelper;
                    //if (Uri.TryCreate(prvm.Ip, UriKind.RelativeOrAbsolute, out uriHelper))
                    //{
                    //}
                    //addressToPing = prvm.Ip.Trim().Replace("https://", string.Empty);
                    firstOpenPort = "443";
                }
                else if (prvm.Ip.Trim().StartsWith(value: "ftp://") || prvm.Ip.Trim().StartsWith(value: "ftp."))
                {
                    //addressToPing = prvm.Ip.Trim().Replace("ftp://", string.Empty);
                    firstOpenPort = "21";
                }
                else if (prvm.Ip.Trim().StartsWith(value: "http://"))
                {
                    //addressToPing = prvm.Ip.Trim().Replace("ftp://", string.Empty);
                    firstOpenPort = "80";
                }
            }
            else //(string.IsNullOrEmpty(firstOpenPort))
            {
                firstOpenPort = Utils.GetFirstOpenPort(addressToPing);
            }

            var list = new List<PingResponseSummaryViewModel>();
            var tasksForPings = new List<Task<string>>();
            var tasksForLatencies = new List<Task<string>>();
            var clients = new List<HttpClient>();

            if (string.IsNullOrEmpty(firstOpenPort))
            {
                var result = new { error = $"Host '{addressToPing}' has no open TCP ports!" };
                return Json(result);
            }

            for (int i = 0; i < urls.Count; i++)
            {
                var client = new HttpClient();
                clients.Add(client);
                string encodedArgs = Uri.EscapeDataString($"--tcp -p  {firstOpenPort} --delay 50ms -v1 {addressToPing} -c 5");
                string urlWithArgs = urls[i] + "/home/exec?program=nping&args=" + encodedArgs;
                Task<string> task = client.GetStringAsync(urlWithArgs);
                tasksForPings.Add(task);
            }

            for (int i = 0; i < urls.Count; i++)
            {
                var client = new HttpClient();
                clients.Add(client);
                // Ако не се намери отворен порт ще липсва latency. Primer: ftp.microsoft.com
                // Затова трябва да се използва - --top-ports 10
                string encodedArgs = Uri.EscapeDataString($"-n --top-ports 10 -Pn {addressToPing}"); // Точно тези са аргументите, -sn -n
                string urlWithArgs = urls[i] + "/home/exec?program=nmap&args=" + encodedArgs;
                Task<string> task = client.GetStringAsync(urlWithArgs);
                tasksForLatencies.Add(task);
            }

            Task.WaitAll(tasksForPings.ToArray().Union(tasksForLatencies).ToArray(),
                (int)TimeSpan.FromMinutes(3).TotalMilliseconds);

            bool isUserRequestedAddressIp = false;
            IPAddress dummy1;
            isUserRequestedAddressIp = IPAddress.TryParse(addressToPing, out dummy1);

            for (int i = 0; i < tasksForPings.Count; i++)
            {
                PingResponseSummaryViewModel summary = PingReplyParser.ParseSummary(tasksForPings[i].Result);
                string sourceIp = Utils.HotstNameToIp[urls[i]];
                summary.SourceIpAddress = sourceIp;
                summary.SourceHostName = Utils.HotstNameToAzureLocation[urls[i]];
                if (!isUserRequestedAddressIp)
                {
                    summary.DestinationHostName = addressToPing;
                    summary.DestinationIpAddress = summary.DestinationIpAddress; //Utils.GetIpAddressFromHostName(prvm.Ip, urls[i]);
                }
                else
                {
                    summary.DestinationIpAddress = addressToPing;
                }
                string latenceyString = tasksForLatencies[i].Result;
                summary.Latency = Utils.ParseLatencyString(latenceyString);
                list.Add(summary);
            }

            foreach (var client in clients)
            {
                client.Dispose();
            }

            var pingPermalink = new PingPermalink();
            pingPermalink.ShowInHistory = prvm.ShowInHistory;
            pingPermalink.UserCreatedIpAddress = Request.UserHostAddress;
            pingPermalink.DestinationAddress = prvm.Ip;
            pingPermalink.UserCreated = Request.UserHostAddress;
            pingPermalink.UserModified = Request.UserHostAddress;
            pingPermalink.DateCreated = DateTime.Now;
            pingPermalink.DateModified = DateTime.Now;

            List<PingResponseSummary> pr = Mapper.Map<List<PingResponseSummary>>(list);

            pingPermalink.PingResponseSummaries.AddRange(pr);
            context.PingPermalinks.Add(pingPermalink);
            context.SaveChanges();

            return Json(pingPermalink.Id);
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