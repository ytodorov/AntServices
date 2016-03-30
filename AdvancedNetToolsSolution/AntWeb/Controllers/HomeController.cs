#region Using

using SmartAdminMvc.Infrastructure;
using SmartAdminMvc.Models;
using System.Web.Mvc;

#endregion

namespace SmartAdminMvc.Controllers
{
    
    public class HomeController : Controller
    {
        // GET: home/index
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Social()
        {
            return View();
        }

        // GET: home/inbox
        public ActionResult Inbox()
        {
            return View();
        }

        // GET: home/widgets
        public ActionResult Widgets()
        {
            return View();
        }

        // GET: home/chat
        public ActionResult Chat()
        {
            return View();
        }

        [HttpPost]
        public string GoogleMap(TraceRouteReplyViewModel[] models)
        {
            Response.ContentType = "text/plain; charset=utf-8";
            var gmString = Utils.GetGoogleMapsString(models);
            return gmString;
        }
    }
}