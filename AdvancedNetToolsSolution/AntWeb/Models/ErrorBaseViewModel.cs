using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartAdminMvc.Models
{
    public class ErrorBaseViewModel : ViewModelBase
    {
        public string DestinationAddress { get; set; }

        public string UserCreatedIpAddress { get; set; }

        public bool? ShowInHistory { get; set; }
    }
}