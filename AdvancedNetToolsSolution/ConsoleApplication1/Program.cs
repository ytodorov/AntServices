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
            string path = @"C:\Program Files (x86)\Nmap\nmap.exe";
            string args2 = "-sn 8.8.8.8";
            Process p = new Process();
            p.StartInfo.FileName = path;
            p.StartInfo.Arguments = args2;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.UseShellExecute = false;
            p.Start();
            Console.WriteLine(p.StandardOutput.ReadToEnd());
           
            //var st = Process.Start(path, args);

        }
    }
}
