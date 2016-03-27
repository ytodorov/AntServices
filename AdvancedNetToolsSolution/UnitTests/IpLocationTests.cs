using SmartAdminMvc.Infrastructure;
using SmartAdminMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class IpLocationTests
    {
        [Fact]
        public void GetLocationTest()
        {
            IpLocationViewModel ipLocationViewModel = Utils.GetLocationDataForIp("www.abv.bg");

            Assert.NotNull(ipLocationViewModel);
        }
    }
}
