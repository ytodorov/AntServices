using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using System.Net.NetworkInformation;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Reflection;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Net.Sockets;
using System.Diagnostics;
using System.Threading;
using System.IO;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AdvancedNetToolsServicesWeb.Controllers
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public string Index()
        {
            return "This is index";
        }

        public string Exec(string program, string args)
        {
            Response.ContentType = "text/plain; charset=utf-8";
            if (!program.EndsWith(".exe"))
            {
                program += ".exe";
            }

            try
            {
                var allFiles = Directory.GetFiles(Startup.ApplicationBasePath, "*.*", SearchOption.AllDirectories);
                var programFullPath = allFiles.FirstOrDefault(f => Path.GetFileName(f).ToLower().Equals(program.ToLower()));
                if (!string.IsNullOrEmpty(programFullPath))
                {

                    Process p = new Process();
                    p.StartInfo.FileName = programFullPath;
                    p.StartInfo.Arguments = args;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.RedirectStandardError = true;
                    p.StartInfo.UseShellExecute = false;

                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.ErrorDialog = false;

                    p.Start();
                    p.WaitForExit(10000);

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
