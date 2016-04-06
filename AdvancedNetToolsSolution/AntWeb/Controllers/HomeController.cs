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

        public HomeController()
        {
            //_context = context;
        }
        // GET: home/index
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Error404()
        {
            return View();
        }
        public ActionResult Error500()
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