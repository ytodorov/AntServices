using SmartAdminMvc.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public  class UnitTestBase
    {
        public UnitTestBase()
        {
            MappingConfig.RegisterMaps();
        }
    }
}
