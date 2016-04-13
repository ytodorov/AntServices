#region Using

using System.Net.Http;
using System.Web.Mvc;

#endregion

namespace SmartAdminMvc.Controllers
{

    public class MyIpController : Controller
    {
        // GET: home/index
        public ActionResult Index()
        {
            return View(model: Request.UserHostAddress);
            //using (HttpClient client = new HttpClient())
            //{
            //    string url = "http://ants-neu.cloudapp.net/home/getclientip";
            //    var res = client.GetStringAsync(url).Result;
            //    return View(model:res);

            //}            
        }

    }
}