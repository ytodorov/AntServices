using SmartAdminMvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AntWeb.Controllers
{
    public class InterfaceController : BaseController
    {

        public ActionResult EmailTemplate()
        {
            return View();
        }

    }
}
