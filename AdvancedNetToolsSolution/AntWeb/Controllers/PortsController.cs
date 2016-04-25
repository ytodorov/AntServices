using AntDal;
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

namespace SmartAdminMvc.Controllers
{
    public class PortsController : Controller
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
            if (string.IsNullOrEmpty(ip))
            {
                return Json(string.Empty);
            }

            using (HttpClient client = new HttpClient())
            {                
                string encodedArgs0 = Uri.EscapeDataString($"-T5 --top-ports 1000 -Pn {ip}");
                string url = "http://ants-neu.cloudapp.net/home/exec?program=nmap&args=" + encodedArgs0;

                string portSummary = client.GetStringAsync(url).Result;

                List<PortResponseSummaryViewModel> portViewModels = PortParser.ParseSummary(portSummary);






                // Save to Db

                using (AntDbContext context = new AntDbContext())
                {
                    var pp = new PortPermalink();
                    pp.ShowInHistory =showInHistory;
                    pp.UserCreatedIpAddress = Request.UserHostAddress;
                    pp.DestinationAddress = ip;
                    pp.UserCreated = Request.UserHostAddress;
                    pp.UserModified = Request.UserHostAddress;
                    pp.DateCreated = DateTime.Now;
                    pp.DateModified = DateTime.Now;

                    List<PortResponseSummary> pr = Mapper.Map<List<PortResponseSummary>>(portViewModels);

                    pp.PortResponseSummaries.AddRange(pr);
                    context.PortPermalinks.Add(pp);
                    context.SaveChanges();

                    return Json(pp.Id);
                }

            }
        }

        public ActionResult GetOpenPorts()
        {
            return View();
        }
    }
}