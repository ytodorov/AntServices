#region Using

using System.Net.Http;
using System.Web.Mvc;

#endregion

namespace SmartAdminMvc.Controllers
{

    public class WhoisController : Controller
    {
        // GET: home/index
        public ActionResult Index()
        {
            using (HttpClient client = new HttpClient())
            {
                string url = "http://antnortheu.cloudapp.net/home/exec?program=whois&args=8.8.8.8";
                var res = client.GetStringAsync(url).Result;

                return View(model: res);
            }
        }

    }
}