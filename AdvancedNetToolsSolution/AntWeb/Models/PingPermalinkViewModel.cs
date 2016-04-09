using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SmartAdminMvc.Models
{
    public class PingPermalinkViewModel : ViewModelBase
    {
        public string DestinationAddress { get; set; }

        public string UserCreatedIpAddress { get; set; }

        public bool? ShowInHistory { get; set; }

        public string PermalinkAddress
        {
            get
            {
                string result = string.Empty;

                if (HttpContext.Current?.Request != null)
                {
                    string fullRootUrl = HttpContext.Current?.Request.Url.OriginalString.Replace(HttpContext.Current?.Request.Url.LocalPath, string.Empty);

                    result = $"{fullRootUrl}/ping?id={Id}";
                    return result;
                }
                return string.Empty;
            }
        }

        public List<PingResponseSummaryViewModel> PingResponseSummaries { get; set; }

        //public string PingResponsesIpAddress
        //{
        //    get
        //    {
        //        StringBuilder result = new StringBuilder();
        //        for (int i = 0; i < PingResponseSummaries.Count; i++)
        //        {
        //            result.Append(PingResponseSummaries[i].SourceIpAddress);
        //            if (i != PingResponseSummaries.Count - 1)
        //            {
        //                result.Append(",");
        //            }
        //        }
        //        string stringResult = result.ToString();
        //        return stringResult;
        //    }
        //    set
        //    {

        //    }

        //}
    }
}