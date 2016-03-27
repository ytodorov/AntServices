using SmartAdminMvc.Infrastructure;
using SmartAdminMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class TraceRouteTests
    {

        [Fact]
        public void ParseSingleLineExample1Test()
        {
            string line = "7   34.00 ms be-9-0.ibr01.dub30.ntwk.msn.net (104.44.4.138)";
            TraceRouteReplyViewModel result = TraceRouteParser.ParseSingleLine(line);

            Assert.NotNull(result);
            Assert.Equal(7, result.Hop);
            Assert.Equal(34, result.Rtt);
            Assert.Equal("104.44.4.138", result.Ip);
            Assert.Equal("be-9-0.ibr01.dub30.ntwk.msn.net", result.AddressName);

        }

        [Theory]
        [InlineData("1   ... 5")]
        [InlineData("6   34.00 ms be-11-0.ibr01.dub30.ntwk.msn.net (104.44.9.6)")]
        [InlineData("7   34.00 ms be-9-0.ibr01.dub30.ntwk.msn.net (104.44.4.138)")]
        [InlineData("9   34.00 ms be-1-0.ibr02.ams.ntwk.msn.net (104.44.4.214)")]
        [InlineData("11  32.00 ms ae12-0.fra-96cbe-1a.ntwk.msn.net (104.44.228.0)")]
        public void ParseSingleLineTest(string line)
        {
            TraceRouteReplyViewModel result = TraceRouteParser.ParseSingleLine(line);

            Assert.NotNull(result);
            Assert.True(result.Hop != 0);
        }              

        [Theory]
        [InlineData("92.247.12.80")]
        [InlineData("77.70.121.1")]
        [InlineData("8.8.8.8")]
        [InlineData("abv.bg")]
        [InlineData("www.dir.bg")]
        //[InlineData("2001:4860:4860::8888")]
        //[InlineData("2001:4860:4860::8844")]       
        public void ParseSummaryTest(string ip)
        {

            using (HttpClient client = new HttpClient())
            {
                var encodedArgs = Uri.EscapeDataString($"--traceroute {ip} -sn -T5");
                string url = "http://antnortheu.cloudapp.net/home/exec?program=nmap&args=" + encodedArgs;
                var traceStringToParse = client.GetStringAsync(url).Result;

                List<TraceRouteReplyViewModel> traceRouteViewModels = TraceRouteParser.ParseSummary(traceStringToParse);

                Assert.True(traceRouteViewModels.Count > 0);
                IPAddress helper;
                if (IPAddress.TryParse(ip, out helper))
                {
                    // Ако ip e адрес проверяваме дали последния резултат е точно този адрес. Така трябва да бъде
                    Assert.True(traceRouteViewModels.Last().Ip.Equals(ip));
                }

            }
        }
                
    }
}


/*
 IPHostEntry hostEntry;

            hostEntry = Dns.GetHostEntry(ip);
                     

            if (hostEntry.AddressList.Length > 0)
            {
                var ip2 = hostEntry.AddressList[0];
                Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                s.Connect(ip2, 80);
            }


    ip = "2001:4860:4860::8844";

            var ip6 = IPAddress.Parse(ip);

            string ipv6Option = string.Empty;
            if (ip6.AddressFamily == AddressFamily.InterNetworkV6)
            {
                ipv6Option = "-6 --unprivileged";
            }
*/
