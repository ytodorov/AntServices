#region Using

using System;
using System.Net.Http;
using System.Web.Mvc;

#endregion

namespace SmartAdminMvc.Controllers
{

    public class TracerouteController : Controller
    {
        // GET: home/index
        public ActionResult Index()
        {
            using (HttpClient client = new HttpClient())
            {
                var encodedArgs = Uri.EscapeDataString($"--traceroute 92.247.12.80 -sn -n -T5");
                string url = "http://antnortheu.cloudapp.net/home/exec?program=nmap&args=" + encodedArgs;
                var res = client.GetStringAsync(url).Result;
                return View(model: res);
            }

           
        }

    }
}