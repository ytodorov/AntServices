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
using System.Net.Sockets;
using System.Diagnostics;
using System.Threading;

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

        public string Tcp()
        {
            TcpClient client = new TcpClient();
            client.Connect("8.8.8.8", 53);
            return client.Connected.ToString();
        }

        public string Nmap()
        {
            try
            {
                var abp = Startup.ApplicationBasePath;
                //C:\Projects\AdvancedNetToolsServicesRepo\AdvancedNetToolsSolution\src\AdvancedNetToolsServicesWeb\nmap.exe


                string path = abp + "\\psping.exe";
                string args2 = "-i 0 -n 10 8.8.8.8:53";
                Process p = new Process();
                p.StartInfo.FileName = path;
                p.StartInfo.Arguments = args2;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.UseShellExecute = false;
                p.Start();

                //Thread.Sleep(10000);

                //string result = "";
                string result = p.StandardOutput.ReadToEnd();
                string error = p.StandardError.ReadToEnd();
                return result + "ГРЕШКИ" + error;
            }
            catch (Exception ex)
            {
                string json = JsonConvert.SerializeObject(ex, Formatting.Indented);
                return json;
            }
        }
        public string Ports(string ip = "8.8.8.8")
        {
            try
            {
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer 
                // connected to the same address as specified by the server, port
                // combination.
                Int32 port = 53;
                TcpClient client = new TcpClient(ip, port);

                string message = "test";

                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();

                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);

                Console.WriteLine("Sent: {0}", message);

                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
                data = new Byte[256];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", responseData);

                // Close everything.
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            return "";


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
