using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SmartAdminMvc.Controllers
{
    //[WhiteSpaceFilter]
    public class BaseController : Controller
    {
        //[OutputCacheAttribute(Duration = 3600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        //public virtual ActionResult Index()
        //{
        //    return View();
        //}

        //public virtual ActionResult Index(int? id)
        //{
        //    if (!id.HasValue)
        //    {
        //        return View();
        //    }
        //    return new EmptyResult();
        //}

        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding)
        {
            var baseResult = base.Json(data, contentType, contentEncoding, JsonRequestBehavior.AllowGet);
            baseResult.MaxJsonLength = int.MaxValue;
            return baseResult;
        }

        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            behavior = JsonRequestBehavior.AllowGet;
            var baseResult = base.Json(data, contentType, contentEncoding, behavior);
            baseResult.MaxJsonLength = int.MaxValue;
            return baseResult;
        }

        protected override void HandleUnknownAction(string actionName)
        {
            // Нищо не  правим. Предполагаме че няма нужда от exception
            // Това се получава когато чрез код викаме страници от toolsfornet.com, Примерно от HttpClient
            // A public action method 'exec' was not found on controller 'SmartAdminMvc.Controllers.HomeController'.
            // Prosto premahvame nevalidnite izvikvaniq ot PingAllLocations proekta
            base.HandleUnknownAction(actionName);
        }
    }
}