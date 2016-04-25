using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using System.Diagnostics;
using System.Net.Http;
using System.Timers;

namespace PingAllLocationsWebJob
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {      
        static void Main()
        {
            var timer = new Timer(TimeSpan.FromMinutes(value: 5).TotalMilliseconds);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
            PingAllLocations();          

        }

        private static void PreventWebJobFromIdle_Elapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine(e.SignalTime);
        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            PingAllLocations();
        }

        static void PingAllLocations()
        {
#pragma warning disable JustCode_NamingConventions // Naming conventions inconsistency
            var GetDeployedServicesUrlAddresses = new List<string>()
#pragma warning restore JustCode_NamingConventions // Naming conventions inconsistency
            {
                "http://toolsfornet.com",
                "http://ants-ea.cloudapp.net", // "40.83.125.9",
                "http://ants-je.cloudapp.net", // "13.71.155.140"
                "http://ants-sea.cloudapp.net", // "13.76.100.42"
                "http://ant-cus.cloudapp.net", // "52.165.26.10"
                "http://ant-jw.cloudapp.net", // "40.74.137.95"
                "http://ants-eus.cloudapp.net", // "13.90.212.72"
                "http://ants-neu.cloudapp.net", // "13.74.188.161"
                "http://ants-scus.cloudapp.net", // "104.214.70.79"
                "http://ants-weu.cloudapp.net", // "104.46.38.89"
                "http://ants-wus.cloudapp.net" // "13.93.211.38"
            };

            foreach (var urlService in GetDeployedServicesUrlAddresses)
            {
                Stopwatch sw = Stopwatch.StartNew();

                using (HttpClient client = new HttpClient())
                {

                    string encodedArgs0 = Uri.EscapeDataString(stringToEscape: " 8.8.8.8 --delay 10ms -v1");
                    string res = client.GetStringAsync(urlService).Result;
                    Console.WriteLine($"{sw.ElapsedMilliseconds} {urlService}");


                }

                using (HttpClient client = new HttpClient())
                {
                    sw = Stopwatch.StartNew();
                    string encodedArgs0 = Uri.EscapeDataString(stringToEscape: " 8.8.8.8  --tcp -p 53 --delay 10ms -v1 -c 1");
                    string url = $"{urlService}/home/exec?program=nping&args=" + encodedArgs0;
                    string res = client.GetStringAsync(url).Result;
                    if (res.Length > 100)
                    {
                        res = res.Substring(startIndex: 0, length: 100);
                    }
                    Console.WriteLine($"{sw.ElapsedMilliseconds} nping");
                }
                using (HttpClient client = new HttpClient())
                {
                    sw = Stopwatch.StartNew();
                    string encodedArgs0 = Uri.EscapeDataString(stringToEscape: " 8.8.8.8 -Pn -sn -n");
                    string url = $"{urlService}/home/exec?program=nmap&args=" + encodedArgs0;
                    string res = client.GetStringAsync(url).Result;
                    if (res.Length > 100)
                    {
                        res = res.Substring(startIndex: 0, length: 100);
                    }
                    Console.WriteLine($"{sw.ElapsedMilliseconds} nmap");
                }

                Console.WriteLine(value: "--------------");

            }
        }
    }
}
