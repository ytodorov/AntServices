using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntDal.Entities
{
    public class SpeedPermalink : PermalinkBase
    {
        public SpeedPermalink()
        {
            SpeedResponseSummaries = new List<SpeedResponseSummary>();
        }

        public virtual List<SpeedResponseSummary> SpeedResponseSummaries { get; set; }
    }
}
