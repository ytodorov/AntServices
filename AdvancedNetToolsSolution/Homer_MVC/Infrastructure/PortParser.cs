using Homer_MVC.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Homer_MVC.Infrastructure
{
    public static class PortParser
    {
        public static List<PortViewModel> ParseSummary(string portSummary)
        {
            List<PortViewModel> result = new List<PortViewModel>();

            List<string> lines = GetLinesFromSummary(portSummary);
            foreach (var line in lines)
            {
                PortViewModel prvm = ParseSingleLine(line);
                result.Add(prvm);
            }
            return result;
        }

        public static PortViewModel ParseSingleLine(string line)
        {
            PortViewModel result = new PortViewModel();
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
                var splitPortAndProtocol = firstWord.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
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