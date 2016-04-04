#region Using

using SmartAdminMvc.Infrastructure;
using SmartAdminMvc.Models;
using System.Web.Mvc;
using System.Linq;
using SmartAdminMvc.Data;
using AntDal;


#endregion

namespace SmartAdminMvc.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly AntDbContext _context;

        public HomeController(AntDbContext context)
        {
            _context = context;
        }
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
            var gmString = Utils.GetGoogleMapsString(models.Select(m => m.Ip));
            return gmString;
        }

        [HttpPost]
        public string GoogleMapFromIps(string[] ips)
        {
            Response.ContentType = "text/plain; charset=utf-8";
            var gmString = Utils.GetGoogleMapsString(ips);
            return gmString;
        }
    }
}