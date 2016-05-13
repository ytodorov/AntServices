using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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

        [NotMapped]
        public string Location
        {
            get
            {
                var res = string.Empty;

                res = TracerouteResponseSummary.SourceHostName;
                return res;
            }
        }
        

        public int? TracerouteResponseSummaryId { get; set; }
        public virtual TracerouteResponseSummary TracerouteResponseSummary { get; set; }
    }
}
