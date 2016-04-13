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

        public bool IsDestinationIpAddress
        {
            get
            {
                IPAddress dummy;
                var isIpAddress = IPAddress.TryParse(DestinationAddress, out dummy);
                return isIpAddress;
            }
        }

        public string PermalinkAddress
        {
            get
            {
                string result = string.Empty;

                if (HttpContext.Current?.Request != null)
                {
                    string fullRootUrl = HttpContext.Current?.Request.Url.OriginalString.Replace(HttpContext.Current?.Request.Url.LocalPath, string.Empty);
                    if (!HttpContext.Current.Request.Url.ToString().Contains("localhost"))
                    {
                        fullRootUrl = fullRootUrl.Replace(":" + HttpContext.Current?.Request.Url.Port, string.Empty);
                    }
                    result = $"{fullRootUrl}/ping?id={Id}";
                    return result;
                }
                return string.Empty;
            }
        }

        public List<PingResponseSummaryViewModel> PingResponseSummaries { get; set; }
               
    }
}