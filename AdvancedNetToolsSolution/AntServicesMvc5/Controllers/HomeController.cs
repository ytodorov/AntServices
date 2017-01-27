using MediaToolkit;
using MediaToolkit.Model;
using Newtonsoft.Json;
using PubNubMessaging.Core;
using SmartAdminMvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

        public string Download(int downloadLength)
        {
            Response.ContentType = "text/plain; charset=utf-8";
            Request.Headers["Accept-Encoding"] = ""; // Това премахва компресирането
            string result = Utils.RandomString(downloadLength);
            return result;
        }

        [HttpPost]
        public string Upload(string uploadString)
        {
            Response.ContentType = "text/plain; charset=utf-8";
            return string.Empty;
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

        public void RemoveOldFiles(string programFullPath)
        {
            FileInfo di = new FileInfo(programFullPath);
            var files = di.Directory.GetFiles();
            foreach (var fileInfo in files)
            {
                try
                {
                    var extension = Path.GetExtension(fileInfo.FullName);
                    if (!extension.Contains("exe"))
                    {
                        if (fileInfo.CreationTimeUtc < DateTime.UtcNow.AddHours(-3))
                        {
                            fileInfo.Delete();
                        }
                    }
                }
                catch(Exception)
                {

                }
            }

            
        }

        /// <summary>
        /// Download Youtube Audio Video
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public ActionResult DAV(string guid)
        {
            string[] allFiles = Directory.GetFiles(HostingEnvironment.ApplicationPhysicalPath, searchPattern: "*.*", searchOption: SearchOption.AllDirectories);

            var resultPath = allFiles.FirstOrDefault(f => f.Contains(guid));

            string ct = MimeMapping.GetMimeMapping(resultPath);

            FileInfo fi = new FileInfo(resultPath);
            string fileName = fi.Name.Replace(guid, string.Empty);

            var result = File(resultPath, ct, fileName);
            return result;
        }

        public ActionResult YoutubeDownload(string program, string args, string guid, string fileName)
        {
            Response.ContentType = "text/plain; charset=utf-8";
            if (!program.EndsWith(value: ".exe"))
            {
                program += ".exe";
            }

            try
            {


                string[] allFiles = Directory.GetFiles(HostingEnvironment.ApplicationPhysicalPath, searchPattern: "*.exe", searchOption: SearchOption.AllDirectories);

                StringBuilder sb = new StringBuilder();
                foreach (var s in allFiles)
                {
                    sb.AppendLine(s);
                }


                string programFullPath = string.Empty; 

                foreach (string fullFileName in allFiles)
                {
                    string fn = Path.GetFileName(fullFileName);
                    if (fn.ToLowerInvariant().Equals(program.ToLowerInvariant(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        programFullPath = fullFileName;
                        sb.AppendLine(fn.ToLowerInvariant() + " IS EQUAL TO " + program.ToLowerInvariant());
                    }
                    else
                    {
                        sb.AppendLine(fn.ToLowerInvariant() + " is not equal to " + program.ToLowerInvariant());
                    }
                }


                if (!string.IsNullOrEmpty(programFullPath))
                {
                    Task.Factory.StartNew(() => RemoveOldFiles(programFullPath));
                    

                    FileInfo fi = new FileInfo(programFullPath);

                    var allFilesInExeDir = fi.Directory.GetFiles();

                    if (string.IsNullOrEmpty(guid))
                    {
                        guid = Guid.NewGuid().ToString();
                    }
                    var p = new Process();
                    p.StartInfo.FileName = programFullPath;
                    p.StartInfo.Arguments = args + " -o " + guid;
                    p.StartInfo.RedirectStandardInput = true;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.RedirectStandardError = true;
                    p.StartInfo.UseShellExecute = false; // zaradi  p.StartInfo.RedirectStandardInput = true;

                    p.StartInfo.CreateNoWindow = false;
                    p.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                    p.StartInfo.ErrorDialog = false;
                    p.StartInfo.WorkingDirectory = fi.Directory.FullName;

                 

                    p.Start();

                    //https://msdn.microsoft.com/en-us/library/system.diagnostics.process.standarderror(v=vs.110).aspx
                    //p.StandardOutput.ReadLineAsync(); // .ReadToEnd();

                    while (!p.StandardOutput.EndOfStream)
                    {
                        string line = p.StandardOutput.ReadLine();
                        // do something with line
                    }

                    string error = p.StandardError.ReadToEnd();

                    p.WaitForExit(milliseconds: (int)TimeSpan.FromSeconds(1).TotalMilliseconds);

                    allFiles = Directory.GetFiles(HostingEnvironment.ApplicationPhysicalPath, searchPattern: "*.*", searchOption: SearchOption.AllDirectories);
                    string resultPath = allFiles.FirstOrDefault(f => Path.GetFileName(f).ToLower().Contains(guid.ToLower()));

                    //return result + "ГРЕШКИ" + error;
                    allFilesInExeDir = fi.Directory.GetFiles();

                    if (System.IO.File.Exists(resultPath))
                    {
                        try
                        {
                            if (!p.HasExited)
                            {
                                p.Kill();
                            }
                        }
                        catch (Exception)
                        {

                        }
                        FileInfo resultFi = new FileInfo(resultPath);
                        string filepathWithoutExtension = resultPath.Replace(Path.GetExtension(resultPath), string.Empty);
                        resultFi.MoveTo(filepathWithoutExtension + fileName);

                        Pubnub pubnub = new Pubnub("pub-c-5bd3c97d-e760-4aa8-9b91-0746c78606f9",
            "sub-c-406da20e-e48e-11e6-b325-02ee2ddab7fe", "sec-c-MzkzZmE0Y2UtODRkMC00MzcxLThmMTYtNWIzOGQyOTVmYjgz");

                        pubnub.Publish(guid, guid, UserCallBack, PubnubClientError);

                        return new EmptyResult();
                    }
                    return Json("No such output directory " + resultPath + error + "search locations: " + sb.ToString());



                }
                return Json("No such program " + program + "search locations: " + sb.ToString());
            }
            catch (Exception ex)
            {
                string json = JsonConvert.SerializeObject(ex, Formatting.Indented);
                return Json(json);
            }
            finally
            {

            }

        }

        private void P_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            string message = e.Data;
        }

        public void UserCallBack(object o)
        {

        }

        public void PubnubClientError(PubnubClientError error)
        {

        }
    }
}