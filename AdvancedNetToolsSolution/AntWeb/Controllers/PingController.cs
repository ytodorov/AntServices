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
using System.Net.Sockets;
using System.Data.Entity;
using AutoMapper;
using TimeAgo;

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
                    PingPermalink pp = context.PingPermalinks.Include(e => e.PingResponseSummaries).FirstOrDefault(d => d.Id == id);
                    if (pp != null)
                    {
                        PingPermalinkViewModel ppvm = Mapper.Map<PingPermalinkViewModel>(pp);
                        return View(model: ppvm);
                    }
                }
            }
            return View();

        }

        [HttpPost]
        public ActionResult GenerateId(PingRequestViewModel prvm, AntDbContext context)
        {
            string addressToPing = prvm.Ip;
            Uri uriResult;                    
            bool isUri = Uri.TryCreate(prvm.Ip, UriKind.Absolute, out uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps || uriResult.Scheme == Uri.UriSchemeFtp);
            if (isUri)
            {
                addressToPing = uriResult.Host;
            }





            prvm.DelayBetweenPings = 200; // 200 is more successfull
            if (string.IsNullOrEmpty(prvm.Ip))
            {
                return Json(string.Empty);
            }

            IPAddress dummy;
            bool isIpAddress = IPAddress.TryParse(prvm.Ip, out dummy);
            string firstOpenPort = "80";

            if (!isIpAddress)
            {
                if (prvm.Ip.Trim().StartsWith("https://"))
                {
                    //Uri uriHelper;
                    //if (Uri.TryCreate(prvm.Ip, UriKind.RelativeOrAbsolute, out uriHelper))
                    //{

                    //}
                    //addressToPing = prvm.Ip.Trim().Replace("https://", string.Empty);
                    firstOpenPort = "443";
                }
                else if (prvm.Ip.Trim().StartsWith("ftp://"))
                {
                    //addressToPing = prvm.Ip.Trim().Replace("ftp://", string.Empty);
                    firstOpenPort = "21";
                }
                else if (prvm.Ip.Trim().StartsWith("http://"))
                {
                    //addressToPing = prvm.Ip.Trim().Replace("ftp://", string.Empty);
                    firstOpenPort = "80";
                }
            }

            else //(string.IsNullOrEmpty(firstOpenPort))
            {
                firstOpenPort = Utils.GetFirstOpenPort(addressToPing);
            }

            List<PingResponseSummaryViewModel> list = new List<PingResponseSummaryViewModel>();
            List<Task<string>> tasks = new List<Task<string>>();
            List<HttpClient> clients = new List<HttpClient>();
            //string firstOpenPort = "80";
            //using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            //{
            //    try
            //    {
            //        socket.SendTimeout = 500;
            //        socket.ReceiveTimeout = 500;
            //        socket.Connect(prvm.Ip, 80);
            //        socket.Close();
            //    }
            //    catch (SocketException ex)
            //    {
            //        firstOpenPort = Utils.GetFirstOpenPort(prvm.Ip);
            //    }
            //}

            //string firstOpenPort = Utils.GetFirstOpenPort(prvm.Ip);

            if (string.IsNullOrEmpty(firstOpenPort))
            {
                var result = new { error = "Invalid address for ping" };
                return Json(result);
            }

            var urls = Utils.GetDeployedServicesUrlAddresses.ToList();

            for (int i = 0; i < urls.Count; i++)
            {

                HttpClient client = new HttpClient();
                clients.Add(client);
                var encodedArgs = Uri.EscapeDataString($"--tcp -p  {firstOpenPort} --delay 50ms -v1 {addressToPing} -c 5");
                var urlWithArgs = urls[i] + "/home/exec?program=nping&args=" + encodedArgs;
                Task<string> task = client.GetStringAsync(urlWithArgs);
                tasks.Add(task);
            }

            Task.WaitAll(tasks.ToArray());

            bool isUserRequestedAddressIp = false;
            IPAddress dummy1;
            isUserRequestedAddressIp = IPAddress.TryParse(addressToPing, out dummy1);

            for (int i = 0; i < tasks.Count; i++)
            {
                var summary = PingReplyParser.ParseSummary(tasks[i].Result);
                string sourceIp = Utils.HotstNameToIp[urls[i]];
                summary.SourceIpAddress = sourceIp;
                summary.SourceHostName = Utils.HotstNameToAzureLocation[urls[i]];
                if (!isUserRequestedAddressIp)
                {
                    summary.DestinationHostName = addressToPing;
                    summary.DestinationIpAddress = summary.DestinationIpAddress; //Utils.GetIpAddressFromHostName(prvm.Ip, urls[i]);
                }
                else
                {
                    summary.DestinationIpAddress = addressToPing;
                }
                list.Add(summary);
                clients[i].Dispose();
            }


            // Save to Db

            //using (AntDbContext context = new AntDbContext())
            {
                PingPermalink pingPermalink = new PingPermalink();
                pingPermalink.ShowInHistory = prvm.ShowInHistory;
                pingPermalink.UserCreatedIpAddress = Request.UserHostAddress;
                pingPermalink.DestinationAddress = prvm.Ip;
                pingPermalink.UserCreated = Request.UserHostAddress;
                pingPermalink.UserModified = Request.UserHostAddress;
                pingPermalink.DateCreated = DateTime.Now;
                pingPermalink.DateModified = DateTime.Now;

                List<PingResponseSummary> pr = Mapper.Map<List<PingResponseSummary>>(list);

                pingPermalink.PingResponseSummaries.AddRange(pr);
                context.PingPermalinks.Add(pingPermalink);
                context.SaveChanges();

                return Json(pingPermalink.Id);
            }
        }
        public ActionResult ReadPingPermalinks([DataSourceRequest] DataSourceRequest request, string address, bool? allPermalinks = false)
        {
            List<PingPermalink> pingPermalinks;
            if (!allPermalinks.GetValueOrDefault())
            {
                pingPermalinks = GetPermalinksForCurrentUser(address);
            }
            else
            {
                using (AntDbContext context = new AntDbContext())
                {
                    pingPermalinks = context.PingPermalinks.Where(p => p.ShowInHistory == true).OrderByDescending(k => k.Id).Take(100).ToList();
                }
            }

            var pingPermalinksViewModels = Mapper.Map<List<PingPermalinkViewModel>>(pingPermalinks);
            foreach (var p in pingPermalinksViewModels)
            {
                p.DateCreatedTimeAgo = p.DateCreated.GetValueOrDefault().TimeAgo();
            }

            var dsResult = Json(pingPermalinksViewModels.ToDataSourceResult(request));
            return dsResult;

        }
        public ActionResult ReadAddressesToPing([DataSourceRequest] DataSourceRequest request)
        {


            List<PingPermalink> pingPermalinks = GetPermalinksForCurrentUser(null);
            pingPermalinks = pingPermalinks.GroupBy(pp => pp.DestinationAddress).Select(group => group.First()).ToList();

            List<AddressHistoryViewModel> list = new List<AddressHistoryViewModel>();
            list.Add(new AddressHistoryViewModel { Name = Request.UserHostAddress, Category = "My IP", Order = 0 });
            foreach (var pp in pingPermalinks)
            {
                AddressHistoryViewModel ahvm = new AddressHistoryViewModel() { Name = pp.DestinationAddress, Category = "History", Order = 1 };
                if (!list.Any(l => l.Name.Equals(ahvm.Name)))
                {
                    list.Add(ahvm);
                }
            }

            foreach (string topSite in Utils.TopSitesGlobal)
            {
                AddressHistoryViewModel ahvm = new AddressHistoryViewModel() { Name = topSite, Category = "Top sites", Order = 2 };
                if (!list.Any(l => l.Name.Equals(ahvm.Name)))
                {
                    list.Add(ahvm);
                }
            }
            list = list.OrderBy(l => l.Order).ToList();
           return Json(list, JsonRequestBehavior.AllowGet);
        }

        private List<PingPermalink> GetPermalinksForCurrentUser(string address)
        {
            using (AntDbContext context = new AntDbContext())
            {
                string userIpAddress = Request.UserHostAddress;
                List<PingPermalink> pingPermalinks;
                if (string.IsNullOrEmpty(address))
                {
                    pingPermalinks = context.PingPermalinks.Where(p => p.UserCreatedIpAddress == userIpAddress && p.ShowInHistory == true).ToList();
                }
                else
                {
                    pingPermalinks = context.PingPermalinks.Where(p => p.DestinationAddress == address && p.ShowInHistory == true).ToList();
                }
                return pingPermalinks;
            }
        }

    }
}