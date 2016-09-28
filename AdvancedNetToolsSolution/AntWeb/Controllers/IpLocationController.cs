#region Using

using System.Net.Http;
using System.Web.Mvc;

#endregion

namespace SmartAdminMvc.Controllers
{

    public class IpLocationController : Controller
    {
        public ActionResult Find(string ip)
        {
            return View((object)ip);   
        }

    }
}