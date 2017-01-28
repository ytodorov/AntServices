using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AntServicesMvc5.Models
{
    public class DownloadYoutubeMessage
    {
        [JsonProperty("guid")]
        public string Guid { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("percentage")]
        public int Percentage { get; set; }

        [JsonProperty("etaInSeconds")]
        public int EtaInSeconds { get; set; }

    }
}