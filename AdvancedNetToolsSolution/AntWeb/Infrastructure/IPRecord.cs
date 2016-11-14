using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace GlobalIPCSSampleCode
{
    [DataContract]
    class IPRecord
    {
        [DataMember(Name = "RecordID", IsRequired = true)]
        public string RecordID { get; set; }

        [DataMember(Name = "IPAddress", IsRequired = true)]
        public string IPAddress { get; set; }

        [DataMember(Name = "Latitude", IsRequired = true)]
        public string Latitude { get; set; }

        [DataMember(Name = "Longitude", IsRequired = true)]
        public string Longitude { get; set; }

        [DataMember(Name = "PostalCode", IsRequired = true)]
        public string PostalCode { get; set; }

        [DataMember(Name = "Region", IsRequired = true)]
        public string Region { get; set; }

        [DataMember(Name = "ISPName", IsRequired = true)]
        public string ISPName { get; set; }

        [DataMember(Name = "DomainName", IsRequired = true)]
        public string DomainName { get; set; }

        [DataMember(Name = "City", IsRequired = true)]
        public string City { get; set; }

        [DataMember(Name = "CountryName", IsRequired = true)]
        public string CountryName { get; set; }

        [DataMember(Name = "CountryAbbreviation", IsRequired = true)]
        public string CountryAbbreviation { get; set; }

        [DataMember(Name = "ConnectionSpeed", IsRequired = true)]
        public string ConnectionSpeed { get; set; }

        [DataMember(Name = "ConnectionType", IsRequired = true)]
        public string ConnectionType { get; set; }

        [DataMember(Name = "UTC", IsRequired = true)]
        public string UTC { get; set; }

        [DataMember(Name = "Continent", IsRequired = true)]
        public string Continent { get; set; }

        [DataMember(Name = "Result", IsRequired = true)]
        public string Result { get; set; }
    }
}
