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
            Timer timer = new Timer(TimeSpan.FromMinutes(5).TotalMilliseconds);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
            PingAllLocations();
            Console.ReadLine();

            Timer preventWebJobFromIdle = new Timer(TimeSpan.FromMinutes(1.5).TotalMilliseconds);
            preventWebJobFromIdle.Elapsed += PreventWebJobFromIdle_Elapsed;
            preventWebJobFromIdle.Start();


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
            var GetDeployedServicesUrlAddresses = new List<string>()
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

                    var encodedArgs0 = Uri.EscapeDataString(" 8.8.8.8 --delay 10ms -v1");
                    var res = client.GetStringAsync(urlService).Result;
                    Console.WriteLine($"{sw.ElapsedMilliseconds} {urlService}");
                }
            }
        }
    }
}
