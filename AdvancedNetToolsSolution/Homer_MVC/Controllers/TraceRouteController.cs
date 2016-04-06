using Homer_MVC.Infrastructure;
using Homer_MVC.Models;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Homer_MVC.Controllers
{
    public class TraceRouteController : BaseController
    {
        public ActionResult Index()
        {
            //if (id.HasValue)
            //{
            //    using (AntDbContext context = new AntDbContext())
            //    {
            //        PingPermalink pp = context.PingPermalinks.Find(id);
            //        if (pp != null)
            //        {

            //            PingPermalinkViewModel ppvm = new PingPermalinkViewModel();
            //            ppvm.Ip = pp.DestinationAddress;
            //            ppvm.Id = pp.Id;
            //            ppvm.PingResponseSummaries = AutoMapper.Mapper.DynamicMap<List<PingResponseSummaryViewModel>>(pp.PingResponseSummaries);

            //            ppvm.GoogleMapString = Utils.GetGoogleMapsString(new string[] { Constants.DublinUrl, ppvm.PingResponseSummaries[0].SourceAddress });
            //            return View(model: ppvm);
            //        }
            //    }

            //}
            return View(model: new TraceRouteReplyViewModel());

        }
        [HttpPost]
        public ActionResult GenerateId(TraceRouteReplyViewModel trrvm)
        {
            if (string.IsNullOrEmpty(trrvm.Ip))
            {
                return Json(string.Empty);
            }
            List<TraceRouteReplyViewModel> list = new List<TraceRouteReplyViewModel>();
            List<Task<string>> tasks = new List<Task<string>>();
            List<HttpClient> clients = new List<HttpClient>();

            {
                for (int i = 0; i < 1; i++)
                {
                    HttpClient client = new HttpClient();
                    clients.Add(client);
                    var encodedArgs = Uri.EscapeDataString($"");
                    string url = "http://antnortheu.cloudapp.net/home/exec?program=nping&args=" + encodedArgs;
                    Task<string> task = client.GetStringAsync(url);
                    var summary = TraceRouteParser.ParseSummary(task.Result);
                    list.Add(summary[0]);
                    tasks.Add(task);
                }


                Task.WaitAll(tasks.ToArray());

                for (int i = 0; i < tasks.Count; i++)
                {
                    var summary = TraceRouteParser.ParseSummary(tasks[i].Result);
                    //summary.SourceAddress = "antnortheu.cloudapp.net"; // TEEEEEMP
                    list.Add(summary[0]);
                    clients[i].Dispose();
                }
            }

            // Save to Db

            //using (AntDbContext context = new AntDbContext())
            //{
            //    PingPermalink pp = new PingPermalink();                
            //    pp.DestinationAddress = prvm.Ip;

            //    PingResponseSummary pr = AutoMapper.Mapper.DynamicMap<PingResponseSummary>(list[0]);

            //    pp.PingResponseSummaries.Add(pr);
            //    context.PingPermalinks.Add(pp);
            //    context.SaveChanges();

            //    return Json(pp.Id);
            //}
            return Json(1);
        }
    }
}