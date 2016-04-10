using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartAdminMvc.Models
{
    public class PortResponseSummaryViewModel : ViewModelBase
    {
        public int PortNumber { get; set; }

        public string Protocol { get; set; }

        public string State { get; set; }

        public string Service { get; set; }

        public string Version { get; set; }
    }
}