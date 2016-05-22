﻿using AntDal;
using AntDal.Entities;
using SmartAdminMvc.Infrastructure;
using SmartAdminMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using AutoMapper;
using Kendo.Mvc.UI;
using TimeAgo;
using Kendo.Mvc.Extensions;

namespace SmartAdminMvc.Controllers
{
    public class PortscanController : BaseController
    {
        // GET: home/index
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

            List<string> urls = new List<string>() { "http://ants-neu.cloudapp.net" };

            var tasksForTraceroutes = new List<Task<string>>();
            var tasksForLatencies = new List<Task<string>>();
            var clients = new List<HttpClient>();

            for (int i = 0; i < urls.Count; i++)
            {
                var client = new HttpClient();
                clients.Add(client);
                string encodedArgs0 = Uri.EscapeDataString($"-T5 --top-ports 1000 -Pn {ip}");
                string urlWithArgs = urls[i] + "/home/exec?program=nmap&args=" + encodedArgs0;
                Task<string> task = client.GetStringAsync(urlWithArgs);
                tasksForTraceroutes.Add(task);
            }

            Task.WaitAll(tasksForTraceroutes.ToArray().Union(tasksForLatencies).ToArray());



            for (int i = 0; i < tasksForTraceroutes.Count; i++)
            {
                var portsSummary = tasksForTraceroutes[i].Result;

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

            }

            return new EmptyResult();

            //using (AntDbContext context = new AntDbContext())
            //{
            //    var pp = new PortPermalink();
            //    pp.ShowInHistory = showInHistory;
            //    pp.UserCreatedIpAddress = Request.UserHostAddress;
            //    pp.DestinationAddress = ip;
            //    pp.UserCreated = Request.UserHostAddress;
            //    pp.UserModified = Request.UserHostAddress;
            //    pp.DateCreated = DateTime.Now;
            //    pp.DateModified = DateTime.Now;

            //    List<PortResponseSummary> pr = Mapper.Map<List<PortResponseSummary>>(portViewModels);

            //    pp.PortResponseSummaries.AddRange(pr);
            //    context.PortPermalinks.Add(pp);
            //    context.SaveChanges();

            //    return Json(pp.Id);
            //}


            //if (string.IsNullOrEmpty(ip))
            //{
            //    return Json(string.Empty);
            //}

            //using (HttpClient client = new HttpClient())
            //{
            //    string encodedArgs0 = Uri.EscapeDataString($"-T5 --top-ports 1000 -Pn {ip}");
            //    string url = "http://ants-neu.cloudapp.net/home/exec?program=nmap&args=" + encodedArgs0;

            //    string portSummary = client.GetStringAsync(url).Result;

            //    List<PortResponseSummaryViewModel> portViewModels = PortParser.ParseSummary(portSummary);






            //    // Save to Db

            //    using (AntDbContext context = new AntDbContext())
            //    {
            //        var pp = new PortPermalink();
            //        pp.ShowInHistory = showInHistory;
            //        pp.UserCreatedIpAddress = Request.UserHostAddress;
            //        pp.DestinationAddress = ip;
            //        pp.UserCreated = Request.UserHostAddress;
            //        pp.UserModified = Request.UserHostAddress;
            //        pp.DateCreated = DateTime.Now;
            //        pp.DateModified = DateTime.Now;

            //        List<PortResponseSummary> pr = Mapper.Map<List<PortResponseSummary>>(portViewModels);

            //        pp.PortResponseSummaries.AddRange(pr);
            //        context.PortPermalinks.Add(pp);
            //        context.SaveChanges();

            //        return Json(pp.Id);
            //    }

            //}
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