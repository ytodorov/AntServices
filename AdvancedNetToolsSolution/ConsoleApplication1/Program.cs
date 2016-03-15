using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var fresult = string.Empty;
            try
            {
                var abp = Environment.CurrentDirectory;
                //C:\Projects\AdvancedNetToolsServicesRepo\AdvancedNetToolsSolution\src\AdvancedNetToolsServicesWeb\nmap.exe


                string path = abp + "\\psping.exe";
                string args2 = "-i 0 -n 10 8.8.8.8:53";
                Process p = new Process();
                p.StartInfo.FileName = path;
                p.StartInfo.Arguments = args2;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                
                p.StartInfo.UseShellExecute = false;

                p.Start();

                //Thread.Sleep(10000);

                //string result = "";
                var result = p.StandardOutput.ReadToEnd();
                string error = p.StandardError.ReadToEnd();
                fresult = result + "ГРЕШКИ" + error;
            }
            catch (Exception ex)
            {
                fresult = ex.Message;
            }
            Console.WriteLine(fresult);

        }

    }
}
