using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Homer_MVC.Models
{
    public class PortViewModel
    {
        public int Port { get; set; }

        public string Protocol { get; set; }

        public string State { get; set; }

        public string Service { get; set; }
    }
}