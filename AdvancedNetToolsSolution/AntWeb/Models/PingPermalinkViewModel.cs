using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SmartAdminMvc.Models
{
    public class PingPermalinkViewModel
    {
        public int Id { get; set; }
        public string Ip { get; set; }

        public string GoogleMapString { get; set; }

        public List<PingResponseSummaryViewModel> PingResponseSummaries { get; set; }

        public string PingResponsesIpAddress
        {
            get
            {
                StringBuilder result = new StringBuilder();
                for (int i = 0; i < PingResponseSummaries.Count; i++)
                {
                    result.Append(PingResponseSummaries[i].SourceIpAddress);
                    if (i != PingResponseSummaries.Count - 1)
                    {
                        result.Append(",");
                    }
                }
                string stringResult = result.ToString();
                return stringResult;
            }
            set
            {

            }

        }
    }
}