using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntDal.Entities
{
    public partial class MelissaIpLocation : EntityBase
    {
        public string IpAddress { get; set; }

        public int MyProperty { get; set; }

        public short Confidence { get; set; }
        
        public string Results { get; set; }

        public string Domain { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string ZipCode { get; set; }

        public string Region { get; set; }

        public string ISP { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string CountryAbbreviation { get; set; }
    }
}
