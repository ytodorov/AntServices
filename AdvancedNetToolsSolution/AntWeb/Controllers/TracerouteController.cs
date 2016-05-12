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

#endregion

namespace SmartAdminMvc.Controllers
{

    public class TracerouteController : BaseController
    {
        public ActionResult Index(int? id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult GenerateId(TracerouteRequestViewModel prvm, AntDbContext context)
        {
            using (HttpClient client = new HttpClient())
            {
                string encodedArgs = Uri.EscapeDataString($" --traceroute -sn -n {prvm.Ip}");
                string url = "http://ants-sea2.cloudapp.net/home/exec?program=nmap&args=" + encodedArgs;
                string tracerouteSummary = client.GetStringAsync(url).Result;

                List<TracerouteResponseDetailViewModel> list = TraceRouteParser.ParseSummary(tracerouteSummary);


                var traceroutePermalink = new TraceroutePermalink();
                traceroutePermalink.ShowInHistory = prvm.ShowInHistory;
                traceroutePermalink.UserCreatedIpAddress = Request.UserHostAddress;
                traceroutePermalink.DestinationAddress = prvm.Ip;
                traceroutePermalink.UserCreated = Request.UserHostAddress;
                traceroutePermalink.UserModified = Request.UserHostAddress;
                traceroutePermalink.DateCreated = DateTime.Now;
                traceroutePermalink.DateModified = DateTime.Now;

                List<TracerouteResponseDetail> tracerouteResponseDetails = Mapper.Map<List<TracerouteResponseDetail>>(list);

                TracerouteResponseSummary trs = new TracerouteResponseSummary();
                trs.SourceHostName = "ants-sea2.cloudapp.net";
                trs.TracerouteResponseDetails.AddRange(tracerouteResponseDetails);

                traceroutePermalink.TracerouteResponseSummaries.Add(trs);


                context.TraceroutePermalinks.Add(traceroutePermalink);
                context.SaveChanges();
                    
                    


            }
            return null;
        }
        
    }
}