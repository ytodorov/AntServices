﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.Extensions.Configuration;

namespace AdvancedNetToolsServicesWeb
{
    public class Startup
    {
        public static IConfigurationRoot Configuration;

        public static string ApplicationBasePath;

        public Startup(IApplicationEnvironment appEnv)
        {
            ApplicationBasePath = appEnv.ApplicationBasePath;

            var builder = new ConfigurationBuilder()
                .SetBasePath(appEnv.ApplicationBasePath)               
                .AddEnvironmentVariables();
            
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(new YordanExceptionFilter());
            });
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseIISPlatformHandler();

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);


    }

    public class YordanExceptionFilter : ActionFilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            ///logic...
        }
    }
}
