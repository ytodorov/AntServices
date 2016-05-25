using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartAdminMvc.Models
{
    public class SpeedResponseSummaryViewModel : ViewModelBase
    {
        public string DestinationIpAddress { get; set; }

        public string DestinationHostName { get; set; }

        public int? BitsPerSecond { get; set; }

        public int? SpeedPermalinkId { get; set; }
    }
}