using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace SmartAdminMvc.Models
{
    public class TraceroutePermalinkViewModel : PermalinkBaseViewModel
    {
        public List<TracerouteResponseSummaryViewModel> TracerouteResponseSummaries { get; set; }

        public List<TracerouteResponseDetailViewModel> AllDetails
        {
            get
            {
                List<TracerouteResponseDetailViewModel> details = new List<TracerouteResponseDetailViewModel>();
                foreach (TracerouteResponseSummaryViewModel s in TracerouteResponseSummaries)
                {
                    details.AddRange(s.TracerouteResponseDetails);
                }
                return details;
            }
        }
    }
}