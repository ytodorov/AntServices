using SmartAdminMvc.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace SmartAdminMvc.Infrastructure
{
    public static class PingReplyParser
    {
        public static PingResponseSummaryViewModel ParseSummary(string summary)
        {
            /*Max rtt: 4.000ms | Min rtt: 3.000ms | Avg rtt: 3.599ms
              Raw packets sent: 5 (210B) | Rcvd: 5 (230B) | Lost: 0 (0.00%)
              Tx time: 0.15100s | Tx bytes/s: 1390.73 | Tx pkts/s: 33.11
              Rx time: 0.15400s | Rx bytes/s: 1493.51 | Rx pkts/s: 32.47*/

            var result = new PingResponseSummaryViewModel();

            // destinationIp SENT (0.8440s) ICMP [10.0.0.4 > 8.8.8.8 Echo request (type=8/code=0) id=24130 seq=1] IP [ttl=64 id=40491 proto=1 csum=0xc2a2 iplen=28 ]
            // SENT (0.2780s) TCP [100.104.66.29:20131 > 216.58.200.110:80 S seq=918634342 win=1480 csum=0x9DD1] IP [ttl=64 id=4649 proto=6 csum=0x2179 iplen=40 ]
            int firstIndexOfGreaterThan = summary.IndexOf(value: ">");
            int firstIndexOfEchoRequest = summary.IndexOf(value: " S seq=");
            int lengthOfIp = firstIndexOfEchoRequest - firstIndexOfGreaterThan;
            string ipWithPort = summary.Substring(firstIndexOfGreaterThan + 1, lengthOfIp - 1).Trim();
            result.DestinationIpAddress = ipWithPort.Substring(startIndex: 0, length: ipWithPort.IndexOf(value: ":"));

            string[] lines = summary.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            string firstLine = lines.First(s => s.StartsWith(value: "max", comparisonType: StringComparison.InvariantCultureIgnoreCase));

            if (!string.IsNullOrEmpty(firstLine))
            {
                string timesAndEmptyStrings = firstLine.Replace(oldValue: "Max rtt:", newValue: string.Empty)
                    .Replace(oldValue: "Min rtt:", newValue: string.Empty)
                    .Replace(oldValue: "Avg rtt:", newValue: string.Empty)
                    .Replace(oldValue: "|", newValue: string.Empty)
                    .Replace(oldValue: "ms", newValue: string.Empty);


                string[] times = timesAndEmptyStrings.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                double maxRtt;
                double.TryParse(times[0], NumberStyles.Any, CultureInfo.InvariantCulture, out maxRtt);
                double minRtt;
                double.TryParse(times[1], NumberStyles.Any, CultureInfo.InvariantCulture, out minRtt);
                double avgRtt;
                double.TryParse(times[2], NumberStyles.Any, CultureInfo.InvariantCulture, out avgRtt);

                result.MaxRtt = maxRtt;
                result.MinRtt = minRtt;
                result.AvgRtt = avgRtt;
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

                int packetsSent = int.Parse(times[0], CultureInfo.InvariantCulture);
                int packetsReceived = int.Parse(times[3], CultureInfo.InvariantCulture);
                result.PacketsSent = packetsSent;
                result.PacketsReceived = packetsReceived;
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
                double txBytesPerSecond = double.Parse(times[1], CultureInfo.InvariantCulture);
                double txPacketsPerSecond = double.Parse(times[2], CultureInfo.InvariantCulture);

                result.TxTimeInSeconds = txTime;
                result.TxBytesPerSecond = txBytesPerSecond;
                result.TxPacketsPerSecond = txPacketsPerSecond;


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

                result.RxTimeInSeconds = rxTime;
                result.RxBytesPerSecond = rxBytesPerSecond;
                result.RxPacketsPerSecond = rxPacketsPerSecond;


            }
            return result;

        }
    }
}