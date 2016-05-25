using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace SmartAdminMvc.Models
{
    public class PingPermalinkViewModel : PermalinkBaseViewModel
    {



        public List<PingResponseSummaryViewModel> PingResponseSummaries { get; set; }

    }
}