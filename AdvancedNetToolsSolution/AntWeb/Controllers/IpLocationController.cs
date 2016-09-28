#region Using

using System.Net.Http;
using System.Web.Mvc;

#endregion

namespace SmartAdminMvc.Controllers
{

    public class IpLocationController : Controller
    {
        public ActionResult Index(string ip)
        {
            return View((object)ip);   
        }

    }
}