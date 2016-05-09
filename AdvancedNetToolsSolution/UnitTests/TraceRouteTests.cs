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
            Assert.Equal(expected: 7, actual: result.Hop);
            Assert.Equal(expected: 34, actual: result.Rtt);
            Assert.Equal(expected: "104.44.4.138", actual: result.Ip);
            Assert.Equal(expected: "be-9-0.ibr01.dub30.ntwk.msn.net", actual: result.AddressName);

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

        [Fact]
        public void ParseSummaryTest()
        {
            StringBuilder sb = new StringBuilder();



            List<string> addresses = Utils.GetDeployedServicesUrlAddresses;//.Skip(1).ToList();
            foreach (var urlService in addresses)
            {

                using (HttpClient client = new HttpClient())
                {
                    string encodedArgs = Uri.EscapeDataString($" -sS -p 80 -Pn --traceroute www.abv.bg");
                    string url = $"{urlService}/home/exec?program=nmap&args=" + encodedArgs;
                    string traceStringToParse = client.GetStringAsync(url).Result;

                    sb.AppendLine(traceStringToParse);
                    sb.AppendLine();
                    sb.AppendLine();

                    List<TraceRouteReplyViewModel> traceRouteViewModels = TraceRouteParser.ParseSummary(traceStringToParse);

                   

                }
            }

            string result = sb.ToString();
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
