#region Using

using SmartAdminMvc.Infrastructure;
using SmartAdminMvc.Models;
using System.Web.Mvc;
using System.Linq;
using SmartAdminMvc.Data;
using AntDal;
using System;
using System.Collections.Generic;
using Kendo.Mvc.UI;
using System.Data.Entity;


#endregion

namespace SmartAdminMvc.Controllers
{

    public class HomeController : Controller
    {
        //private readonly AntDbContext _context; // never used

        public HomeController()
        {
            //_context = context;
        }
        // GET: home/index
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
            var gmString = Utils.GetGoogleMapsString(models.Select(m => m.Ip));
            return gmString;
        }

        [HttpPost]
        public string GoogleMapFromIps(int? permalinkId)
        {
            using (AntDbContext context = new AntDbContext())
            {
                var pingPermalink = context.PingPermalinks.Include(path => path.PingResponseSummaries).FirstOrDefault(p => p.Id == permalinkId);

                List<string> ipAddresses = new List<string>();
                foreach (var prs in pingPermalink.PingResponseSummaries)
                {
                    ipAddresses.Add(prs.SourceIpAddress);
                    ipAddresses.Add(prs.DestinationIpAddress);
                }
                Response.ContentType = "text/plain; charset=utf-8";
                var gmString = Utils.GetGoogleMapsString(ipAddresses, starLine: true);
                return gmString;
            }
        }       
       
    }
}