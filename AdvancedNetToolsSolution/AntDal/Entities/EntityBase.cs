using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntDal.Entities
{
    public class EntityBase
    {
        public int Id { get; set; }

        public string UserCreated { get; set; }

        public string UserModified { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime? DateModified { get; set; }
    }
}
