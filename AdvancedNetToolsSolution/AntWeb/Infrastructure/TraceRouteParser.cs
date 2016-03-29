using SmartAdminMvc.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace SmartAdminMvc.Infrastructure
{
    public static class TraceRouteParser
    {
        public static List<TraceRouteReplyViewModel> ParseSummary(string traceRouteSummary)
        {
            List<TraceRouteReplyViewModel> result = new List<TraceRouteReplyViewModel>();
            
            List<string> lines = GetLinesFromSummary(traceRouteSummary);
            foreach (var line in lines)
            {
                TraceRouteReplyViewModel trrvm = ParseSingleLine(line);
                result.Add(trrvm);
            }

            return result;
        }

        public static TraceRouteReplyViewModel ParseSingleLine(string line)
        {
            TraceRouteReplyViewModel result = new TraceRouteReplyViewModel();
            int hop;
            double rtt;
            string address, ip;

            var times = line.Split(new string[] {" "}, StringSplitOptions.RemoveEmptyEntries);
            int.TryParse(times[0], out hop);
            
            if (times.Length == 4)
            {
                double.TryParse(times[1], NumberStyles.Any, CultureInfo.InvariantCulture, out rtt);
                if (!string.IsNullOrEmpty(times[3]))
                {
                    address = times[3];
                    ip = times[4];
                }
                else
                {
                    address = "";
                    ip = times[3];
                }
                ip = ip.Substring(1, ip.Length - 2);
            }
            else
            {
                double.TryParse(times[0], out rtt);
                address = "";
                ip = "";
            }
            result.Hop = hop;
            result.Rtt = rtt;
            result.AddressName = address;
            result.Ip = ip;

            return result;
        }

        private static List<string> GetLinesFromSummary(string traceRouteSummary)
        {
            var pFrom = traceRouteSummary.IndexOf("1 ") + "1 ".Length;
            var pTo = traceRouteSummary.LastIndexOf("Nmap");
            var res = traceRouteSummary.Substring(pFrom, pTo - pFrom);
            List<string> lines = new List<string>();
            lines.Add(res);
            return lines;
        }
    }
}