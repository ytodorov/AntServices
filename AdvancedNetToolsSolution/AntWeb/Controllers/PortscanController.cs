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
        // GET: home/index
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
        public ActionResult GenerateId(string ip, bool? showInHistory)
        {
            //List<string> urls = Utils.GetDeployedServicesUrlAddresses.ToList().Take(1).ToList();

            try
            {
                Uri url = new Uri(ip);
                ip = url.Host;
            }
            catch (Exception)
            {
            }


            string errorMessage = Utils.CheckIfHostIsUp(ip);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                var result = new { error = errorMessage };
                return Json(result);
            }

            var client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(15);
            // Do not use T5
            string encodedArgs0 = Uri.EscapeDataString($"-T4 -Pn --top-ports 1000 {ip}");
            string urlWithArgs = "http://ants-neu.cloudapp.net/home/exec?program=nmap&args=" + encodedArgs0;
            var portsSummary = client.GetStringAsync(urlWithArgs).Result;
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


            return new EmptyResult();
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
    }
}