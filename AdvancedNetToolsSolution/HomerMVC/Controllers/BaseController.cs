using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using System.Text;
using Newtonsoft.Json;

namespace Homer_MVC.Controllers
{
    public class BaseController : Controller
    {
        
        public override JsonResult Json(object data)
        {
            return base.Json(data);
        }

        public override JsonResult Json(object data, JsonSerializerSettings serializerSettings)
        {            
            
            return base.Json(data, serializerSettings);
        }
        //protected override JsonResult Json(object data, string contentType, Encoding contentEncoding)
        //{
        //    return base.Json(data, contentType, contentEncoding, JsonRequestBehavior.AllowGet);
        //}

        //protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        //{
        //    behavior = JsonRequestBehavior.AllowGet;
        //    return base.Json(data, contentType, contentEncoding, behavior);
        //}
    }
}