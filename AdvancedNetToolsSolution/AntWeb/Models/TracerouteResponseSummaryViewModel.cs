using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartAdminMvc.Models
{
    public class TracerouteResponseSummaryViewModel : ViewModelBase
    {
        public string SourceIpAddress { get; set; }

        public string SourceHostName { get; set; }

        public int? TraceroutePermalinkId { get; set; }

        public List<TracerouteResponseDetailViewModel> TracerouteResponseDetails { get; set; }

    }
}