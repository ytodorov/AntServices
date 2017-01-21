using Newtonsoft.Json;
using SmartAdminMvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
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

        public string ExecFfmpeg(string program, string args, string inputFileName, string inputBytesBase64)
        {
            byte[] tempFileBytes = Convert.FromBase64String(inputBytesBase64);
            Response.ContentType = "text/plain; charset=utf-8";
            if (!program.EndsWith(value: ".exe"))
            {
                program += ".exe";
            }
            string pathToWriteTemp = string.Empty;
            try
            {


                string[] allFiles = Directory.GetFiles(HostingEnvironment.ApplicationPhysicalPath, searchPattern: "*.*", searchOption: SearchOption.AllDirectories);
                string programFullPath = allFiles.FirstOrDefault(f => Path.GetFileName(f).ToLower().Equals(program.ToLower()));
                if (!string.IsNullOrEmpty(programFullPath))
                {
                    FileInfo fi = new FileInfo(programFullPath);
                    pathToWriteTemp = Path.Combine(fi.Directory.FullName, inputFileName);

                    System.IO.File.WriteAllBytes(pathToWriteTemp, tempFileBytes);

                    var allFilesInExeDir = fi.Directory.GetFiles();



                    var p = new Process();
                    p.StartInfo.FileName = programFullPath;
                    p.StartInfo.Arguments = args;
                    p.StartInfo.RedirectStandardInput = true;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.RedirectStandardError = true;
                    p.StartInfo.UseShellExecute = false;

                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    p.StartInfo.ErrorDialog = false;
                    p.StartInfo.WorkingDirectory = fi.Directory.FullName;


                    p.Start();
                    p.WaitForExit(milliseconds: 10000);


                    string resultPath = Path.Combine(fi.Directory.FullName, "output.mp3");

                    //string result = p.StandardOutput.ReadToEnd();
                    //string error = p.StandardError.ReadToEnd();

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
                        catch(Exception)
                        {

                        }
                        var bytesToReturs = System.IO.File.ReadAllBytes(resultPath);
                        var base64Result = Convert.ToBase64String(bytesToReturs);

                        //Thread.Sleep(500);
                        try
                        {
                            System.IO.File.Delete(pathToWriteTemp);
                            System.IO.File.Delete(resultPath);
                        }
                        catch
                        {

                        }
                        return base64Result;
                    }

                 
                }
                return "No such program " + program;
            }
            catch (Exception ex)
            {
                if (System.IO.File.Exists(pathToWriteTemp))
                {

                    string json = JsonConvert.SerializeObject(ex, Formatting.Indented);
                    return json;
                }
            }
            finally
            {
                
            }
            return string.Empty;

        }
    }
}