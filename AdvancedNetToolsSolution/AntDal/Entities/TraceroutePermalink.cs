using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntDal.Entities
{
    public class TraceroutePermalink : PermalinkBase
    {
        public TraceroutePermalink()
        {
            PingResponseSummaries = new List<PingResponseSummary>();
        }
               
        public virtual List<PingResponseSummary> PingResponseSummaries { get; set; }
    }
}
