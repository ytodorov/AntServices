using SmartAdminMvc.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
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

            var times = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            int.TryParse(times[0], out hop);
            double.TryParse(times[1], NumberStyles.Any, CultureInfo.InvariantCulture, out rtt);

            if (times.Length == 5)
            {
                address = times[3];
                ip = times[4].Substring(1, times[4].Length - 2);
            }
            else if (times.Length == 4)
            {
                address = "";
                ip = times[3];
            }
            else
            {
                double.TryParse(times[1], out rtt);
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
            var lines = traceRouteSummary.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            var correctLines = new List<string>();

            foreach (string line in lines)
            {
                var words = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                var firstWord = words.FirstOrDefault();
                int helper;
                if (int.TryParse(firstWord, out helper))
                {
                    correctLines.Add(line);
                }
            }
            return correctLines;

        }
    }
}