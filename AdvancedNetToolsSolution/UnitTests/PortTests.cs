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
    public class PortTests
    {
        [Fact]
        public void ParseSingleLineExample1Test()
        {
            string line = "22/tcp  filtered ssh";
            PortReplyModel result = PortParser.ParseSingleLine(line);

            Assert.NotNull(result);
            Assert.Equal(22, result.Port);
            Assert.Equal("tcp", result.Protocol);
            Assert.Equal("filtered", result.State);
            Assert.Equal("ssh", result.Service);

        }
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
