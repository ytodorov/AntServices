#region Using

using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using SmartAdminMvc.Infrastructure;
using SmartAdminMvc.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web.Mvc;

#endregion

namespace SmartAdminMvc.Controllers
{

    public class SpeedtestController : BaseController
    {
        // GET: home/index
        public ActionResult Index()
        {
            return View();
        }

        public string Download(int bytes)
        {
            Response.ContentType = "text/plain; charset=utf-8";
            Request.Headers["Accept-Encoding"] = "";
            //string s = new string('A', bytes);
            //return s;

            RNGCryptoServiceProvider Rand = new RNGCryptoServiceProvider();
            byte[] byteArray = new byte[bytes];
            Rand.GetBytes(byteArray);
            string s = Convert.ToBase64String(byteArray);
            return s;
        }

        [HttpPost]
        public string Upload(string uploadString)
        {
            Response.ContentType = "text/plain; charset=utf-8";
            return string.Empty;
        }


    }
}