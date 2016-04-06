#region Using

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
        // GET: home/index
        public ActionResult Index(int? id)
        {
            if (id == 123)
            {
               var result =  MemoryCache.Default.Get("123") as string;
                return View(model: result);
            }
            return View();
        }

        public ActionResult Read([DataSourceRequest] DataSourceRequest request, string ip)
        {
            using (HttpClient client = new HttpClient())
            {
                var encodedArgs = Uri.EscapeDataString($" --traceroute -sn -n {ip}");
                string url = "http://antnortheu.cloudapp.net/home/exec?program=nmap&args=" + encodedArgs;
                var tracerouteSummary = client.GetStringAsync(url).Result;

                List<TraceRouteReplyViewModel> list = TraceRouteParser.ParseSummary(tracerouteSummary);

                var dsResult = Json(list.ToDataSourceResult(request));
                return dsResult;
            }
        }

        [HttpPost]
        public ActionResult SaveResultHtml(string resultHtmlBase64)
        {
            string resultHtml = resultHtmlBase64.Base64Decode();

            MemoryCache.Default.Add("123", resultHtml, null);

            return new EmptyResult();
        }

    }
}