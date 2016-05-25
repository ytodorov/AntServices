using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartAdminMvc.Models
{
    public class TracerouteResponseDetailViewModel
    {
        public int Hop { get; set; }

        public double Rtt { get; set; }

        public string AddressName { get; set; }

        public string Ip { get; set; }

        public string Location { get; set; }

    }
}