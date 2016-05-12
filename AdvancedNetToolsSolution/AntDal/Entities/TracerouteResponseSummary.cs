using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntDal.Entities
{
    public class TracerouteResponseSummary : EntityBase
    {
        public TracerouteResponseSummary()
        {
            TracerouteResponseDetails = new List<TracerouteResponseDetail>();
        }


        public string SourceIpAddress { get; set; }

        public string SourceHostName { get; set; }

        public int? TraceroutePermalinkId { get; set; }
        public virtual TraceroutePermalink TraceroutePermalink { get; set; }

        public virtual List<TracerouteResponseDetail> TracerouteResponseDetails { get; set; }
    }
}
