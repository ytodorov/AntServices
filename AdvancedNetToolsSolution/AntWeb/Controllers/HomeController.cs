#region Using

using SmartAdminMvc.Infrastructure;
using SmartAdminMvc.Models;
using System.Web.Mvc;
using System.Linq;
using SmartAdminMvc.Data;
using AntDal;
using System;
using System.Collections.Generic;


#endregion

namespace SmartAdminMvc.Controllers
{

    public class HomeController : Controller
    {
        private readonly AntDbContext _context;

        public HomeController()
        {
            //_context = context;
        }
        // GET: home/index
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Error404()
        {
            return View();
        }
        public ActionResult Error500()
        {
            return View();
        }

        [HttpPost]
        public string GoogleMap(TraceRouteReplyViewModel[] models)
        {
            Response.ContentType = "text/plain; charset=utf-8";
            var gmString = Utils.GetGoogleMapsString(models.Select(m => m.Ip));
            return gmString;
        }

        [HttpPost]
        public string GoogleMapFromIps(int? permalinkId)
        {
            using (AntDbContext context = new AntDbContext())
            {
                var pingPermalink = context.PingPermalinks.Find(permalinkId);

                List<string> ipAddresses = new List<string>();
                foreach (var prs in pingPermalink.PingResponseSummaries)
                {
                    ipAddresses.Add(prs.SourceIpAddress);
                    ipAddresses.Add(prs.DestinationIpAddress);
                }

                //string[] ipAddresses = pingPermalink.PingResponseSummaries.
                //     string[] ipAddress = ips.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                Response.ContentType = "text/plain; charset=utf-8";
                var gmString = Utils.GetGoogleMapsString(ipAddresses, starLine: true);
                return gmString;

            }


        }
    }
}