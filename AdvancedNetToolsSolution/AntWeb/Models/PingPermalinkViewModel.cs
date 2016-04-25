﻿using System;
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
                bool isIpAddress = IPAddress.TryParse(DestinationAddress, out dummy);
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
                    if (!HttpContext.Current.Request.Url.ToString().Contains(value: "localhost"))
                    {
                        fullRootUrl = fullRootUrl.Replace(":" + HttpContext.Current?.Request.Url.Port, string.Empty);
                    }
                    string urlOrIpString = "url";
                    if (IsDestinationIpAddress)
                    {
                        urlOrIpString = "ip";
                    }

                    //Uri helper;
                    //if(Uri.TryCreate(fullRootUrl, UriKind.Absolute, out helper))
                    //{
                    //    fullRootUrl = helper.Host;
                    //}

                    result = $"{fullRootUrl}/ping?{urlOrIpString}={DestinationAddress}&id={Id}";
                    return result;
                }
                return string.Empty;
            }
        }

        public List<PingResponseSummaryViewModel> PingResponseSummaries { get; set; }
               
    }
}