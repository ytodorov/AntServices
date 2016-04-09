#region Using

using AntDal;
using AntDal.Entities;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using SmartAdminMvc.Infrastructure;
using SmartAdminMvc.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;

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
                        PingPermalinkViewModel ppvm = AutoMapper.Mapper.DynamicMap<PingPermalinkViewModel>(pp);
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

            var urls = Utils.GetDeployedServicesUrlAddresses();

            for (int i = 0; i < urls.Count; i++)
            {

                HttpClient client = new HttpClient();
                clients.Add(client);
                var encodedArgs = Uri.EscapeDataString($" {prvm.Ip} --delay 50ms -c 4 -v1");
                var urlWithArgs = urls[i] + "/home/exec?program=nping&args=" + encodedArgs;
                Task<string> task = client.GetStringAsync(urlWithArgs);
                tasks.Add(task);
            }

            Task.WaitAll(tasks.ToArray());

            bool isUserRequestedAddressIp = false;
            IPAddress dummy1;
            isUserRequestedAddressIp = IPAddress.TryParse(prvm.Ip, out dummy1);

            for (int i = 0; i < tasks.Count; i++)
            {
                var summary = PingReplyParser.ParseSummary(tasks[i].Result);
                string sourceIp = Utils.HotstNameToIp[urls[i]];
                summary.SourceIpAddress = sourceIp;
                summary.SourceHostName = Utils.HotstNameToAzureLocation[urls[i]];
                if (!isUserRequestedAddressIp)
                {
                    summary.DestinationHostName = prvm.Ip;
                    summary.DestinationIpAddress = Utils.GetIpAddressFromHostName(prvm.Ip, urls[i]);
                }
                else
                {
                    summary.DestinationIpAddress = prvm.Ip;
                }
                list.Add(summary);
                clients[i].Dispose();
            }


            // Save to Db

            using (AntDbContext context = new AntDbContext())
            {
                PingPermalink pp = new PingPermalink();
                pp.UserCreatedIpAddress = Request.UserHostAddress;
                pp.DestinationAddress = prvm.Ip;
                pp.UserCreated = Request.UserHostAddress;
                pp.UserModified = Request.UserHostAddress;
                pp.DateCreated = DateTime.Now;
                pp.DateModified = DateTime.Now;

                List<PingResponseSummary> pr = AutoMapper.Mapper.DynamicMap<List<PingResponseSummary>>(list);

                pp.PingResponseSummaries.AddRange(pr);
                context.PingPermalinks.Add(pp);
                context.SaveChanges();

                return Json(pp.Id);
            }
        }
        public ActionResult ReadPingPermalinks([DataSourceRequest] DataSourceRequest request)
        {
            string userIpAddress = Request.UserHostAddress;
            using (AntDbContext context = new AntDbContext())
            {
                var pingPermalinks = context.PingPermalinks.Where(p => p.UserCreatedIpAddress == userIpAddress).ToList();
                var pingPermalinksViewModels = AutoMapper.Mapper.DynamicMap<List<PingPermalinkViewModel>>(pingPermalinks);
                var dsResult = Json(pingPermalinksViewModels.ToDataSourceResult(request));
                return dsResult;
            }



        }
    }
}