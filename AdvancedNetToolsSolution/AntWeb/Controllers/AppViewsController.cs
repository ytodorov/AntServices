#region Using

using System;
using System.Net.Mail;
using System.Web.Mvc;
using SmartAdminMvc.Models;
using System.Net;
using System.Threading.Tasks;

#endregion

namespace SmartAdminMvc.Controllers
{
    
    public class AppViewsController : Controller
    {
        public ActionResult EmailCompose()
        {
            return View();
        }
    }
}