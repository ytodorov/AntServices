using SmartAdminMvc.Infrastructure;
using SmartAdminMvc.Models;
using System.Web.Mvc;
using System.Linq;
using AntDal;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace SmartAdminMvc.Controllers
{

    public class HomeController : Controller
    {

        public HomeController()
        {
        }

        public ActionResult Index()
        {
            return View();
        }

        //http://stackoverflow.com/questions/7265193/mvc-problem-with-custom-error-pages

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
            string gmString = Utils.GetGoogleMapsString(models.Select(m => m.Ip));
            return gmString;
        }

        [HttpPost]
        public string GoogleMapFromIps(int? permalinkId)
        {
            using (AntDbContext context = new AntDbContext())
            {
                AntDal.Entities.PingPermalink pingPermalink = context.PingPermalinks.Include(path => path.PingResponseSummaries).FirstOrDefault(p => p.Id == permalinkId);

                var ipAddresses = new List<string>();
                foreach (var prs in pingPermalink.PingResponseSummaries)
                {
                    ipAddresses.Add(prs.SourceIpAddress);
                    ipAddresses.Add(prs.DestinationIpAddress);
                }
                Response.ContentType = "text/plain; charset=utf-8";
                string gmString = Utils.GetGoogleMapsString(ipAddresses, starLine: true);
                return gmString;
            }
        }

    }
}