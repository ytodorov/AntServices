using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntDal.Entities
{
    public class SpeedResponseSummary : EntityBase
    {
        public string DestinationIpAddress { get; set; }

        public string DestinationHostName { get; set; }

        public int? BitsPerSecond { get; set; }

        public SpeedPermalink SpeedPermalink { get; set; }
        public int? SpeedPermalinkId { get; set; }

    }
}
