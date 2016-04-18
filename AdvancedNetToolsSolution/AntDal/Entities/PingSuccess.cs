using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntDal.Entities
{
    public class PingSuccess : EntityBase
    {
        public TimeSpan TimeNeeded { get; set; }
        public string StatusMessage { get; set; }
        public string IpAddress { get; set; }
        public bool Successful { get; set; }

    }
}
