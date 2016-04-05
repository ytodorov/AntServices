using Homer_MVC.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Homer_MVC.Infrastructure
{
    public static class PingReplyParser
    {
        public static PingResponseSummaryViewModel ParseSummary(string summary)
        {
            /*Max rtt: 4.000ms | Min rtt: 3.000ms | Avg rtt: 3.599ms
              Raw packets sent: 5 (210B) | Rcvd: 5 (230B) | Lost: 0 (0.00%)
              Tx time: 0.15100s | Tx bytes/s: 1390.73 | Tx pkts/s: 33.11
              Rx time: 0.15400s | Rx bytes/s: 1493.51 | Rx pkts/s: 32.47*/

            PingResponseSummaryViewModel result = new PingResponseSummaryViewModel();

            var lines = summary.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            var firstLine = lines.First(s => s.StartsWith("max", StringComparison.InvariantCultureIgnoreCase));

            if (!string.IsNullOrEmpty(firstLine))
            {
                var timesAndEmptyStrings = firstLine.Replace("Max rtt:", string.Empty)
                    .Replace("Min rtt:", string.Empty)
                    .Replace("Avg rtt:", string.Empty)
                    .Replace("|", string.Empty)
                    .Replace("ms", string.Empty);


                var times = timesAndEmptyStrings.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

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
                double txBytesPerSecond = double.Parse(times[1], CultureInfo.InvariantCulture);
                double txPacketsPerSecond = double.Parse(times[2], CultureInfo.InvariantCulture);

                result.TxTimeInSeconds = txTime;
                result.TxBytesPerSecond = txBytesPerSecond;
                result.TxPacketsPerSecond = txPacketsPerSecond;


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

                result.RxTimeInSeconds = rxTime;
                result.RxBytesPerSecond = rxBytesPerSecond;
                result.RxPacketsPerSecond = rxPacketsPerSecond;


            }
            return result;               

        }
    }
}