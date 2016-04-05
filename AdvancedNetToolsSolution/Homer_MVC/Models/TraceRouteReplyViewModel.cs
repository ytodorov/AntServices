using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartAdminMvc.Models
{
    public class TraceRouteReplyViewModel
    {
        public int Hop { get; set; }

        public double Rtt { get; set; }

        public string AddressName { get; set; }

        public string Ip { get; set; }
    }
}