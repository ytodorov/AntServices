using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartAdminMvc.Models
{
    public class PortPermalinkViewModel : PermalinkBaseViewModel
    {
        public List<PortResponseSummaryViewModel> PortResponseSummaries { get; set; }

        public bool ForcePortScan { get; set; }
    }
}