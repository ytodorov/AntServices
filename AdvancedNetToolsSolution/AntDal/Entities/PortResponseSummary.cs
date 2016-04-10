using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntDal.Entities
{
    public class PortResponseSummary : EntityBase
    {
        public int? PortNumber { get; set; }

        public string Protocol { get; set; }

        public string State { get; set; }

        public string Service { get; set; }

        public string Version { get; set; }

        public PortPermalink PortPermalink { get; set; }
        public int? PortPermalinkId { get; set; }

    }
}
