using MediaToolkit;
using MediaToolkit.Model;
using Newtonsoft.Json;
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

        public string ConvertAudio(string inputFileBytesAsBase64String, string inputFileExtensionWithDot,
            string outputFileExtensionWithDot)
        {
            var folderPath = Path.GetTempPath();  //server.MapPath("~/tmp");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var tempFileInput = Path.Combine(folderPath, "input" + Guid.NewGuid().ToString() + inputFileExtensionWithDot);

            var bytes = Convert.FromBase64String(inputFileBytesAsBase64String);

            System.IO.File.WriteAllBytes(tempFileInput, bytes);

            var tempFileOutput = Path.Combine(folderPath, "output" + Guid.NewGuid().ToString() + outputFileExtensionWithDot);
            System.IO.File.WriteAllBytes(tempFileOutput, new byte[0]);

            var inputFile = new MediaFile { Filename = tempFileInput };
            var outputFile = new MediaFile { Filename = tempFileOutput };

            byte[] result = null;
            using (var engine = new Engine())
            {
                engine.GetMetadata(inputFile);

                engine.Convert(inputFile, outputFile);

                result = System.IO.File.ReadAllBytes(outputFile.Filename);

            }

            System.IO.File.Delete(tempFileInput);
            System.IO.File.Delete(tempFileOutput);

            string base64Result = Convert.ToBase64String(result);

            return base64Result;
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
                        catch (Exception)
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

        public ActionResult YoutubeDownload(string program, string args)
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


                string programFullPath = string.Empty; //allFiles.FirstOrDefault(f =>
                //f.ToLowerInvariant().IndexOf(program.ToLowerInvariant(), 0, StringComparison.InvariantCultureIgnoreCase) >= 0);

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
                    FileInfo fi = new FileInfo(programFullPath);

                    var allFilesInExeDir = fi.Directory.GetFiles();


                    var guid = Guid.NewGuid().ToString();
                    var p = new Process();
                    p.StartInfo.FileName = programFullPath;
                    p.StartInfo.Arguments = args + " -o " + guid + ".tmp";
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
                    string result = p.StandardOutput.ReadToEnd();
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

                        //var bytesToReturs = System.IO.File.ReadAllBytes(resultPath);

                        byte[] bytesToReturs = null;


                       


                        //using (var file = new FileStream(resultPath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, false))
                        {
                            //bytesToReturs = new byte[file.Length];
                            var file = new FileStream(resultPath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, false);
                            var test = new FileStreamResult(file, "application/octet-stream");
                            return test;

                            //int bytesRead = await file.ReadAsync(bytesToReturs, 0, (int)file.Length);


                          
                        }
                        try
                        {
                            System.IO.File.Delete(resultPath);
                        }
                        catch
                        {

                        }
                        var resToReturn = File(bytesToReturs, "text/html");
                        return resToReturn;
                    }
                    return  Json("No such output directory " + resultPath + result + error + "search locations: " + sb.ToString());



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
    }
}