using SmartAdminMvc.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace SmartAdminMvc.Infrastructure
{
    public static class PortParser
    {
        public static List<PortReplyModel> ParseSummary(string portSummary)
        {
            List<PortReplyModel> result = new List<PortReplyModel>();

            List<string> lines = GetLinesFromSummary(portSummary);
            foreach (var line in lines)
            {
                PortReplyModel prvm = ParseSingleLine(line);
                result.Add(prvm);
            }
            return result;
        }

        public static PortReplyModel ParseSingleLine(string line)
        {
            PortReplyModel result = new PortReplyModel();
            int port;
            string protocol, state, service;
            var times = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            var splitPortAndProtocol = times[0].Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            int.TryParse(splitPortAndProtocol[0], out port);

            result.Port = port;
            result.Protocol = splitPortAndProtocol[1];
            result.State = times[1];
            result.Service = times[2];

            return result;
        }

        private static List<string> GetLinesFromSummary(string portSummary)
        {
            var lines = portSummary.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

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