using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntDal.Entities
{
    public class BarcodePermalink : PermalinkBase
    {
        public BarcodePermalink()
        {

        }

        public string Value { get; set; }
    }
}
