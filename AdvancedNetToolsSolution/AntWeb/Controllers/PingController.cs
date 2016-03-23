#region Using

using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using SmartAdminMvc.Infrastructure;
using SmartAdminMvc.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
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
            List<PingReplySummaryViewModel> list = new List<PingReplySummaryViewModel>();
            List<Task<string>> tasks = new List<Task<string>>();
            List<HttpClient> clients = new List<HttpClient>();

            //using (HttpClient client = new HttpClient())
            {
                for (int i = 0; i < 5; i++)
                {
                    HttpClient client = new HttpClient();
                    clients.Add(client);
                    var encodedArgs = Uri.EscapeDataString(" 8.8.8.8 --delay 10ms -v1");
                    string url = "http://antnortheu.cloudapp.net/home/exec?program=nping&args=" + encodedArgs;
                    Task<string> task = client.GetStringAsync(url);
                    //var summary = PingReplyParser.ParseSummary(task.Result);
                    //list.Add(summary);
                    tasks.Add(task);
                }


                Task.WaitAll(tasks.ToArray());

                for (int i = 0; i < tasks.Count; i++)
                {
                    var summary = PingReplyParser.ParseSummary(tasks[i].Result);
                    list.Add(summary);
                    clients[i].Dispose();
                }
            }


            var dsResult = Json(list.ToDataSourceResult(request));
            return dsResult;

        }
    }
}