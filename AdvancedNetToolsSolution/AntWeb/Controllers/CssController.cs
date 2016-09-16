#region Using

using AntDal;
using AntDal.Entities;
using AutoMapper;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SmartAdminMvc.Infrastructure;
using SmartAdminMvc.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web.Mvc;
using TimeAgo;
using WebMarkupMin.Core;

#endregion Using

namespace SmartAdminMvc.Controllers
{
    public class CssController : BaseController
    {
        //[OutputCache(CacheProfile = "MyCache")]
        public ActionResult Index()
        {           
            return View();
        }

        public ActionResult Minify(string css)
        {
            var cssMinifier = new KristensenCssMinifier();
            var cssMinified =  cssMinifier.Minify(css, false);
            if (cssMinified.Errors.Count == 0)
            {
                return Json(cssMinified.MinifiedContent);
            }
            else
            {
                return Json(cssMinified.Errors[0].Message);
            }
        }
    }
}