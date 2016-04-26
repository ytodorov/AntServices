using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntDal.Entities
{
    public class ErrorLogging : EntityBase
    {
        public string Message { get; set; }

        public string StackTrace { get; set; }

        public string Data { get; set; }

        public bool? ShowInHistory { get; set; }
        public string UserCreatedIpAddress { get; set; }
        public string DestinationAddress { get; set; }
    }
}
