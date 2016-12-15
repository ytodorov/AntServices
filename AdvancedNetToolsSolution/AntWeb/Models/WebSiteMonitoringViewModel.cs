using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartAdminMvc.Models
{
    public class WebSiteMonitoringViewModel : ViewModelBase
    {
        public string Url { get; set; }

        public int? PingInterval { get; set; }
    }
}