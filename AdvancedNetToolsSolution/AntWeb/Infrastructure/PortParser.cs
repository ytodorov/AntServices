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
        public static List<PortResponseSummaryViewModel> ParseSummary(string portSummary)
        {
            var result = new List<PortResponseSummaryViewModel>();

            List<string> lines = GetLinesFromSummary(portSummary);
            foreach (var line in lines)
            {
                PortResponseSummaryViewModel prvm = ParseSingleLine(line);
                result.Add(prvm);
            }
            result = result.Where(r => r != null).ToList();
            return result;
        }

        public static PortResponseSummaryViewModel ParseSingleLine(string line)
        {
            var result = new PortResponseSummaryViewModel();
            int port;
            string[] times = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            if (times.Length > 0)
            {
                string[] splitPortAndProtocol = times[0].Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                int.TryParse(splitPortAndProtocol[0], out port);

                result.PortNumber = port;
                if (splitPortAndProtocol.Length == 2)
                {
                    result.Protocol = splitPortAndProtocol[1];
                }
                else
                {
                    return null;
                }
            }
            if (times.Length > 1)
            {
                result.State = times[1];
            }
            if (times.Length > 2)
            {
                result.Service = times[2];
            }
            if (times.Length > 3)
            {
                result.Version = times[3];
            }
            if (times.Length > 4)
            {
                result.Version += times[4];
            }

            return result;
        }

        private static List<string> GetLinesFromSummary(string portSummary)
        {
            string[] lines = portSummary.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            var correctLines = new List<string>();

            foreach (string line in lines)
            {
                string[] words = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                string firstWord = words.FirstOrDefault();
                string[] splitPortAndProtocol = firstWord.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                int helper;
                if (int.TryParse(splitPortAndProtocol[0], out helper))
                {
                    correctLines.Add(line);
                }
            }
            return correctLines;

        }
    }
}