using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Homer_MVC.Controllers
{
    public class AppViewsController : Controller
    {
        public ActionResult EmailCompose()
        {
            return View();
        }
    }
}
