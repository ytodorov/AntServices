using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartAdminMvc.Models
{
    public class PortPermalinkViewModel : PermalinkBaseViewModel
    {
        public List<PortResponseSummaryViewModel> PortResponseSummaries { get; set; }

        public string DestinationIpAddress { get; set; }

        public int? OpenPortsCount { get; set; }

        public bool ForcePortScan { get; set; }

        public override string PermalinkAddress
        {
            get
            {
                var result = base.PermalinkAddress;
                if (!string.IsNullOrEmpty(DestinationAddress))
                {
                    int indexOfQuote = result.IndexOf("?");
                    result = result.Substring(0, indexOfQuote);
                    result = $"{result}?url={DestinationAddress}&ip={DestinationIpAddress}&id={Id}";
                }
                else
                {
                    result = $"{result}?ip={DestinationIpAddress}&id={Id}";
                }
                return result;
            }
        }
    }
}