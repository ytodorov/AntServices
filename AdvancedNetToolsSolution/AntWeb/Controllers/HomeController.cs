using AntDal;
using AntDal.Entities;
using SmartAdminMvc.Infrastructure;
using SmartAdminMvc.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace SmartAdminMvc.Controllers
{
    public class HomeController : BaseController
    {
        //чупя билда да видя дали ще има email
        //public HomeController()
        //{
        //}

        [OutputCache(CacheProfile = "MyCache")]
        public virtual ActionResult Index()
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
        public string GoogleMap(TracerouteResponseDetailViewModel[] models)
        {
            Response.ContentType = "text/plain; charset=utf-8";
            string gmString = Utils.GetGoogleMapsString(models.Select(m => m.Ip));
            return gmString;
        }

        [HttpPost]
        public string GoogleMapFromIps(int? permalinkId, AntDbContext context, string permalinkType)
        {
            if (permalinkType.Equals(typeof(PingPermalink).Name, StringComparison.CurrentCultureIgnoreCase))
            {
                PingPermalink pingPermalink = context.PingPermalinks.Include(path => path.PingResponseSummaries)
                    .FirstOrDefault(p => p.Id == permalinkId);

                var ipAddresses = new List<string>();
                foreach (var prs in pingPermalink.PingResponseSummaries)
                {
                    ipAddresses.Add(prs.SourceIpAddress);
                    ipAddresses.Add(prs.DestinationIpAddress);
                }
                Response.ContentType = "text/plain; charset=utf-8";
                string gmString = Utils.GetGoogleMapsString(ipAddresses, pingPermalink.PingResponseSummaries, starLine: true);
                return gmString;
            }
            else if (permalinkType.Equals(typeof(TraceroutePermalink).Name, StringComparison.CurrentCultureIgnoreCase))
            {
                TraceroutePermalink pingPermalink = context.TraceroutePermalinks.Include(path =>
                path.TracerouteResponseSummaries.Select(t => t.TracerouteResponseDetails))
                    .FirstOrDefault(p => p.Id == permalinkId);

                var ipAddresses = new List<string>();
                foreach (var prs in pingPermalink.TracerouteResponseSummaries)
                {
                    var orderedTRDs = prs.TracerouteResponseDetails.OrderBy(o => o.Hop).ToList();
                    ipAddresses.Add(prs.SourceIpAddress);
                    foreach (var t in orderedTRDs)
                    {
                        IPAddress ipAddress;
                        if (IPAddress.TryParse(t.Ip, out ipAddress))
                        {
                            if (!Utils.IsIPLocal(ipAddress))
                            {
                                ipAddresses.Add(t.Ip);
                            }
                        }
                    }
                    //break;
                }
                Response.ContentType = "text/plain; charset=utf-8";
                string gmString = Utils.GetGoogleMapsString(ipAddresses, null, starLine: false);
                return gmString;
            }
            return string.Empty;
        }

        public string Download(int downloadLength)
        {
            Response.ContentType = "text/plain; charset=utf-8";
            Request.Headers["Accept-Encoding"] = ""; // Това премахва компресирането
            string result = Utils.RandomString(downloadLength);
            return result;
        }

        [HttpPost]
        public string Upload(string uploadString)
        {
            Response.ContentType = "text/plain; charset=utf-8";
            return string.Empty;
        }
    }
}