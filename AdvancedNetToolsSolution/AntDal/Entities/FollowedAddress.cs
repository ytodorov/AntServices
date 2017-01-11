using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntDal.Entities
{
    public class FollowedAddress : EntityBase
    {
        public string Url { get; set; }

        public string Ip { get; set; }
    }
}
