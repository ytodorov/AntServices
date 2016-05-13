#region Using

using AntDal;
using AntDal.Entities;
using AutoMapper;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using SmartAdminMvc.Extensions;
using SmartAdminMvc.Infrastructure;
using SmartAdminMvc.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Data.Entity;
using System.Linq;
using TimeAgo;

#endregion

namespace SmartAdminMvc.Controllers
{

    public class TracerouteController : BaseController
    {
        public ActionResult Index(int? id)
        {
            if (id.HasValue)
            {
                using (AntDbContext context = new AntDbContext())
                {
                    TraceroutePermalink pp = context.TraceroutePermalinks
                        .Include(e => e.TracerouteResponseSummaries.Select(s => s.TracerouteResponseDetails)).FirstOrDefault(d => d.Id == id);
                    if (pp != null)
                    {
                        TraceroutePermalinkViewModel ppvm = Mapper.Map<TraceroutePermalinkViewModel>(pp);

                        

                        return View(model: ppvm);
                    }
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult GenerateId(TracerouteRequestViewModel prvm, AntDbContext context)
        {
            List<string> urls = Utils.GetDeployedServicesUrlAddresses.ToList();

            var tasksForTraceroutes = new List<Task<string>>();
            var tasksForLatencies = new List<Task<string>>();
            var clients = new List<HttpClient>();

            for (int i = 0; i < urls.Count; i++)
            {
                var client = new HttpClient();
                clients.Add(client);
                string encodedArgs = Uri.EscapeDataString($" --traceroute -sn {prvm.Ip}");
                string urlWithArgs = urls[i] + "/home/exec?program=nmap&args=" + encodedArgs;
                Task<string> task = client.GetStringAsync(urlWithArgs);
                tasksForTraceroutes.Add(task);
            }

            Task.WaitAll(tasksForTraceroutes.ToArray().Union(tasksForLatencies).ToArray());

            var traceroutePermalink = new TraceroutePermalink();
            traceroutePermalink.ShowInHistory = prvm.ShowInHistory;
            traceroutePermalink.UserCreatedIpAddress = Request.UserHostAddress;
            traceroutePermalink.DestinationAddress = prvm.Ip;
            traceroutePermalink.UserCreated = Request.UserHostAddress;
            traceroutePermalink.UserModified = Request.UserHostAddress;
            traceroutePermalink.DateCreated = DateTime.Now;
            traceroutePermalink.DateModified = DateTime.Now;

            for (int i = 0; i < tasksForTraceroutes.Count; i++)
            {
                var tracerouteSummary = tasksForTraceroutes[i].Result;

                List<TracerouteResponseDetailViewModel> tracerouteResponseDetailsViewModels = TraceRouteParser.ParseSummary(tracerouteSummary);

                List<TracerouteResponseDetail> tracerouteResponseDetails = Mapper.Map<List<TracerouteResponseDetail>>(tracerouteResponseDetailsViewModels);
                TracerouteResponseSummary trs = new TracerouteResponseSummary();
           
                var url = urls[i];
                var name = Utils.HotstNameToAzureLocation[url];
                trs.SourceHostName = name;
                trs.SourceIpAddress = Utils.HotstNameToIp[url];

                trs.TracerouteResponseDetails.AddRange(tracerouteResponseDetails);

                traceroutePermalink.TracerouteResponseSummaries.Add(trs);

            }


            context.TraceroutePermalinks.Add(traceroutePermalink);
            context.SaveChanges();

            return Json(traceroutePermalink.Id);


            //using (HttpClient client = new HttpClient())
            //{
            //    string encodedArgs = Uri.EscapeDataString($" --traceroute -sn {prvm.Ip}");
            //    string url = "http://ants-sea2.cloudapp.net/home/exec?program=nmap&args=" + encodedArgs;
            //    string tracerouteSummary = client.GetStringAsync(url).Result;

            //    List<TracerouteResponseDetailViewModel> list = TraceRouteParser.ParseSummary(tracerouteSummary);


            //    var traceroutePermalink = new TraceroutePermalink();
            //    traceroutePermalink.ShowInHistory = prvm.ShowInHistory;
            //    traceroutePermalink.UserCreatedIpAddress = Request.UserHostAddress;
            //    traceroutePermalink.DestinationAddress = prvm.Ip;
            //    traceroutePermalink.UserCreated = Request.UserHostAddress;
            //    traceroutePermalink.UserModified = Request.UserHostAddress;
            //    traceroutePermalink.DateCreated = DateTime.Now;
            //    traceroutePermalink.DateModified = DateTime.Now;

            //    List<TracerouteResponseDetail> tracerouteResponseDetails = Mapper.Map<List<TracerouteResponseDetail>>(list);

            //    TracerouteResponseSummary trs = new TracerouteResponseSummary();
            //    trs.SourceHostName = "ants-sea2.cloudapp.net";
            //    trs.TracerouteResponseDetails.AddRange(tracerouteResponseDetails);

            //    traceroutePermalink.TracerouteResponseSummaries.Add(trs);


            //    context.TraceroutePermalinks.Add(traceroutePermalink);
            //    context.SaveChanges();

            //    return Json(traceroutePermalink.Id);


            //}
          
        }

        public ActionResult ReadTraceroutePermalinks([DataSourceRequest] DataSourceRequest request, string address, bool? allPermalinks = false)
        {
            List<TraceroutePermalink> traceroutePermalinks;
            if (!allPermalinks.GetValueOrDefault())
            {
                traceroutePermalinks = GetPermalinksForCurrentUser(address);
            }
            else
            {
                using (AntDbContext context = new AntDbContext())
                {
                    traceroutePermalinks = context.TraceroutePermalinks.Where(p => p.ShowInHistory == true).OrderByDescending(k => k.Id).Take(count: 100).ToList();
                }
            }

            List<TraceroutePermalinkViewModel> traceroutePermalinksViewModels = Mapper.Map<List<TraceroutePermalinkViewModel>>(traceroutePermalinks);
            foreach (var p in traceroutePermalinksViewModels)
            {
                p.DateCreatedTimeAgo = p.DateCreated.GetValueOrDefault().TimeAgo();
            }

            JsonResult dsResult = Json(traceroutePermalinksViewModels.ToDataSourceResult(request));
            return dsResult;

        }
      

        private List<TraceroutePermalink> GetPermalinksForCurrentUser(string address)
        {
            using (AntDbContext context = new AntDbContext())
            {
                string userIpAddress = Request.UserHostAddress;
                List<TraceroutePermalink> pingPermalinks;
                if (string.IsNullOrEmpty(address))
                {
                    pingPermalinks = context.TraceroutePermalinks.Where(p => p.UserCreatedIpAddress == userIpAddress && p.ShowInHistory == true).ToList();
                }
                else
                {
                    pingPermalinks = context.TraceroutePermalinks.Where(p => p.DestinationAddress == address && p.ShowInHistory == true).ToList();
                }
                return pingPermalinks;
            }
        }

    }
}