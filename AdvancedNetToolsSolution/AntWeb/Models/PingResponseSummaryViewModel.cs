﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartAdminMvc.Models
{
    public class PingResponseSummaryViewModel : ViewModelBase
    {
        public string SourceIpAddress { get; set; }

        public string SourceHostName { get; set; }

        public string DestinationIpAddress { get; set; }

        public string DestinationHostName { get; set; }

        public string AddressInfoSummary
        {
            get
            {
                string result = $"From {SourceIpAddress} ({SourceHostName}) To {DestinationIpAddress}";
                if (string.IsNullOrEmpty(value: ""))
                {
                    result += $"({DestinationHostName})";
                }
                return result;
            }
        }

        public double? MaxRtt { get; set; }

        public double? MinRtt { get; set; }

        public double? AvgRtt { get; set; }

        public int? PacketsSent { get; set; }

        public int PacketsBytesSent { get; set; }

        public int PacketsCountReceived { get; set; }

        public int? PacketsReceived { get; set; }

        public int PacketsCountLost { get; set; }

        public int PacketsBytesLost { get; set; }

        public double PercentageLost { get; set; }

        public double TxTimeInSeconds { get; set; }

        public double TxBytesPerSecond { get; set; }

        public double TxPacketsPerSecond { get; set; }

        public double RxTimeInSeconds { get; set; }

        public double RxBytesPerSecond { get; set; }

        public double RxPacketsPerSecond { get; set; }

        public double? Latency { get; set; }



    }
}