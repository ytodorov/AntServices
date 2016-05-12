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
            TracerouteResponseSummaries = new List<TracerouteResponseSummary>();
        }
               
        public virtual List<TracerouteResponseSummary> TracerouteResponseSummaries { get; set; }
    }
}
