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
            var times = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            int hop;
            int.TryParse(times[0], out hop);
            double rtt;
            string address;
            string ip;
            if (!string.IsNullOrEmpty(times[4]))
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
            }
            else
            {
                double.TryParse(times[2], out rtt);
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