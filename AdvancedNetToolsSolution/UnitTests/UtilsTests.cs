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
            var results = new List<string>();
            foreach (string location in locations)
            {
                string result = Utils.GetIpAddressFromHostName(hostName: "google.com", locationOfDeployedService: location);
                IPAddress dummy;
                bool isIpv4ParseSuccessfull = IPAddress.TryParse(result, out dummy);
                Assert.True(isIpv4ParseSuccessfull);

                results.Add(result);


            }
           
        }
        [Fact]
        private void PingUrlsSoTheyDontSleep()
        {
            List<string> urls = Utils.GetDeployedServicesUrlAddresses;
            urls.Add(item: "http://ant-ne.azurewebsites.net");
            foreach (string url in urls)
            {
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        string tracerouteSummary = client.GetStringAsync(url).Result;
                    }
                    catch
                    {

                    }

                }
            }
        }
    }
}
