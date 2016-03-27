using SmartAdminMvc.Models;
using System;
using System.Collections.Generic;
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
            return new TraceRouteReplyViewModel();
        }

        private static List<string> GetLinesFromSummary(string traceRouteSummary)
        {
            return new List<string>();
        }


    }
}