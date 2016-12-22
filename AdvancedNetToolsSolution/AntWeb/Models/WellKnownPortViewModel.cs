using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartAdminMvc.Models
{
    public class WellKnownPortViewModel : ViewModelBase
    {
        public string ServiceName { get; set; }
        public uint PortNumber { get; set; }
        public string TransportProtocol { get; set; }
        public string Description { get; set; }
        //public string Assignee { get; set; }
        //public string Contact { get; set; }
        //public DateTime? RegistrationDate { get; set; }
        //public DateTime? ModificationDate { get; set; }
        //public string Reference { get; set; }
        //public uint ServiceCode { get; set; }
        //public string KnownUnauthorizedUses { get; set; }
        //public string AssignmentNotes { get; set; }
    }
}