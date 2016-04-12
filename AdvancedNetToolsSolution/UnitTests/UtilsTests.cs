using SmartAdminMvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class UtilsTests
    {
        [Fact]
        public void GetIpAddressFromHostNameTest()
        {
            List<string> locations = Utils.GetDeployedServicesUrlAddresses;
            List<string> results = new List<string>();
            foreach (string location in locations)
            {
                var result = Utils.GetIpAddressFromHostName("google.com", location);
                IPAddress dummy;
                var isIpv4ParseSuccessfull = IPAddress.TryParse(result, out dummy);
                Assert.True(isIpv4ParseSuccessfull);

                results.Add(result);


            }
           
        }
        [Fact]
        private void PingUrlsSoTheyDontSleep()
        {
            List<string> urls = Utils.GetDeployedServicesUrlAddresses;
            urls.Add("http://ant-ne.azurewebsites.net");
            foreach (string url in urls)
            {
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        var tracerouteSummary = client.GetStringAsync(url).Result;
                    }
                    catch
                    {

                    }

                }
            }
        }
    }
}
