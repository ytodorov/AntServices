using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace AntServicesMvc5
{
#pragma warning disable JustCode_CSharp_TypeFileNameMismatch // Types not matching file names
    public class MvcApplication : System.Web.HttpApplication
#pragma warning restore JustCode_CSharp_TypeFileNameMismatch // Types not matching file names
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
