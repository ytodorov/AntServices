using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartAdminMvc.Models
{
    public class PingPermalinkViewModel
    {
        public string Ip { get; set; }

        public List<PingResponseSummaryViewModel> PingResponseSummaries { get; set; }
    }
}