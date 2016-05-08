using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartAdminMvc.Models
{
    public class SpeedPermalinkViewModel : PermalinkBaseViewModel
    {
        public List<SpeedResponseSummaryViewModel> SpeedResponseSummaries { get; set; }
    }
}