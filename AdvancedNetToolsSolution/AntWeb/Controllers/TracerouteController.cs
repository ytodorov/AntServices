#region Using

using AntDal;
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

             
            }
            return null;
        }
        
    }
}