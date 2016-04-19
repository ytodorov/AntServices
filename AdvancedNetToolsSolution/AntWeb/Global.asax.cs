#region Using

using AntDal;
using AntDal.Entities;
using AutoMapper;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using SmartAdminMvc.App_Start;
using SmartAdminMvc.Infrastructure;
using SmartAdminMvc.Models;
using StructureMap;
using StructureMap.Graph;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
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
        //public IContainer Container
        //{
        //    get
        //    {
        //        return (IContainer)HttpContext.Current.Items["_Container"];
        //    }
        //    set
        //    {
        //        HttpContext.Current.Items["_Container"] = value;
        //    }
        //}

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            IdentityConfig.RegisterIdentities();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            MappingConfig.RegisterMaps();
            //PreventAppsFromSleep();
            
        }
        ////public void Application_BeginRequest()
        ////{
        ////    Container = ObjectFactory.Container.GetNestedContainer();
        ////}

        ////public void Application_EndRequest()
        ////{
        ////    Container.Dispose();
        ////    Container = null; 
        ////}

        protected void Application_Error()
        {
            Exception ex = Server.GetLastError();
            using (AntDbContext context = new AntDbContext())
            {
                ErrorLogging el = new ErrorLogging();
                el.Message = ex.Message;
                el.StackTrace = ex.StackTrace.ToString();
                el.Data = ex.Data.ToString();
                el.UserCreated = Request.UserHostAddress;
                el.UserModified = Request.UserHostAddress;
                el.DateCreated = DateTime.Now;
                el.DateModified = DateTime.Now;


                context.ErrorLoggings.Add(el);
                context.SaveChanges();
            }
            //    var log = new LoggerConfiguration()
            //        .WriteTo.MSSqlServer(connectionString: @"Server=Alex-PC\SQLEXPRESS;Database=Ant;user=sa;password=1510alex;MultipleActiveResultSets=true;", tableName: "ErrorLoggings")
            //        .CreateLogger();
            //}
            
            //log.("@{Title}", ex.Message);
            //log.Information("@{Description}", ex.StackTrace);
            //log.Information("@{IpAddress}", ex.Data);
        }

        private void PreventAppsFromSleep()
        {
            Timer timer = new Timer(TimeSpan.FromMinutes(1).TotalMilliseconds);
            timer.Start();
            timer.Elapsed += PingUrlsSoTheyDontSleep;
        }

        private void PingUrlsSoTheyDontSleep(object sender, ElapsedEventArgs e)
        {
            List<string> urls = Utils.GetDeployedServicesUrlAddresses;
            urls.Add("http://ant-ne.azurewebsites.net");
            Stopwatch stopWatch = new Stopwatch();
            foreach (string url in urls)
            {
                using (AntDbContext context = new AntDbContext())
                {
                    PingSuccess ps = new PingSuccess();

                    using (HttpClient client = new HttpClient())
                    {
                        try
                        {
                            stopWatch.Start();
                             var tracerouteSummary = client.GetStringAsync(url).Result;
                            ps.Successful = true;
                            ps.IpAddress = url;
                            stopWatch.Stop();
                            ps.TimeNeeded =  stopWatch.Elapsed;
                            ps.StatusMessage = "The url" + ps.IpAddress + "has successfully been pinged for" + ps.TimeNeeded;

                        }
                        catch
                        {
                            ps.Successful = false;
                            ps.IpAddress = url;
                            stopWatch.Stop();
                            ps.TimeNeeded = stopWatch.Elapsed;
                            ps.StatusMessage = "Error occured during the build";
                        }
                        context.PingSuccesses.Add(ps);
                        context.SaveChanges();
                    }
                }
            }
        }
    }
}