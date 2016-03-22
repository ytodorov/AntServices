#region Using

using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using SmartAdminMvc.Infrastructure;
using SmartAdminMvc.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Web.Mvc;

#endregion

namespace SmartAdminMvc.Controllers
{

    public class PingController : BaseController
    {
        // GET: home/index
        public ActionResult Index()
        {
            using (HttpClient client = new HttpClient())
            {
                string url = "http://antnortheu.cloudapp.net/home/exec?program=psping&args=8.8.8.8%20-q%20-i%200%20-n%2010";
                var res = client.GetStringAsync(url).Result;

                return View(model: res);

                
            }
        }
        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            using (HttpClient client = new HttpClient())
            {
                var encodedArgs = Uri.EscapeDataString(" 8.8.8.8 --delay 10ms -v1");
                string url = "http://antnortheu.cloudapp.net/home/exec?program=nping&args=" + encodedArgs;
                var res = client.GetStringAsync(url).Result;

                var summary = PingReplyParser.ParseSummary(res);
                List<PingReplySummaryViewModel> list = new List<PingReplySummaryViewModel>() { summary, summary, summary };

                var dsResult = Json(list.ToDataSourceResult(request));
                return dsResult;
            }
        }
    }
}