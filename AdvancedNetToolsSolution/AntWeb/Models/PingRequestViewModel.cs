using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartAdminMvc.Models
{
    public class PingRequestViewModel
    {
        public string Ip { get; set; }

        public int PacketsCount { get; set; }

        public int PacketsSize { get; set; }

        public int DelayBetweenPings { get; set; }


        public int Ttl { get; set; }

        public bool Df { get; set; }

      


    }
}