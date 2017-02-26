using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace SmartAdminMvc.Models
{
    public class PermalinkBaseViewModel : ViewModelBase
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

        public virtual string PermalinkAddress
        {
            get
            {
                string result = string.Empty;

                if (HttpContext.Current?.Request != null)
                {
                 
                    string fullRootUrl = HttpContext.Current?.Request.Url.OriginalString.Replace
                        (HttpContext.Current?.Request.Url.PathAndQuery, string.Empty);
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

                    string controllerName = string.Empty;
                    if (HttpContext.Current.Request.Url.Segments.Length >= 1)
                    {
                        controllerName = HttpContext.Current.Request.Url.Segments[1].Replace("/", string.Empty);
                    }

                    result = $"{fullRootUrl}/{controllerName}?{urlOrIpString}={DestinationAddress}&id={Id}";                  

                    return result;
                }
                return string.Empty;
            }
        }

        public string DestinationAddress { get; set; }

        public string UserCreatedIpAddress { get; set; }

        public bool? ShowInHistory { get; set; }
    }
}