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
            PortResponseSummaryViewModel result = PortParser.ParseSingleLine(line);

            Assert.NotNull(result);
            Assert.Equal(22, result.PortNumber);
            Assert.Equal("tcp", result.Protocol);
            Assert.Equal("filtered", result.State);
            Assert.Equal("ssh", result.Service);

        }

        [Theory]
        [InlineData("23/tcp  filtered telnet")]
        [InlineData("81/tcp  open     hosts2-ns")]
        [InlineData("80/tcp  open     http")]
        [InlineData("111/tcp filtered rpcbind")]
        [InlineData("135/tcp filtered msrpc")]
        public void ParseSingleLineTest(string line)
        {
            PortResponseSummaryViewModel result = PortParser.ParseSingleLine(line);

            Assert.NotNull(result);
        }
        [Fact]
        public void OpenPortsTest()
        {
            using (HttpClient client = new HttpClient())
            {

                var encodedArgs0 = Uri.EscapeDataString("-T4 -F www.dir.bg");
                string url = "http://antnortheu.cloudapp.net/home/exec?program=nmap&args=" + encodedArgs0;
                var portSummary = client.GetStringAsync(url).Result;

                List<PortResponseSummaryViewModel> portViewModels = PortParser.ParseSummary(portSummary);
                
                Assert.True(portViewModels.Count > 0);
                
            }
        }
    }
}
