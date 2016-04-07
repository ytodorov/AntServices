#region Using

using System.Web.Mvc;

#endregion

namespace SmartAdminMvc.Controllers
{
    
    public class AppViewsController : Controller
    {
        public ActionResult EmailCompose()
        {
            return View();
        }

        public ActionResult EmailView()
        {
            return View();
        }
    }
}