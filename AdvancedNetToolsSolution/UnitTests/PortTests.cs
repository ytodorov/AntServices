using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class PortTests
    {
        [Fact]
        public void OpenPortsTest()
        {
            using (HttpClient client = new HttpClient())
            {

                var encodedArgs0 = Uri.EscapeDataString("-p 1-1024 -Pn www.dir.bg");
                string url = "http://antnortheu.cloudapp.net/home/exec?program=nmap&args=" + encodedArgs0;
                var portSummary = client.GetStringAsync(url).Result;

                //List<PortViewModel> traceRouteViewModels = PortParser.ParseSummary(portSummary);
            }
        }
    }
}
