using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartAdminMvc.Controllers
{
    public class PortsController : Controller
    {
        // GET: Ports
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetOpenPorts()
        {
            return View();
        }
    }
}