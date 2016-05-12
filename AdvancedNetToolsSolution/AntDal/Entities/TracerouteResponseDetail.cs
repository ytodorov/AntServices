using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntDal.Entities
{
    public class TracerouteResponseDetail : EntityBase
    {
        public int Hop { get; set; }

        public double Rtt { get; set; }

        public string AddressName { get; set; }

        public string Ip { get; set; }

        public int? TracerouteResponseSummaryId { get; set; }
        public virtual TracerouteResponseSummary TracerouteResponseSummary { get; set; }
    }
}
