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
        public static PingReplySummaryViewModel ParseSummary(string summary)
        {
            PingReplySummaryViewModel result = new PingReplySummaryViewModel();

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

                double maxRtt = double.Parse(times[0], CultureInfo.InvariantCulture);
                double minRtt = double.Parse(times[1], CultureInfo.InvariantCulture);
                double avgRtt = double.Parse(times[2], CultureInfo.InvariantCulture);

                result.MaxRtt = maxRtt;
                result.MinRtt = minRtt;
                result.AvgRtt = avgRtt;
            }
            return result;               

        }
    }
}