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
            Process remoteProcess = new Process();
            remoteProcess.StartInfo.UseShellExecute = false;
            remoteProcess.StartInfo.RedirectStandardOutput = true;
            remoteProcess.StartInfo.FileName = @"psexec";//need put tool psexec into executed path,eg: bin
                    remoteProcess.StartInfo.Arguments = @"\\" + FQDN + " cmd /c netstat -an";
            remoteProcess.Start();
            remoteProcess.WaitForExit(10000);
            string result = remoteProcess.StandardOutput.ReadToEnd();
            string[] r = { "\r\n" };
            string[] tempResult = result.Split(r, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 22; i < 55; i++)
            {
                TcpClient client = new TcpClient();
                client.SendTimeout = 100;
                client.ReceiveTimeout = 100;

                try
                {
                    client.Connect("8.8.8.8", i);
                    Console.WriteLine(client.Connected);
                }
                catch(SocketException ex)
                {
                    Console.WriteLine(ex.Message);
                }
              
            }
           
            
        }
    }
}
