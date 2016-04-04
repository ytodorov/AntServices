using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Homer_MVC.Models
{
    public class PingReplyViewModel
    {
        public string Address { get; set; }

        public int BufferLength { get; set; }

        public bool Df { get; set; }

        public int Ttl { get; set; }

        public int Rtt { get; set; }

        public int StatusCode { get; set; }

    }
}