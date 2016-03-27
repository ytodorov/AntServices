using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class TraceRouteTests
    {
        [Fact]
        public void GetTraceRouteString()
        {
            using (HttpClient client = new HttpClient())
            {
                var encodedArgs = Uri.EscapeDataString($"--traceroute 92.247.12.80 -sn -T5");
                string url = "http://antnortheu.cloudapp.net/home/exec?program=nmap&args=" + encodedArgs;
                var res = client.GetStringAsync(url).Result;

                // some test
                int aTest = 123;
            }
        }
    }
}
