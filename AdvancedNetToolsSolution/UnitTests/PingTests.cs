using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Xunit;

namespace UnitTests
{
    public class PingTests
    {
        [Fact]
        public void ParsePintSummaryTest()
        {
            using (HttpClient client = new HttpClient())
            {

                var encodedArgs0 = Uri.EscapeDataString(" 8.8.8.8 --delay 10ms -v1");
                string url = "http://antnortheu.cloudapp.net/home/exec?program=nping&args=" + encodedArgs0;
                var res = client.GetStringAsync(url).Result;

                var lines = res.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                var firstLine = lines.First(s => s.StartsWith("max", StringComparison.InvariantCultureIgnoreCase));

                if (!string.IsNullOrEmpty(firstLine))
                {
                    var timesAndEmptyStrings = firstLine.Replace("Max rtt:", string.Empty)
                        .Replace("Min rtt:", string.Empty)
                        .Replace("Avg rtt:", string.Empty)
                        .Replace("|", string.Empty)
                        .Replace("ms", string.Empty);
                        

                    var times = timesAndEmptyStrings.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                    double maxRtt = double.Parse(times[0], CultureInfo.InvariantCulture);
                    double minRtt = double.Parse(times[1], CultureInfo.InvariantCulture);
                    double avgRtt = double.Parse(times[2], CultureInfo.InvariantCulture);


                }
                

            }
        }
    }
}
