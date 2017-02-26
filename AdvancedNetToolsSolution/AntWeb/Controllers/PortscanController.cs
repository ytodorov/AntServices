using AntDal;
using AntDal.Entities;
using AutoMapper;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using SmartAdminMvc.Infrastructure;
using SmartAdminMvc.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TimeAgo;

namespace SmartAdminMvc.Controllers
{
    public class PortscanController : BaseController
    {
        [OutputCache(CacheProfile = "MyCache")]
        public ActionResult Index(int? id)
        {
            if (id.HasValue)
            {
                using (AntDbContext context = new AntDbContext())
                {
                    PortPermalink pp = context.PortPermalinks.Include(e => e.PortResponseSummaries).FirstOrDefault(s => s.Id == id);
                    if (pp != null)
                    {
                        PortPermalinkViewModel ppvm = Mapper.Map<PortPermalinkViewModel>(pp);
                        return View(model: ppvm);
                    }
                }
            }
            //else if (TempData["forcePortScan"] != null && (bool)TempData["forcePortScan"])
            //{
            //    PortPermalinkViewModel ppvm = new PortPermalinkViewModel()
            //    {
            //        DestinationAddress = Request.UserHostAddress,
            //        ForcePortScan = true
            //    };
            //    return View(model: ppvm);
            //}
            return View();
        }

        [Route("alexa-top-1000")]
        public ActionResult AlexaTop1000()
        {
            return View(viewName: "alexa-top-1000");
        }

        [Route("alexa-top-10000")]
        public ActionResult AlexaTop10000()
        {
            return View(viewName: "alexa-top-10000");
        }

        public ActionResult Me()
        {
            PortPermalinkViewModel ppvm = new PortPermalinkViewModel()
            {
                DestinationAddress = Request.UserHostAddress,
                ForcePortScan = true
            };

            var res = View("Index", ppvm);
            //TempData["forcePortScan"] = true;
            return res;
        }

        [HttpPost]
        public ActionResult GenerateId(string ip, bool? showInHistory, bool? wellKnown1000, bool? wellKnown, bool? allPorts)
        {
            var clients = new List<HttpClient>();
            try
            {


                ip = Utils.GetCorrectAddressOrHost(ip);
                string url = null;
                if (Utils.IsUrl(ip))
                {
                    url = ip;
                    ip = Utils.GetIpAddressFromHostName(url, Utils.GetDeployedServicesUrlAddresses[0]);
                }

                List<string> urls = Utils.GetDeployedServicesUrlAddresses.ToList();

                var tasksForPortscans = new List<Task<HttpResponseMessage>>();

                int grLength = 6554;

                for (int i = 0; i < urls.Count; i++)
                {
                    var client2 = new HttpClient();
                    client2.Timeout = TimeSpan.FromMinutes(60);
                    clients.Add(client2);

                    string args0 = $"-T4 -Pn -p {Utils.WellKnowPortStringListTop1000[i]} {ip}";
                    if (wellKnown.GetValueOrDefault())
                    {
                        args0 = $"-T4 -Pn -p {Utils.WellKnowPortStringList[i]} {ip}";
                    }
                    else if (allPorts.GetValueOrDefault())
                    {
                        var first = i * grLength + 1;
                        var last = (i + 1) * grLength;
                        if (last > 65535)
                        {
                            last = 65535;
                        }
                        args0 = $"-T4 -Pn -p {first}-{last} {ip}";
                    }

                    var content = new FormUrlEncodedContent(new[]
               {
                new KeyValuePair<string, string>("program", "nmap"),
                new KeyValuePair<string, string>("args", args0)
            });

                    Task<HttpResponseMessage> task = client2.PostAsync($"{urls[i]}/home/exec", content);
                    tasksForPortscans.Add(task);
                }
                Task.WaitAll(tasksForPortscans.ToArray(),
                 (int)TimeSpan.FromMinutes(60).TotalMilliseconds);

                List<string> portSummaryResultList = new List<string>();

                var portsSummary = string.Empty;
                for (int i = 0; i < tasksForPortscans.Count; i++)
                {
                    portsSummary = tasksForPortscans[i]
                 .Result.Content.ReadAsStringAsync().Result;
                    portSummaryResultList.Add(portsSummary);
                }




                using (AntDbContext context = new AntDbContext())
                {
                    var pp = new PortPermalink();
                    pp.ShowInHistory = showInHistory;
                    pp.UserCreatedIpAddress = Request?.UserHostAddress;
                    pp.DestinationAddress = ip;

                    if (!string.IsNullOrEmpty(url))
                    {
                        pp.DestinationIpAddress = ip;
                        pp.DestinationAddress = url;
                    }



                    pp.UserCreated = Request?.UserHostAddress;
                    pp.UserModified = Request?.UserHostAddress;
                    pp.DateCreated = DateTime.Now;
                    pp.DateModified = DateTime.Now;

                    List<PortResponseSummaryViewModel> allPortViewModels = new List<PortResponseSummaryViewModel>();

                    foreach (var ps in portSummaryResultList)
                    {
                        allPortViewModels.AddRange(PortParser.ParseSummary(ps));
                    }
                    //List<PortResponseSummaryViewModel> portViewModels = PortParser.ParseSummary(portsSummary);

                    List<PortResponseSummary> pr = Mapper.Map<List<PortResponseSummary>>(allPortViewModels);
                    pp.OpenPortsCount = allPortViewModels.Count(a => a.State.IsCaseInsensitiveEqual("open"));
                    pp.PortResponseSummaries.AddRange(pr);
                    context.PortPermalinks.Add(pp);
                    context.SaveChanges();

                    return Json(pp.Id);
                }
            }
            catch (Exception ex)
            {
                var result = new { error = ex.Message };
                return Json(result);
            }
            finally
            {
                foreach (var cl in clients)
                {
                    cl.Dispose();
                }
            }
        }

