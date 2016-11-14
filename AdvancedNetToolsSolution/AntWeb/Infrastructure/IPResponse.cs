using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace GlobalIPCSSampleCode
{
    [DataContract]
    class IPResponse
    {
        [DataMember(Name="Version", IsRequired=true)]
        public string Version { get; set; }

        [DataMember(Name = "TransmissionReference", IsRequired = true)]
        public string TransmissionReference { get; set; }

        [DataMember(Name = "TransmissionResults", IsRequired = true)]
        public string TransmissionResults { get; set; }

        [DataMember(Name = "Records", IsRequired = false)]
        public IPRecord[] Records { get; set; }
    }
}
