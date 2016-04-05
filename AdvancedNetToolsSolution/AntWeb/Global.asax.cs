#region Using

using SmartAdminMvc.Infrastructure;
using StructureMap;
using StructureMap.Graph;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Timers;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

#endregion

namespace SmartAdminMvc
{
    public class MvcApplication : HttpApplication
    {
        public IContainer Container
        {
            get
            {
                return (IContainer)HttpContext.Current.Items["_Container"];
            }
            set
            {
                HttpContext.Current.Items["_Container"] = value;
            }
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            IdentityConfig.RegisterIdentities();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            PreventAppsFromSleep();
                      

        }
        public void Application_BeginRequest()
        {
            Container = ObjectFactory.Container.GetNestedContainer();
        }

        public void Application_EndRequest()
        {
            Container.Dispose();
            Container = null; 
        }

        protected void Application_Error()
        {
            Exception ex = Server.GetLastError();
        }

        private void PreventAppsFromSleep()
        {
            Timer timer = new Timer(TimeSpan.FromMinutes(1).TotalMilliseconds);
            timer.Start();
            timer.Elapsed += PingUrlsSoTheyDontSleep;
        }

        private void PingUrlsSoTheyDontSleep(object sender, ElapsedEventArgs e)
        {
            List<string> urls = Utils.GetDeployedUrlAddresses();
            foreach (string url in urls)
            {
                using (HttpClient client = new HttpClient())
                {
                    var tracerouteSummary = client.GetStringAsync(url).Result;
                }
            }
        }
    }
}