        public ActionResult ReadPortPermalinks([DataSourceRequest] DataSourceRequest request, AntDbContext context,
            string address, bool? allPermalinks = false, int maxResults = 1000, string info = null)
        {
            List<PortPermalink> portPermalinks = new List<PortPermalink>();
            if (string.IsNullOrEmpty(info))
            {
                if (!allPermalinks.GetValueOrDefault())
                {
                    portPermalinks = GetPermalinksForCurrentUser(address, context);
                }
                else
                {
                    portPermalinks = context.PortPermalinks.Where(p => p.ShowInHistory == true).OrderByDescending(k => k.Id)
                        .Take(count: maxResults).ToList();
                }
            }
            else
            {
                if ("alexatop1000".Equals(info, StringComparison.InvariantCultureIgnoreCase))
                {
                    var alexatop1000 = Utils.TopSitesGlobal.Take(1000).ToList();
                    portPermalinks = context.PortPermalinks.Where(p => alexatop1000.Contains(p.DestinationAddress))
                        .GroupBy(f => f.DestinationIpAddress).Select(s => s.FirstOrDefault())
                        .Take(1000).ToList();
                }
                else if ("alexatop10000".Equals(info, StringComparison.InvariantCultureIgnoreCase))
                {
                    var alexatop10000 = Utils.TopSitesGlobal.Take(10000).ToList();
                    portPermalinks = context.PortPermalinks.Where(p => alexatop10000.Contains(p.DestinationAddress))
                         .GroupBy(f => f.DestinationIpAddress).Select(s => s.FirstOrDefault())
                        .Take(10000).ToList();
                }


            }

            List<PortPermalinkViewModel> portPermalinksViewModels = Mapper.Map<List<PortPermalinkViewModel>>(portPermalinks);
            foreach (var p in portPermalinksViewModels)
            {
                p.DateCreatedTimeAgo = p.DateCreated.GetValueOrDefault().TimeAgo();
            }

            JsonResult dsResult = Json(portPermalinksViewModels.ToDataSourceResult(request));
            return dsResult;
        }


        private List<PortPermalink> GetPermalinksForCurrentUser(string address, AntDbContext context)
        {
            string userIpAddress = Request?.UserHostAddress;
            List<PortPermalink> pingPermalinks;
            if (string.IsNullOrEmpty(address))
            {
                pingPermalinks = context.PortPermalinks.Where(p => p.UserCreatedIpAddress == userIpAddress && p.ShowInHistory == true).ToList();
            }
            else
            {
                pingPermalinks = context.PortPermalinks.Where(p => p.DestinationAddress == address && p.ShowInHistory == true).ToList();
            }
            return pingPermalinks;
        }

        public ActionResult ReadWellKnownPorts([DataSourceRequest] DataSourceRequest request)
        {
            JsonResult dsResult = Json(Utils.WellKnownPorts.ToDataSourceResult(request));
            return dsResult;
        }
    }
    public class PortPermalinkEquality : IEqualityComparer<PortPermalink>
    {
        public PortPermalinkEquality()
        {

        }

        public bool Equals(PortPermalink x, PortPermalink y)
        {
            if (x == null && x == null)
                return true;
            else if (x == null || y == null)
                return false;
            else if (x.DestinationIpAddress == y.DestinationIpAddress)
                return true;
            else
                return false;
        }

        public int GetHashCode(PortPermalink obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}