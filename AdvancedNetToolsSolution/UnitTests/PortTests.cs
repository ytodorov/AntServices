using SmartAdminMvc.Infrastructure;
using SmartAdminMvc.Infrastructure.Jobs;
using SmartAdminMvc.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class PortTests : UnitTestBase
    {

        [Fact]
        public void ParseSingleLineExample1Test()
        {
            string line = "22/tcp  filtered ssh";
            PortResponseSummaryViewModel result = PortParser.ParseSingleLine(line);

            Assert.NotNull(result);
            Assert.Equal(expected: 22, actual: result.PortNumber);
            Assert.Equal(expected: "tcp", actual: result.Protocol);
            Assert.Equal(expected: "filtered", actual: result.State);
            Assert.Equal(expected: "ssh", actual: result.Service);

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

                string encodedArgs0 = Uri.EscapeDataString(stringToEscape: "-T4 -F www.dir.bg");
                string url = "http://antnortheu.cloudapp.net/home/exec?program=nmap&args=" + encodedArgs0;
                string portSummary = client.GetStringAsync(url).Result;

                List<PortResponseSummaryViewModel> portViewModels = PortParser.ParseSummary(portSummary);

                Assert.True(portViewModels.Count > 0);

            }
        }

        [Fact]
        public void ScanAllPortsTest()
        {
            //   { "http://ants-ea.cloudapp.net", "East Asia" }, is VERY SLOW http://ants-scus.cloudapp.net
            var serviceUrls = Utils.GetDeployedServicesUrlAddresses.Skip(1).ToList();//.Take(1).ToList();

            int maxPortNumber = ushort.MaxValue; // 10;
            var batchSize = maxPortNumber / serviceUrls.Count;
            List<Task<string>> tasks = new List<Task<string>>();
            List<HttpClient> clients = new List<HttpClient>();


            List<double> timmings = new List<double>();

            List<string> strRequests = new List<string>();
            for (int i = 0; i < serviceUrls.Count; i++)
            {
                int startPort = i * batchSize + 1;
                int endPort = (i + 1) * batchSize;
                if (endPort >= maxPortNumber)
                {
                    endPort = maxPortNumber;
                }
                int portCount = endPort - startPort;



                HttpClient client = new HttpClient();

                var currentServiceUrl = serviceUrls[i];
                // Да не се използва -Т5 никога!!! - giving up on port because retransmission cap hit (2)
                string encodedArgs0 = Uri.EscapeDataString($"-T4 -p {startPort}-{endPort} -Pn data.omegasoft.bg");
                strRequests.Add(encodedArgs0);
                string url = $"{currentServiceUrl}/home/exec?program=nmap&args={encodedArgs0}";

                client.Timeout = TimeSpan.FromMinutes(5);
                var task = client.GetStringAsync(url);
                tasks.Add(task);
               
                clients.Add(client);

                //Stopwatch s = Stopwatch.StartNew();
                //var result = task.Result;
                //timmings.Add(s.Elapsed.TotalSeconds);

            }



            Task.WaitAll(tasks.ToArray(), -1);
            List<PortResponseSummaryViewModel> resultPortResponseSummaryViewModel = new List<PortResponseSummaryViewModel>();
            foreach (Task<string> task in tasks)
            {
                List<PortResponseSummaryViewModel> portViewModels = PortParser.ParseSummary(task.Result);
                resultPortResponseSummaryViewModel.AddRange(portViewModels);
            }
            clients.ForEach((action) => action.Dispose());            
        }

        [Fact]
        public void PortScanAddressesJobTest()
        {
            PortScanAddressesJob pj = new PortScanAddressesJob();
            pj.Execute(null);
        }
    }
}
