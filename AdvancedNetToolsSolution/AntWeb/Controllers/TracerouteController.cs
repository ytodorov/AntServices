#region Using

using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using SmartAdminMvc.Infrastructure;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

#endregion

namespace SmartAdminMvc.Controllers
{

    public class TracerouteController : BaseController
    {
        // GET: home/index
        public ActionResult Index()
        {
            using (HttpClient client = new HttpClient())
            {
                //var encodedArgs = Uri.EscapeDataString($"--traceroute 92.247.12.80 -sn -n -T5");
                //string url = "http://antnortheu.cloudapp.net/home/exec?program=nmap&args=" + encodedArgs;
                //var tracerouteSummary = client.GetStringAsync(url).Result;

                //TraceRouteParser.ParseSummary(tracerouteSummary);

                //return View(model: tracerouteSummary);
                return View();
            }

           
        }

        public ActionResult Read([DataSourceRequest] DataSourceRequest request, string ip)
        {
            using (HttpClient client = new HttpClient())
            {
                var encodedArgs = Uri.EscapeDataString($"--traceroute {ip} -sn -n");
                string url = "http://antnortheu.cloudapp.net/home/exec?program=nmap&args=" + encodedArgs;
                var tracerouteSummary = client.GetStringAsync(url).Result;

                var list = TraceRouteParser.ParseSummary(tracerouteSummary);

                var dsResult = Json(list.ToDataSourceResult(request));
                return dsResult;
            }


         

        }

    }
}