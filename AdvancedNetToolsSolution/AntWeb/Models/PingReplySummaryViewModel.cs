﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartAdminMvc.Models
{
    public class PingReplySummaryViewModel
    {
        public string Address { get; set; }

        public double MaxRtt { get; set; }

        public double MinRtt { get; set; }

        public double AvgRtt { get; set; }

        public int PacketsCountSent { get; set; }

        public int PacketsBytesSent { get; set; }

        public int PacketsCountReceived { get; set; }

        public int PacketsBytesReceived { get; set; }

        public int PacketsCountLost { get; set; }

        public int PacketsBytesLost { get; set; }

        public double PercentageLost { get; set; }

        public double TxTime { get; set; }

        public double TxBytesPerSecond { get; set; }

        public double TxPacketsPerSecond { get; set; }

        public double RxTime { get; set; }

        public double RxBytesPerSecond { get; set; }

        public double RxPacketsPerSecond { get; set; }


    }
}