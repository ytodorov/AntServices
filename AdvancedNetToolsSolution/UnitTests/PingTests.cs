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

                string encodedArgs0 = Uri.EscapeDataString(stringToEscape: " 8.8.8.8 --tcp -p 53 --delay 200ms -v1");
                string url = "http://ants-je.cloudapp.net/home/exec?program=nping&args=" + encodedArgs0;
                string res = client.GetStringAsync(url).Result;

                string[] lines = res.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                string firstLine = lines.First(s => s.StartsWith(value: "max", comparisonType: StringComparison.InvariantCultureIgnoreCase));

                if (!string.IsNullOrEmpty(firstLine))
                {
                    string timesAndEmptyStrings = firstLine.Replace(oldValue: "Max rtt:", newValue: string.Empty)
                        .Replace(oldValue: "Min rtt:", newValue: string.Empty)
                        .Replace(oldValue: "Avg rtt:", newValue: string.Empty)
                        .Replace(oldValue: "|", newValue: string.Empty)
                        .Replace(oldValue: "ms", newValue: string.Empty);
                        

                    string[] times = timesAndEmptyStrings.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                    double maxRtt = double.Parse(times[0], CultureInfo.InvariantCulture);
                    double minRtt = double.Parse(times[1], CultureInfo.InvariantCulture);
                    double avgRtt = double.Parse(times[2], CultureInfo.InvariantCulture);


                }

                string secondLine = lines.First(s => s.StartsWith(value: "raw", comparisonType: StringComparison.InvariantCultureIgnoreCase));

                if (!string.IsNullOrEmpty(secondLine))
                {
                    string timesAndEmptyStrings = secondLine.Replace(oldValue: "Raw packets sent:", newValue: string.Empty)
                        .Replace(oldValue: "Min rtt:", newValue: string.Empty)
                        .Replace(oldValue: "Avg rtt:", newValue: string.Empty)
                        .Replace(oldValue: "|", newValue: string.Empty)
                        .Replace(oldValue: "ms", newValue: string.Empty);


                    string[] times = timesAndEmptyStrings.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                    double packetsSent = double.Parse(times[0], CultureInfo.InvariantCulture);
                    double packetsReceived = double.Parse(times[3], CultureInfo.InvariantCulture);
                    
                }


                string thirdLine = lines.First(s => s.StartsWith(value: "Tx", comparisonType: StringComparison.InvariantCultureIgnoreCase));

                if (!string.IsNullOrEmpty(thirdLine))
                {
                    string timesAndEmptyStrings = thirdLine.Replace(oldValue: "Tx pkts/s:", newValue: string.Empty)
                        .Replace(oldValue: "Tx time:", newValue: string.Empty)
                        .Replace(oldValue: "Tx bytes/s:", newValue: string.Empty)
                        .Replace(oldValue: "|", newValue: string.Empty)
                        .Replace(oldValue: "s", newValue: string.Empty);


                    string[] times = timesAndEmptyStrings.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                    double txTime = double.Parse(times[0], CultureInfo.InvariantCulture);
                    double txBytes = double.Parse(times[1], CultureInfo.InvariantCulture);
                    double txPAckets = double.Parse(times[2], CultureInfo.InvariantCulture);


                }
                string fourthLine = lines.First(s => s.StartsWith(value: "Rx", comparisonType: StringComparison.InvariantCultureIgnoreCase));

                if (!string.IsNullOrEmpty(fourthLine))
                {
                    string timesAndEmptyStrings = fourthLine.Replace(oldValue: "Rx pkts/s:", newValue: string.Empty)
                        .Replace(oldValue: "Rx time:", newValue: string.Empty)
                        .Replace(oldValue: "Rx bytes/s:", newValue: string.Empty)
                        .Replace(oldValue: "|", newValue: string.Empty)
                        .Replace(oldValue: "s", newValue: string.Empty);


                    string[] times = timesAndEmptyStrings.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                    double rxTime = double.Parse(times[0], CultureInfo.InvariantCulture);
                    double rxBytesPerSecond = double.Parse(times[1], CultureInfo.InvariantCulture);
                    double rxPacketsPerSecond = double.Parse(times[2], CultureInfo.InvariantCulture);



                }

            }
        }


        [Fact]
        public void MeasurePingTimings()
        {
            var timings = new List<long>();
            var results = new List<string>();
            List<string> addresses = SmartAdminMvc.Infrastructure.Utils.GetDeployedServicesUrlAddresses;//.Skip(1).ToList();
            foreach (var urlService in addresses)
            {
                Stopwatch sw = Stopwatch.StartNew();

                using (HttpClient client = new HttpClient())
                {

                    string encodedArgs0 = Uri.EscapeDataString(stringToEscape: " 8.8.8.8 --tcp -p 53 --delay 10ms -v1");
                    string url = $"{urlService}/home/exec?program=nping&args=" + encodedArgs0;
                    string res = client.GetStringAsync(url).Result;
                    results.Add(res);
                }
                timings.Add(sw.ElapsedMilliseconds);
            }
        }

        [Fact]
        public void MeasurePingTimings2()
        {
            var timings = new List<long>();
            foreach (var urlService in SmartAdminMvc.Infrastructure.Utils.GetDeployedServicesUrlAddresses)
            {
                Stopwatch sw = Stopwatch.StartNew();

                using (HttpClient client = new HttpClient())
                {

                    string encodedArgs0 = Uri.EscapeDataString(stringToEscape: " 8.8.8.8 --delay 10ms -v1");
                    string url =urlService;
                    string res = client.GetStringAsync(url).Result;
                }
                timings.Add(sw.ElapsedMilliseconds);
            }
        }
    }
}
