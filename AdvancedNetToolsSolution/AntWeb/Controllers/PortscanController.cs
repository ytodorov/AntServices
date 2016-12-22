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
            return View();
        }

        [HttpPost]
        public ActionResult GenerateId(string ip, bool? showInHistory, bool? wellKnownPorts)
        {
            try
            {
                ip = Utils.GetCorrectAddressOrHost(ip);


                //string errorMessage = Utils.CheckIfHostIsUp(ip);
                //if (!string.IsNullOrEmpty(errorMessage))
                //{
                //    var result = new { error = errorMessage };
                //    return Json(result);
                //}

                var client = new HttpClient();
                client.Timeout = TimeSpan.FromMinutes(15);
                // Do not use T5
                string args0 = $"-T4 -Pn -p {Utils.WellKnownPortsString} {ip}";
                if (!wellKnownPorts.GetValueOrDefault())
                {
                    args0 = $"-T4 -Pn --top-ports 2000 {Utils.WellKnownPortsString} {ip}";
                }

                var content = new FormUrlEncodedContent(new[]
           {
                new KeyValuePair<string, string>("program", "nmap"),
                new KeyValuePair<string, string>("args", args0)
            });
                var portsSummary = client.PostAsync("http://ants-neu.cloudapp.net/home/exec", content)
                    .Result.Content.ReadAsStringAsync().Result;

                client.Dispose();

                using (AntDbContext context = new AntDbContext())
                {
                    var pp = new PortPermalink();
                    pp.ShowInHistory = showInHistory;
                    pp.UserCreatedIpAddress = Request.UserHostAddress;
                    pp.DestinationAddress = ip;
                    pp.UserCreated = Request.UserHostAddress;
                    pp.UserModified = Request.UserHostAddress;
                    pp.DateCreated = DateTime.Now;
                    pp.DateModified = DateTime.Now;

                    List<PortResponseSummaryViewModel> portViewModels = PortParser.ParseSummary(portsSummary);

                    List<PortResponseSummary> pr = Mapper.Map<List<PortResponseSummary>>(portViewModels);

                    pp.PortResponseSummaries.AddRange(pr);
                    context.PortPermalinks.Add(pp);
                    context.SaveChanges();

                    return Json(pp.Id);
                }
            }
            catch(Exception ex)
            {
                var result = new { error = ex.Message };
                return Json(result);
            }
        }

        public ActionResult ReadPortPermalinks([DataSourceRequest] DataSourceRequest request, AntDbContext context, string address, bool? allPermalinks = false)
        {
            List<PortPermalink> portPermalinks;
            if (!allPermalinks.GetValueOrDefault())
            {
                portPermalinks = GetPermalinksForCurrentUser(address, context);
            }
            else
            {
                portPermalinks = context.PortPermalinks.Where(p => p.ShowInHistory == true).OrderByDescending(k => k.Id).Take(count: 100).ToList();
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
            string userIpAddress = Request.UserHostAddress;
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
}