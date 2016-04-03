using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartAdminMvc.Models
{
    public class PingPermalinkViewModel
    {
        public int Id { get; set; }
        public string Ip { get; set; }

        public string GoogleMapString { get; set; }

        public List<PingResponseSummaryViewModel> PingResponseSummaries { get; set; }
    }
}