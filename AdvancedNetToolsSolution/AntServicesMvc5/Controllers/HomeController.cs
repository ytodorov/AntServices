﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace AntServicesMvc5.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public string GetClientIp()
        {
            Response.ContentType = "text/plain; charset=utf-8";
            string result = Request.UserHostAddress;
            return result;
        }

        public string Exec(string program, string args)
        {
            Response.ContentType = "text/plain; charset=utf-8";
            if (!program.EndsWith(value: ".exe"))
            {
                program += ".exe";
            }

            try
            {
                string[] allFiles = Directory.GetFiles(HostingEnvironment.ApplicationPhysicalPath, searchPattern: "*.*", searchOption: SearchOption.AllDirectories);
                string programFullPath = allFiles.FirstOrDefault(f => Path.GetFileName(f).ToLower().Equals(program.ToLower()));
                if (!string.IsNullOrEmpty(programFullPath))
                {

                    var p = new Process();
                    p.StartInfo.FileName = programFullPath;
                    p.StartInfo.Arguments = args;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.RedirectStandardError = true;
                    p.StartInfo.UseShellExecute = false;

                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.ErrorDialog = false;

                    p.Start();
                    p.WaitForExit(milliseconds: 10000);

                    string result = p.StandardOutput.ReadToEnd();
                    string error = p.StandardError.ReadToEnd();

                    return result + "ГРЕШКИ" + error;
                }
                return "No such program " + program;
            }
            catch (Exception ex)
            {
                string json = JsonConvert.SerializeObject(ex, Formatting.Indented);
                return json;
            }
        }
    }
}