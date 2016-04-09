using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntDal.Entities
{
    public class PingPermalink : EntityBase
    {
        public PingPermalink()
        {
            PingResponseSummaries = new List<PingResponseSummary>();
        }

        public string DestinationAddress { get; set; }

        public string UserCreatedIpAddress { get; set; }

        public bool? ShowInHistory { get; set; }

        public virtual List<PingResponseSummary> PingResponseSummaries { get; set; }
    }
}
