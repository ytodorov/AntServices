using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntDal.Entities
{
    public class PortPermalink : PermalinkBase
    {
        public PortPermalink()
        {
            PortResponseSummaries = new List<PortResponseSummary>();
        }

        public string DestinationIpAddress { get; set; }

        public int? OpenPortsCount { get; set; }

        public virtual List<PortResponseSummary> PortResponseSummaries { get; set; }
    }
}
