#region Using

using AntDal;
using AntDal.Entities;
using AutoMapper;
using FailTracker.Web.Infrastructure;
using FailTracker.Web.Infrastructure.ModelMetadata;
using FailTracker.Web.Infrastructure.Tasks;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using SmartAdminMvc.App_Start;
using SmartAdminMvc.Infrastructure;
using SmartAdminMvc.Models;
using AntDal.Migrations;
using StructureMap;
using StructureMap.Graph;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Net.Http;
using System.Timers;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SmartAdminMvc.Controllers;
using Quartz;
using SmartAdminMvc.Infrastructure.Jobs;
using Quartz.Impl;
using System.Linq;

#endregion

namespace SmartAdminMvc
{
#pragma warning disable JustCode_CSharp_TypeFileNameMismatch // Types not matching file names
    public class MvcApplication : HttpApplication
#pragma warning restore JustCode_CSharp_TypeFileNameMismatch // Types not matching file names
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
            MappingConfig.RegisterMaps();

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AntDbContext, Configuration>());

            var dummy = Utils.GetDeployedServicesUrlAddresses; // за да се извика конструктора на Uitls възможно най рано



            DependencyResolver.SetResolver(
                new StructureMapDependencyResolver(() => Container ?? IoC.Container));

            IoC.Container.Configure(cfg =>
            {
                cfg.AddRegistry(new StandardRegistry());
                cfg.AddRegistry(new ControllerRegistry());
                cfg.AddRegistry(new ActionFilterRegistry(
                    () => Container ?? IoC.Container));
                cfg.AddRegistry(new MvcRegistry());
                cfg.AddRegistry(new TaskRegistry());
                cfg.AddRegistry(new ModelMetadataRegistry());
            });

            using (var container = IoC.Container.GetNestedContainer())
            {
                foreach (var task in container.GetAllInstances<IRunAtInit>())
                {
                    task.Execute();
                }

                foreach (var task in container.GetAllInstances<IRunAtStartup>())
                {
                    task.Execute();
                }
            }

            if (Environment.MachineName != "YORDAN-PC")
            {
                //This message is to notify you that the Microsoft Azure Safeguards Team received a complaint of malicious activity originating from your Azure deployment(s).Details of the complaint and results of our initial investigation are included below. Failure to respond to this complaint within two business days or continued violation of the Microsoft Azure Acceptable Use Policy may result in the suspension of your deployment(s).
                //Timer timer = new Timer(TimeSpan.FromMinutes(1).TotalMilliseconds);
                //timer.Elapsed += Timer_Elapsed;
                //timer.Start();
            }

            // construct a scheduler factory
            ISchedulerFactory schedFact = new StdSchedulerFactory();

            // get a scheduler
            IScheduler sched = schedFact.GetScheduler();
            sched.Start();
            // define the job and tie it to our HelloJob class
            IJobDetail job = JobBuilder.Create<PortScanAddressesJob>()
                .WithIdentity("myJob", "group1")
                .Build();


            // Trigger the job to run now, and then every 40 seconds
            ITrigger trigger = TriggerBuilder.Create()
              .WithIdentity("myTrigger", "group1")
              .StartNow()
                .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(0, 0).InTimeZone(TimeZoneInfo.Utc))
              .Build();



            sched.ScheduleJob(job, trigger);

        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var top1000Sites = Utils.TopSitesGlobal.Take(1000).ToList();

            var randomInt = Utils.RandomNumberGenerator.Next(1, 9999);
            string siteUrl = Utils.TopSitesGlobal[randomInt];


            using (AntDbContext context = new AntDbContext())
            {
                var allPortScannSites = context.PortPermalinks.Select(p => p.DestinationAddress).ToList();

                for (int i = 0; i < 10000; i++)
                {
                    randomInt = Utils.RandomNumberGenerator.Next(1, 9999);
                    siteUrl = Utils.TopSitesGlobal[randomInt];

                    if (top1000Sites.Contains(siteUrl))
                    {
                        if (!allPortScannSites.Contains(siteUrl))
                        {
                            break;
                        }
                    }
                }



                PingController pc = new PingController();

                //var res = pc.GenerateId(new PingRequestViewModel() { ShowInHistory = true, Ip = siteUrl }, context);

                //TracerouteController tc = new TracerouteController();
                //var res2 = tc.GenerateId(new TracerouteRequestViewModel() { ShowInHistory = true, Ip = siteUrl }, context);

                PortscanController portC = new PortscanController();
                var res3 = portC.GenerateId(siteUrl, true, true, false, false);
            }

        }

        public void Application_BeginRequest()
        {
            HttpContext context = HttpContext.Current;

            string url = context.Request.Url.ToString();

            if (url.Contains(".js") || url.Contains(".css") || url.Contains(".png") || url.Contains(".gif"))
            {
                context.Response.Cache.SetExpires(DateTime.Now.AddDays(7));
                context.Response.Cache.SetMaxAge(TimeSpan.FromDays(7));
            }

            Container = IoC.Container.GetNestedContainer();

            foreach (var task in Container.GetAllInstances<IRunOnEachRequest>())
            {
                task.Execute();
            }
        }

        public void Application_EndRequest()
        {
            if (Container != null)
            {
                try
                {
                    foreach (var task in
                        Container.GetAllInstances<IRunAfterEachRequest>())
                    {
                        task.Execute();
                    }
                }
                finally
                {
                    Container.Dispose();
                    Container = null;
                }
            }
        }

        protected void Application_Error()
        {
            Debugger.Break();
            Exception ex = Server.GetLastError();
            using (AntDbContext context = new AntDbContext())
            {
                var el = new ErrorLogging();
                el.Message = ex.Message + "   " + ex?.InnerException?.Message;
                el.StackTrace = ex.StackTrace.ToString() + ex?.InnerException?.StackTrace;
                el.Data = ex.Data.ToString();
                el.ShowInHistory = true;
                //'Request is not available in this context'
                try
                {
                    el.UserCreated = Request?.UserHostAddress;
                    el.UserModified = Request?.UserHostAddress;
                }
                catch {}
                el.DateCreated = DateTime.Now;
                el.DateModified = DateTime.Now;


                context.ErrorLoggings.Add(el);
                context.SaveChanges();
            }
            if (Container != null)
            {
                foreach (var task in Container.GetAllInstances<IRunOnError>())
                {
                    task.Execute();
                }
            }
        }

        private void PreventAppsFromSleep()
        {
            var timer = new Timer(TimeSpan.FromMinutes(value: 1).TotalMilliseconds);
            timer.Start();
            timer.Elapsed += PingUrlsSoTheyDontSleep;
        }

        private void PingUrlsSoTheyDontSleep(object sender, ElapsedEventArgs e)
        {
            List<string> urls = Utils.GetDeployedServicesUrlAddresses;
            urls.Add(item: "http://ant-ne.azurewebsites.net");
            var stopWatch = new Stopwatch();
            foreach (string url in urls)
            {
                using (AntDbContext context = new AntDbContext())
                {
                    var ps = new PingSuccess();

                    using (HttpClient client = new HttpClient())
                    {
                        try
                        {
                            stopWatch.Start();
                            string tracerouteSummary = client.GetStringAsync(url).Result;
                            ps.Successful = true;
                            ps.IpAddress = url;
                            stopWatch.Stop();
                            ps.TimeNeeded = stopWatch.Elapsed;
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