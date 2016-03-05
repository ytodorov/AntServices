using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using System.Net.NetworkInformation;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Reflection;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Security.Cryptography;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AdvancedNetToolsServicesWeb.Controllers
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public string Index()
        {
            return "This is index";
        }

        public string SendPing(string ip, int timeout = 1000, int bufferSize = 32, int ttl = 128, bool dontFragment = false)
        {
            try
            {
                PingOptions pingOptions = new PingOptions()
                {
                    Ttl = ttl,
                    DontFragment = dontFragment
                };
                byte[] buffer = new byte[bufferSize];
                RNGCryptoServiceProvider rand = new RNGCryptoServiceProvider();
                rand.GetBytes(buffer);

                Ping ping = new Ping();
                PingReply pr = ping.Send(ip, timeout, buffer, pingOptions);

                MyPingReply mpr = new MyPingReply()
                {
                    Address = pr.Address.ToString(),
                    Buffer = pr.Buffer,
                    Options = pr.Options,
                    RoundtripTime = pr.RoundtripTime,
                    Status = pr.Status
                };

                string json = JsonConvert.SerializeObject(mpr, Formatting.Indented);

                return json;
            }
            catch (Exception ex)
            {
                string json = JsonConvert.SerializeObject(ex, Formatting.Indented);
                return json;
            }
        }
    }

    public class MyPingReply
    {
        //
        // Summary:
        //     Gets the address of the host that sends the Internet Control Message Protocol
        //     (ICMP) echo reply.
        //
        // Returns:
        //     An System.Net.IPAddress containing the destination for the ICMP echo message.
        public string Address { get; set; }
        //
        // Summary:
        //     Gets the buffer of data received in an Internet Control Message Protocol (ICMP)
        //     echo reply message.
        //
        // Returns:
        //     A System.Byte array containing the data received in an ICMP echo reply message,
        //     or an empty array, if no reply was received.
        public byte[] Buffer { get; set; }
        //
        // Summary:
        //     Gets the options used to transmit the reply to an Internet Control Message Protocol
        //     (ICMP) echo request.
        //
        // Returns:
        //     A System.Net.NetworkInformation.PingOptions object that contains the Time to
        //     Live (TTL) and the fragmentation directive used for transmitting the reply if
        //     System.Net.NetworkInformation.PingReply.Status is System.Net.NetworkInformation.IPStatus.Success;
        //     otherwise, null.
        public PingOptions Options { get; set; }
        //
        // Summary:
        //     Gets the number of milliseconds taken to send an Internet Control Message Protocol
        //     (ICMP) echo request and receive the corresponding ICMP echo reply message.
        //
        // Returns:
        //     An System.Int64 that specifies the round trip time, in milliseconds.
        public long RoundtripTime { get; set; }
        //
        // Summary:
        //     Gets the status of an attempt to send an Internet Control Message Protocol (ICMP)
        //     echo request and receive the corresponding ICMP echo reply message.
        //
        // Returns:
        //     An System.Net.NetworkInformation.IPStatus value indicating the result of the
        //     request.
        public IPStatus Status { get; set; }
    }
}
