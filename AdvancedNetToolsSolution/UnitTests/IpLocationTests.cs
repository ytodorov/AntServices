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
            using (HttpClient client = new HttpClient())
            {

                string apiKey = "45c5fd0e7b3a7f323010de71fbc4aae7943b9f139ef3578af1b38878d0d02e81";
                string ipAddress = "92.247.12.80";
                string url = $"http://api.ipinfodb.com/v3/ip-city/?key={apiKey}&ip={ipAddress}&format=json";
                string result = client.GetStringAsync(url).Result;
            }
        }
    }
}
