﻿#region Using

using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using SmartAdminMvc.Infrastructure;
using SmartAdminMvc.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web.Mvc;

#endregion

namespace SmartAdminMvc.Controllers
{

    public class PingController : BaseController
    {
        // GET: home/index
        public ActionResult Index(int? id)
        {
            if (id == 123)
            {
                var result = MemoryCache.Default.Get("123") as string;

                PingPermalinkViewModel ppvm = new PingPermalinkViewModel();
                ppvm.Ip = "1.2.3.4";
                ppvm.PingReplies = new List<PingReplySummaryViewModel>()
                {
                    new PingReplySummaryViewModel() {  MinRtt=1, MaxRtt=2, AvgRtt = 1.5 }
                };


                return View(model: ppvm);
            }
            return View();

        }
        public ActionResult Read([DataSourceRequest] DataSourceRequest request, PingRequestViewModel prvm)
        {
            if (string.IsNullOrEmpty(prvm.Ip))
            {
                return Json(new List<PingReplySummaryViewModel>().ToDataSourceResult(request));
            }
            List<PingReplySummaryViewModel> list = new List<PingReplySummaryViewModel>();
            List<Task<string>> tasks = new List<Task<string>>();
            List<HttpClient> clients = new List<HttpClient>();

            //using (HttpClient client = new HttpClient())
            {
                for (int i = 0; i < 1; i++)
                {
                    HttpClient client = new HttpClient();
                    clients.Add(client);
                    var encodedArgs = Uri.EscapeDataString($" {prvm.Ip} --delay {prvm.DelayBetweenPings}ms -v1");
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