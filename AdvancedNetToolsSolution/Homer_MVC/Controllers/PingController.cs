﻿#region Using

using AntDal;
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
            if (id.HasValue)
            {
                using (AntDbContext context = new AntDbContext())
                {
                    PingPermalink pp = context.PingPermalinks.Find(id);
                    if (pp != null)
                    {

                        PingPermalinkViewModel ppvm = new PingPermalinkViewModel();
                        ppvm.Ip = pp.DestinationAddress;
                        ppvm.Id = pp.Id;
                        ppvm.PingResponseSummaries = AutoMapper.Mapper.DynamicMap<List<PingResponseSummaryViewModel>>(pp.PingResponseSummaries);

                        ppvm.GoogleMapString = Utils.GetGoogleMapsString(new string[] { Constants.DublinUrl, ppvm.PingResponseSummaries[0].SourceAddress });
                        return View(model: ppvm);
                    }
                }
                               
            }
            return View();

        }

        [HttpPost]
        public ActionResult GenerateId(PingRequestViewModel prvm)
        {
            prvm.DelayBetweenPings = 200; // 200 is more successfull
            if (string.IsNullOrEmpty(prvm.Ip))
            {
                return Json(string.Empty);
            }
            List<PingResponseSummaryViewModel> list = new List<PingResponseSummaryViewModel>();
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
                    summary.SourceAddress = "antnortheu.cloudapp.net"; // TEEEEEMP
                    list.Add(summary);
                    clients[i].Dispose();
                }
            }

            // Save to Db

            using (AntDbContext context = new AntDbContext())
            {
                PingPermalink pp = new PingPermalink();                
                pp.DestinationAddress = prvm.Ip;

                PingResponseSummary pr = AutoMapper.Mapper.DynamicMap<PingResponseSummary>(list[0]);

                pp.PingResponseSummaries.Add(pr);
                context.PingPermalinks.Add(pp);
                context.SaveChanges();

                return Json(pp.Id);
            }
        }
        public ActionResult Read([DataSourceRequest] DataSourceRequest request, PingRequestViewModel prvm)
        {
            if (string.IsNullOrEmpty(prvm.Ip))
            {
                return Json(new List<PingResponseSummaryViewModel>().ToDataSourceResult(request));
            }
            List<PingResponseSummaryViewModel> list = new List<PingResponseSummaryViewModel>();
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

            // Save to Db

            using (AntDbContext context = new AntDbContext())
            {
                PingPermalink pp = new PingPermalink();
                pp.DestinationAddress = prvm.Ip;

                PingResponseSummary pr = AutoMapper.Mapper.DynamicMap<PingResponseSummary>(list[0]);

                pp.PingResponseSummaries.Add(pr);
                context.PingPermalinks.Add(pp);
                context.SaveChanges();
            }


            var dsResult = Json(list.ToDataSourceResult(request));
            return dsResult;

        }
    }
}