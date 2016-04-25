using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Xunit;

namespace UnitTests
{
    public class PingTests
    {
        [Fact]
        public void ParsePintSummaryTest()
        {
            using (HttpClient client = new HttpClient())
            {

                var encodedArgs0 = Uri.EscapeDataString(" 8.8.8.8 --tcp -p 53 --delay 200ms -v1");
                string url = "http://ants-je.cloudapp.net/home/exec?program=nping&args=" + encodedArgs0;
                var res = client.GetStringAsync(url).Result;

                var lines = res.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                var firstLine = lines.First(s => s.StartsWith("max", StringComparison.InvariantCultureIgnoreCase));

                if (!string.IsNullOrEmpty(firstLine))
                {
                    var timesAndEmptyStrings = firstLine.Replace("Max rtt:", string.Empty)
                        .Replace("Min rtt:", string.Empty)
                        .Replace("Avg rtt:", string.Empty)
                        .Replace("|", string.Empty)
                        .Replace("ms", string.Empty);
                        

                    var times = timesAndEmptyStrings.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                    double maxRtt = double.Parse(times[0], CultureInfo.InvariantCulture);
                    double minRtt = double.Parse(times[1], CultureInfo.InvariantCulture);
                    double avgRtt = double.Parse(times[2], CultureInfo.InvariantCulture);


                }

                var secondLine = lines.First(s => s.StartsWith("raw", StringComparison.InvariantCultureIgnoreCase));

                if (!string.IsNullOrEmpty(secondLine))
                {
                    var timesAndEmptyStrings = secondLine.Replace("Raw packets sent:", string.Empty)
                        .Replace("Min rtt:", string.Empty)
                        .Replace("Avg rtt:", string.Empty)
                        .Replace("|", string.Empty)
                        .Replace("ms", string.Empty);


                    var times = timesAndEmptyStrings.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                    double packetsSent = double.Parse(times[0], CultureInfo.InvariantCulture);
                    double packetsReceived = double.Parse(times[3], CultureInfo.InvariantCulture);
                    
                }


                var thirdLine = lines.First(s => s.StartsWith("Tx", StringComparison.InvariantCultureIgnoreCase));

                if (!string.IsNullOrEmpty(thirdLine))
                {
                    var timesAndEmptyStrings = thirdLine.Replace("Tx pkts/s:", string.Empty)
                        .Replace("Tx time:", string.Empty)
                        .Replace("Tx bytes/s:", string.Empty)
                        .Replace("|", string.Empty)
                        .Replace("s", string.Empty);


                    var times = timesAndEmptyStrings.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                    double txTime = double.Parse(times[0], CultureInfo.InvariantCulture);
                    double txBytes = double.Parse(times[1], CultureInfo.InvariantCulture);
                    double txPAckets = double.Parse(times[2], CultureInfo.InvariantCulture);


                }
                var fourthLine = lines.First(s => s.StartsWith("Rx", StringComparison.InvariantCultureIgnoreCase));

                if (!string.IsNullOrEmpty(fourthLine))
                {
                    var timesAndEmptyStrings = fourthLine.Replace("Rx pkts/s:", string.Empty)
                        .Replace("Rx time:", string.Empty)
                        .Replace("Rx bytes/s:", string.Empty)
                        .Replace("|", string.Empty)
                        .Replace("s", string.Empty);


                    var times = timesAndEmptyStrings.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                    double rxTime = double.Parse(times[0], CultureInfo.InvariantCulture);
                    double rxBytesPerSecond = double.Parse(times[1], CultureInfo.InvariantCulture);
                    double rxPacketsPerSecond = double.Parse(times[2], CultureInfo.InvariantCulture);



                }

            }
        }


        [Fact]
        public void MeasurePingTimings()
        {
            List<long> timings = new List<long>();
            List<string> results = new List<string>();
            var addresses = SmartAdminMvc.Infrastructure.Utils.GetDeployedServicesUrlAddresses;//.Skip(1).ToList();
            foreach (var urlService in addresses)
            {
                Stopwatch sw = Stopwatch.StartNew();

                using (HttpClient client = new HttpClient())
                {

                    var encodedArgs0 = Uri.EscapeDataString(" 8.8.8.8 --tcp -p 53 --delay 10ms -v1");
                    string url = $"{urlService}/home/exec?program=nping&args=" + encodedArgs0;
                    var res = client.GetStringAsync(url).Result;
                    results.Add(res);
                }
                timings.Add(sw.ElapsedMilliseconds);
            }
        }

        [Fact]
        public void MeasurePingTimings2()
        {
            List<long> timings = new List<long>();
            foreach (var urlService in SmartAdminMvc.Infrastructure.Utils.GetDeployedServicesUrlAddresses)
            {
                Stopwatch sw = Stopwatch.StartNew();

                using (HttpClient client = new HttpClient())
                {

                    var encodedArgs0 = Uri.EscapeDataString(" 8.8.8.8 --delay 10ms -v1");
                    string url =urlService;
                    var res = client.GetStringAsync(url).Result;
                }
                timings.Add(sw.ElapsedMilliseconds);
            }
        }
    }
}
