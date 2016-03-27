using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartAdminMvc.Models
{
    public class TraceRouteReplyViewModel
    {
        public int Hop { get; set; }

        public double Rtt { get; set; }

        public string Ip { get; set; }

        public string AddressName { get; set; }
        /*
Starting Nmap 7.01 ( https://nmap.org ) at 2016-03-27 12:56 Coordinated Universal Time
Nmap scan report for 92.247.12.80
Host is up (0.048s latency).

TRACEROUTE (using port 443/tcp)
HOP RTT      ADDRESS
1   ... 5
6   34.00 ms be-11-0.ibr01.dub30.ntwk.msn.net (104.44.9.6)
7   34.00 ms be-9-0.ibr01.dub30.ntwk.msn.net (104.44.4.138)
8   34.00 ms ae6-0.ams-96c-1a.ntwk.msn.net (104.44.4.143)
9   34.00 ms be-1-0.ibr02.ams.ntwk.msn.net (104.44.4.214)
10  34.00 ms ae12-0.fra-96cbe-1a.ntwk.msn.net (104.44.228.0)
11  32.00 ms ae12-0.fra-96cbe-1a.ntwk.msn.net (104.44.228.0)
12  53.00 ms ae2-0.sof01-96cbe-1a.ntwk.msn.net (104.44.227.244)
13  53.00 ms 92.247.12.80

Nmap done: 1 IP address (1 host up) scanned in 4.67 seconds
ГРЕШКИ
*/

    }
}