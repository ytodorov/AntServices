#region Using

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
                string url = "http://antnortheu.cloudapp.net/home/exec?program=tracert&args=8.8.8.8%20-q%20-i%200%20-n%2010";
                var res = client.GetStringAsync(url).Result;

                return View(model: res);
            }
        }

    }
}