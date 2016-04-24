using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntDal.Entities
{
    public class PingResponseSummary : EntityBase
    {
        public string SourceIpAddress { get; set; }

        public string SourceHostName { get; set; }

        public string DestinationIpAddress { get; set; }

        public string DestinationHostName { get; set; }

        public double? MaxRtt { get; set; }

        public double? MinRtt { get; set; }

        public double? AvgRtt { get; set; }

        public int? PacketsSent { get; set; }
        public int? PacketsReceived { get; set; }

        public double? Latency { get; set; }


        public int? PingPermalinkId { get; set; }
        public virtual PingPermalink PingPermalink { get; set; }
    }
}
