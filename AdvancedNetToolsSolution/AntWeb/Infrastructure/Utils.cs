using AntDal;
using AntDal.Entities;
using AutoMapper;
using GlobalIPCSSampleCode;
using MelissaData;
using Newtonsoft.Json;
using SmartAdminMvc.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Helpers;
using System.Web.Hosting;
using System.Xml;


namespace SmartAdminMvc.Infrastructure
{


    public static class Utils
    {

        static Utils()
        {
            RandomNumberGenerator = new Random();

            WellKnownPorts = new List<WellKnownPortViewModel>();
            string pathPorts = HostingEnvironment.MapPath(virtualPath: "~/Misc/service-names-port-numbers.csv");
            WellKnownPorts = ParsePorts(pathPorts).Where(r => r != null).ToList();

            SitesFromXml = new List<string>();

            if (System.Web.HttpContext.Current != null)
            {
                //   string xmlPath = HostingEnvironment.MapPath(virtualPath: "~/sitemap.xml"); //"https://toolsfornet.com/sitemap.xml";
            }
            else
            {
                //   string xmlPath = HostingEnvironment.MapPath(virtualPath: "~/sitemap.xml"); //"https://toolsfornet.com/sitemap.xml";
            }

            string xmlPath = HostingEnvironment.MapPath(virtualPath: "~/sitemap.xml");



            SitesFromXml = ParseXmlFromUrl(xmlPath).Where(r => r != null).ToList();

            TopSitesGlobal = new List<string>();
            string path = HostingEnvironment.MapPath(virtualPath: "~/Misc/TopSites.txt");

            TopSitesGlobal = ParseTopSites(path).Where(r => r != null).ToList();

            GetDeployedServicesUrlAddresses = new List<string>()
            {
                "http://ants-ea.cloudapp.net", // "40.83.125.9",
                "http://ants-je.cloudapp.net", // "13.71.155.140"
                "http://ants-sea2.cloudapp.net", // "13.76.100.42"
                "http://ants-cus2-83p25urt.cloudapp.net", // "52.165.26.10"
                "http://ant-jw.cloudapp.net", // "40.74.137.95"
                "http://ants-eus2-5223is65.cloudapp.net", // "13.90.212.72"
                "http://ants-neu.cloudapp.net", // "13.74.188.161"
                "http://ants-scus2.cloudapp.net", // "104.214.70.79"
                "http://ants-weu.cloudapp.net", // "104.46.38.89"
                "http://ants-wus2.cloudapp.net" // "13.93.211.38"
            };

            // Това е защото гасим и палим виртуалните машини през нощта за да спестим някой лев
            HotstNameToIp = new Dictionary<string, string>();
            foreach (string url in GetDeployedServicesUrlAddresses)
            {
                string ip = GetIpAddressFromHostName(url.Replace(oldValue: "http://", newValue: string.Empty), locationOfDeployedService: "http://ants-neu.cloudapp.net");
                HotstNameToIp.Add(url, ip);
            }

            HotstNameToAzureLocation = new Dictionary<string, string>()
            {

            { "http://ants-ea.cloudapp.net", "East Asia" },
            { "http://ants-sea2.cloudapp.net", "South East Asia" },
            { "http://ants-je.cloudapp.net", "East Japan" },
            { "http://ant-jw.cloudapp.net", "West Japan" },
            { "http://ants-cus2-83p25urt.cloudapp.net", "Central US" },
            { "http://ants-eus2-5223is65.cloudapp.net", "East US" },
            { "http://ants-neu.cloudapp.net", "North EU" },
            { "http://ants-scus2.cloudapp.net", "South Central US" },
            { "http://ants-weu.cloudapp.net", "West EU" },
            { "http://ants-wus2.cloudapp.net" , "West US"}

            };
        }

        public static string Tag(string rootRelativePath)
        {
            string applicationVirtualPath = System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath;
            if (applicationVirtualPath != "/")
            {
                rootRelativePath = applicationVirtualPath + rootRelativePath;
            }

            // *** Йордан: това го правя за да може в debug режим да се използват пълните версии на бъндълите, а в release да са минифицираните версии
#if DEBUG
            // Закоментирам защото има проблеми с минифицирането на файловете
            //if (rootRelativePath.Contains(".min."))
            //{
            //    rootRelativePath = rootRelativePath.Replace(".min.", ".");
            //}
#endif

            if (HttpRuntime.Cache[rootRelativePath] == null)
            {
                string absolute = HostingEnvironment.MapPath("~" + rootRelativePath);

                DateTime date = File.GetLastWriteTime(absolute);
                int index = rootRelativePath.LastIndexOf('/');
                string result = rootRelativePath.Insert(index, "/v-" + date.Ticks);
                if (applicationVirtualPath != "/")
                {
                    string stringToReplace = @"\" + applicationVirtualPath.Replace("/", string.Empty);

                    absolute = ReplaceLastOccurrence(absolute, stringToReplace, string.Empty); // absolute.Replace(stringToReplace, string.Empty);
                }
                HttpRuntime.Cache.Insert(rootRelativePath, result, new CacheDependency(absolute));
            }

            var resultToReturn = HttpRuntime.Cache[rootRelativePath] as string;

            return resultToReturn;
        }

        private static string ReplaceLastOccurrence(string source, string find, string replace)
        {
            int place = source.LastIndexOf(find);
            string result = source.Remove(place, find.Length).Insert(place, replace);
            return result;
        }

        public static List<string> TopSitesGlobal { get; set; }

        public static List<string> GetDeployedServicesUrlAddresses { get; set; }

        public static Dictionary<string, string> HotstNameToIp { get; set; }

        public static Dictionary<string, string> HotstNameToAzureLocation { get; set; }

        public static Random RandomNumberGenerator;

        public static List<string> SitesFromXml { get; set; }

        public static List<WellKnownPortViewModel> WellKnownPorts { get; set; }

        public static string ParseSingleTopSite(string line)
        {
            return line.Split(new char[] { ',' })[1];
        }

        public static List<string> ParseTopSites(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                using (StreamReader sr = new System.IO.StreamReader(path))
                {
                    while (!sr.EndOfStream)
                    {
                        string splitMe = sr.ReadLine();
                        TopSitesGlobal.Add(ParseSingleTopSite(splitMe));
                    }
                }
            }
            return TopSitesGlobal;
        }

        public static List<string> ParseXmlFromUrl(string url)
        {
            // Тук гърми и трябва да се оправи, защото пир стартиране на сайта нямаме достъп до
            // string xmlPath = "https://toolsfornet.com/sitemap.xml";




            using (XmlReader sr = XmlReader.Create(url))
            {
                var addressLines = new List<string>();
                while (sr.Read())
                {
                    if ((sr.NodeType == XmlNodeType.Element) && (sr.Name == "loc"))
                    {
                        sr.MoveToContent();
                        SitesFromXml.Add(sr.ReadElementContentAsString().ToString());
                    }
                }
            }
            return SitesFromXml;
        }
        public static List<WellKnownPortViewModel> ParsePorts(string pathPorts)
        {
            if (!string.IsNullOrEmpty(pathPorts))
            {
                string[] lines = File.ReadAllLines(pathPorts);
                foreach (var line in lines)
                {
                    WellKnownPorts.Add(ParseSinglePort(line));
                }
            }
            return WellKnownPorts;
        }

        public static WellKnownPortViewModel ParseSinglePort(string line)
        {
            var wkpvm = new WellKnownPortViewModel();
            return wkpvm;
            // Тук постоянно гърми, оправи си логиката. rowsSplits има само 4 елемента
            //try
            //{
            //    uint helper;
            //    DateTime help;
            //    string[] rowsSplits = line.Split(new char[] { ',' });
            //    wkpvm.ServiceName = rowsSplits[0];
            //    uint.TryParse(rowsSplits[1], out helper);
            //    wkpvm.PortNumber = helper;
            //    wkpvm.TransportProtocol = rowsSplits[2];
            //    wkpvm.Description = rowsSplits[3];
            //    wkpvm.Assignee = rowsSplits[4];
            //    wkpvm.Contact = rowsSplits[5];
            //    DateTime.TryParse(rowsSplits[6], out help);
            //    wkpvm.RegistrationDate = help;
            //    DateTime.TryParse(rowsSplits[7], out help);
            //    wkpvm.ModificationDate = help;
            //    wkpvm.Reference = rowsSplits[8];
            //    uint.TryParse(rowsSplits[9], out helper);
            //    wkpvm.ServiceCode = helper;
            //    wkpvm.KnownUnauthorizedUses = rowsSplits[10];
            //    wkpvm.AssignmentNotes = rowsSplits[11];
            //}
            //catch (Exception ex)
            //{

            //}
            //return wkpvm;
        }

        class IpInfo
        {
            public string IP { get; set; }

            public string Hostname { get; set; }

            public string City { get; set; }

            public string Region { get; set; }

            public string Country { get; set; }

            public string Loc { get; set; }

            public double Latitude
            {
                get
                {
                    string[] latString = Loc.Split(',');
                    if (latString.Length > 0)
                    {
                        string lat = latString[0];
                        double latDouble = double.Parse(lat, CultureInfo.InvariantCulture);
                        return latDouble;
                    }
                    return 0;
                }
            }

            public double Longitude
            {
                get
                {
                    string[] longString = Loc.Split(',');
                    if (longString.Length > 1)
                    {
                        string longit = longString[1];
                        double longDouble = double.Parse(longit, CultureInfo.InvariantCulture);
                        return longDouble;
                    }
                    return 0;
                }
            }

            public string Org { get; set; }

            public string AS
            {
                get
                {
                    if (!string.IsNullOrEmpty(Org))
                    {
                        string[] longString = Org.Split(' ');
                        if (longString.Length > 0)
                        {
                            string autonomous = longString[0];
                            return autonomous;

                        }
                    }
                    return string.Empty;
                }
            }

            public string Organisation
            {
                get
                {
                    if (!string.IsNullOrEmpty(Org))
                    {
                        int index = Org.IndexOf(' ');
                        if (index >= 0)
                        {
                            string organisation = Org.Substring(index + 1);
                            return organisation;

                        }
                    }
                    return string.Empty;
                }
            }
            public string Postal { get; set; }

        }

        public static MelissaIpLocationViewModel GetLocationDataForIp(string ipOrHostName)
        {
            using (AntDbContext context = new AntDbContext())
            {
                var mipl = context.MelissaIpLocations.FirstOrDefault(m => m.IpAddress == ipOrHostName);
                MelissaIpLocationViewModel res = Mapper.Map<MelissaIpLocationViewModel>(mipl);
                if (mipl != null)
                {
                    res = Mapper.Map<MelissaIpLocationViewModel>(mipl);
                }
                else
                {

                    using (HttpClient client = new HttpClient())
                    {
                        var json = client.GetStringAsync($"http://ipinfo.io/{ipOrHostName}/json").Result;
                        
                        IpInfo ipInfo = JsonConvert.DeserializeObject<IpInfo>(json);
                        mipl = new MelissaIpLocation();
                        mipl.AS = ipInfo.AS;
                        mipl.IpAddress = ipOrHostName;
                        mipl.City = ipInfo.City;
                        mipl.Country = ipInfo.Country;
                        mipl.CountryAbbreviation = ipInfo.Country;
                        mipl.Domain = ipInfo.Hostname;
                        mipl.ISP = ipInfo.Organisation;
                        mipl.Latitude = ipInfo.Latitude;
                        mipl.Longitude = ipInfo.Longitude;
                        mipl.Region = ipInfo.Region;
                        mipl.ZipCode = ipInfo.Postal;

                        context.MelissaIpLocations.Add(mipl);
                        context.SaveChanges();
                        res = Mapper.Map<MelissaIpLocationViewModel>(mipl);

                    }
                }
                                 
                return res;
            }

        }

        public static string RandomString(int length)
        {
#pragma warning disable JustCode_NamingConventions // Naming conventions inconsistency
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToLowerInvariant();
#pragma warning restore JustCode_NamingConventions // Naming conventions inconsistency
            var result = new string(Enumerable.Repeat(chars, length).Select(s => s[RandomNumberGenerator.Next(s.Length)]).ToArray());
            return result;
        }

        public static string GetGoogleMapsString(IEnumerable<string> ips, List<PingResponseSummary> pingSummaries = null, bool starLine = true, int? zoom = null)
        {
            ips = ips.Where(m => !string.IsNullOrEmpty(m));

            var locations = new List<MelissaIpLocationViewModel>();
            var locationNamesInMap = new List<string>();
            var markerNamesInMap = new List<string>();
            var infoWindowsNamesInMap = new List<string>();


            List<string> sourceIps = new List<string>();

            for (int i = 0; i < ips.Count(); i++)
            {
                if (i % 2 == 0)
                {
                    sourceIps.Add(ips.ElementAt(i));
                }
            }

            for (int i = 0; i < ips.Count(); i++)
            {
                // i % 2 == 0 Това в случай, че destionation i source адреса съвпада. Например и двата са в дейта центъра на MS
                MelissaIpLocationViewModel currLocation = GetLocationDataForIp(ips.ElementAt(i));
                //if (i % 2 == 0 && locations.Any(c => c.Latitude == currLocation.Latitude && c.Longitude == currLocation.Longitude))
                //{
                //    for (int j = 0; j < 100; j++)
                //    {
                //        double newLongitude = currLocation.Longitude.GetValueOrDefault() + 0.01;
                //        currLocation.Longitude = newLongitude;
                //        if (!locations.Any(c => c.Latitude == currLocation.Latitude && c.Longitude == currLocation.Longitude))
                //        {
                //            locations.Add(currLocation);
                //            break;
                //        }

                //    }
                //}
                //else
                //{
                locations.Add(currLocation);
                //}

                locationNamesInMap.Add("latLng" + i);
                markerNamesInMap.Add("marker" + i);
                infoWindowsNamesInMap.Add("infoWindow" + i);
            }

            List<MelissaIpLocationViewModel> sourceLocations = new List<MelissaIpLocationViewModel>();

            for (int i = 0; i < locations.Count(); i++)
            {
                if (i % 2 == 0)
                {
                    sourceLocations.Add(locations.ElementAt(i));
                }
            }

            for (int i = 0; i < locations.Count; i++)
            {
                if (i % 2 == 1)
                {
                    var currLocation = locations.ElementAt(i);
                    if (sourceLocations.Any(c => c.Latitude == currLocation.Latitude && c.Longitude == currLocation.Longitude))
                    {
                        double newLongitude = currLocation.Longitude + 0.05;
                        currLocation.Longitude = newLongitude;
                    }
                }
            }


            var sb = new StringBuilder();


            for (int i = 0; i < ips.Count(); i++)
            {
                MelissaIpLocationViewModel location = locations[i];
            }

            for (int i = 0; i < ips.Count(); i++)
            {
                sb.AppendLine($@"var {locationNamesInMap[i]} = {{ lat: {locations[i].Latitude.ToString(CultureInfo.InvariantCulture)}, lng: {locations[i].Longitude.ToString(CultureInfo.InvariantCulture)} }};");
            }
            string zoomAsString = "2";
            if (zoom.HasValue)
            {
                zoomAsString = zoom.GetValueOrDefault().ToString();
            }
            sb.AppendLine($@"var map = new google.maps.Map(document.getElementById('map'), {{
                zoom: {zoomAsString},
                center: {locationNamesInMap.LastOrDefault()}
            }});");

            string fullRootUrl = HttpContext.Current?.Request.Url.OriginalString.Replace(HttpContext.Current?.Request.Url.LocalPath, string.Empty);
            if (!HttpContext.Current.Request.Url.ToString().Contains(value: "localhost"))
            {
                fullRootUrl = fullRootUrl.Replace(":" + HttpContext.Current?.Request.Url.Port, string.Empty);
            }
            string hostAddress = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.LocalPath, string.Empty);
            string greenColorPin = $@"
            var pinGreenImage =new google.maps.MarkerImage('{hostAddress}/content/img/pingreencolor.png',
                new google.maps.Size(21, 34),
                new google.maps.Point(0, 0),
                new google.maps.Point(10, 34));";
            sb.AppendLine(greenColorPin);


            //string redColorPing = @"
            //var pinRedColor = 'FF0000';
            //var pinRedImage = new google.maps.MarkerImage('http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld=%E2%80%A2|' + pinRedColor,
            //    new google.maps.Size(21, 34),
            //    new google.maps.Point(0, 0),
            //    new google.maps.Point(10, 34));";
            //sb.AppendLine(redColorPing);



            string redColorPing = $@"        
            var pinRedImage = new google.maps.MarkerImage('{hostAddress}/content/img/pinredcolor.png',
                new google.maps.Size(21, 34),
                new google.maps.Point(0, 0),
                new google.maps.Point(10, 34));";
            sb.AppendLine(redColorPing);

            for (int i = 0; i < ips.Count(); i++)
            {


                double distanceKm = 0;
                double distanceMiles = 0;
                if (i % 2 == 0 && pingSummaries != null)
                {
                    distanceKm = GeoCodeCalc.CalcDistance(locations[i].Latitude, locations[i].Longitude,
                        locations[i + 1].Latitude, locations[i + 1].Longitude);
                    distanceKm = Math.Round(distanceKm, digits: 0);
                    distanceMiles = GeoCodeCalc.CalcDistance(locations[i].Latitude, locations[i].Longitude,
                       locations[i + 1].Latitude, locations[i + 1].Longitude, GeoCodeCalcMeasurement.Miles);
                    distanceMiles = Math.Round(distanceMiles, digits: 0);


                    sb.AppendLine($@"var {markerNamesInMap[i]} = new google.maps.Marker({{
                position: {locationNamesInMap[i]},
                map: map,
                icon: pinRedImage,
                title: 'Left click to show info window.'
                }});");
                }
                else
                {
                    sb.AppendLine($@"var {markerNamesInMap[i]} = new google.maps.Marker({{
                position: {locationNamesInMap[i]},
                map: map,
                icon: pinGreenImage,
                title: 'Left click to show info window.'
                }});");

                }

                string timeAvg = string.Empty;
                string distance = string.Empty;
                string speed = string.Empty;
                if (i % 2 == 0 && pingSummaries != null)
                {
                    var time = Math.Round(pingSummaries[i / 2].AvgRtt.GetValueOrDefault(), digits: 0);
                    if (time != 0)
                    {
                        timeAvg = @"<font size=""2"" color=""#057CBE"">TIME AVG:&nbsp;</font>" + time.ToString() + "ms<br />";
                        distance = @"<font size=""2"" color=""#057CBE"">DISTANCE:&nbsp;</font>" + distanceKm.ToString() + "km | " + distanceMiles + "mi<br />";
                        speed = @"<font size=""2"" color=""#057CBE"">SPEED:&nbsp;</font>" + ((int)(distanceKm / time)).ToString() + "km/ms | " + ((int)(distanceMiles / time)).ToString() + "mi/ms<br />";
                    }
                }

                var sbMarkerWindowHtml = new StringBuilder();
                sbMarkerWindowHtml.Append($@"\
<font size=""2"" color=""#057CBE"">IP:&nbsp;</font> {ips.ElementAt(i)} <br />{timeAvg}{distance}{speed}\
<font size=""2"" color=""#057CBE"">City:&nbsp;</font> {locations[i].City?.Replace(oldValue: "'", newValue: "&quot;")} <br />\
<font size=""2"" color=""#057CBE"">Region:&nbsp;</font> {locations[i].Region?.Replace(oldValue: "'", newValue: "&quot;")} <br />\
<font size=""2"" color=""#057CBE"">CountryAbbreviation:&nbsp;</font> {locations[i].CountryAbbreviation?.Replace(oldValue: "'", newValue: "&quot;")} <br />\
<font size=""2"" color=""#057CBE"">ISP:&nbsp;</font> {locations[i].ISP?.Replace(oldValue: "'", newValue: "&quot;")} <br />\
<font size=""2"" color=""#057CBE"">Latitude:&nbsp;</font> {locations[i].Latitude} <br />\
<font size=""2"" color=""#057CBE"">Longitude:&nbsp;</font> {locations[i].Longitude} <br />\
<font size=""2"" color=""#057CBE"">AS:&nbsp;</font> {locations[i].AS} <br />\
<font size=""2"" color=""#057CBE"">Domain:&nbsp;</font> {locations[i].Domain?.Replace(oldValue: "'", newValue: "&quot;")} <br />\
");

                string markerWindowHtml = sbMarkerWindowHtml.ToString(); ;

                //string infoWindowContentHtml; -> never used

                string infowindow = $@" var infowindow = new google.maps.InfoWindow({{
    content: '{markerWindowHtml}',
    maxWidth: 200
  }});";
                sb.Append(infowindow);

                string markerWithInfoWindow = $@" {markerNamesInMap[i]}.addListener('click', function() {{
 infowindow.setContent('{markerWindowHtml}');
   infowindow.open(map, this);
  }});";

                sb.Append(markerWithInfoWindow);
            }

            var locationStringsForPolyline = new StringBuilder();
            for (int i = 0; i < locationNamesInMap.Count; i++)
            {
                locationStringsForPolyline.Append(locationNamesInMap[i]);
                if (i != locationNamesInMap.Count - 1)
                {
                    locationStringsForPolyline.AppendLine(value: ",");
                }
            }

            // Define a symbol using a predefined path (an arrow)
            // supplied by the Google Maps JavaScript API.
            sb.Append(value: @" var lineSymbol = {
    path: google.maps.SymbolPath.FORWARD_CLOSED_ARROW
  };");


            if (starLine)
            {
                // count should be even
                for (int i = 0; i < locationNamesInMap.Count / 2; i++)
                {
                    string polyline = $@" var line = new google.maps.Polyline({{
                path: [
                    {locationNamesInMap[2 * i]} , {locationNamesInMap[2 * i + 1]}
                ],
                strokeColor: '#FF0000',
                strokeOpacity: 1.0,
                strokeWeight: 1,
 icons: [
      {{
        icon: lineSymbol,
        offset: '10%'
      }}, {{
        icon: lineSymbol,
        offset: '30%'
      }}, {{
        icon: lineSymbol,
        offset: '50%'
      }}, {{
        icon: lineSymbol,
        offset: '70%'
      }}, {{
        icon: lineSymbol,
        offset: '90%'
      }}

    ],
                map: map
            }});";
                    sb.AppendLine(polyline);
                }


            }
            else
            {

                string polyline = $@" var line = new google.maps.Polyline({{
                path: [
                    {locationStringsForPolyline.ToString()}
                ],
                strokeColor: '#FF0000',
                strokeOpacity: 1.0,
                strokeWeight: 3,
                map: map
            }});";

                sb.AppendLine(polyline);
            }

            sb.AppendLine(value: " $('#map').width('100%');");
            string finalResult = sb.ToString();

            return finalResult;
        }






        public static string GetIpAddressFromHostName(string hostName, string locationOfDeployedService)
        {
            using (HttpClient client = new HttpClient())
            {

                string encodedArgs0 = Uri.EscapeDataString($"{hostName} -n -sn -Pn");
                string url = $"{locationOfDeployedService}/home/exec?program=nmap&args=" + encodedArgs0;
                string res = client.GetStringAsync(url).Result;

                string[] lines = res.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                string ipLine = lines.FirstOrDefault(l => l.StartsWith(value: "Nmap scan report"));
                if (ipLine != null)
                {
                    int lastIndexOfOpen = ipLine.LastIndexOf(value: "(");
                    int lastIndexOfClose = ipLine.LastIndexOf(value: ")");
                    int ipLength = lastIndexOfClose - lastIndexOfOpen;
                    string ip = ipLine.Substring(lastIndexOfOpen + 1, ipLength - 1);
                    return ip;
                }
            }
            return string.Empty;
        }

        public static bool IsValidIpOrUrl(string ipOrUrlToTest)
        {
            IPAddress dummy;
            bool isIpAddress = IPAddress.TryParse(ipOrUrlToTest, out dummy);
            if (isIpAddress)
            {
                return true;
            }
            Uri uriDummy;
            bool isValidUri = Uri.TryCreate(ipOrUrlToTest, UriKind.Absolute, out uriDummy);
            if (isValidUri)
            {
                return true;
            }
            isValidUri = Uri.TryCreate(ipOrUrlToTest, UriKind.Relative, out uriDummy);
            if (isValidUri)
            {
                return true;
            }
            return false;
        }


        public static string GetFirstOpenPort(string ipOrHostName)
        {

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.Timeout = TimeSpan.FromMinutes(1);
                    //Note: Host seems down. If it is really up, but blocking our ping probes, try -Pn
                    string encodedArgs0 = Uri.EscapeDataString($"-T4 --top-ports 100 -Pn {ipOrHostName}");
                    var hostServer = HotstNameToAzureLocation.First().Key;
                    string url = $"{hostServer}/home/exec?program=nmap&args=" + encodedArgs0;

                    //string url = "http://ants-neu.cloudapp.net/home/exec?program=nmap&args=" + encodedArgs0;
                    string portSummary = client.GetStringAsync(url).Result;

                    List<PortResponseSummaryViewModel> portViewModels = PortParser.ParseSummary(portSummary);
                    PortResponseSummaryViewModel firstOpen = portViewModels.FirstOrDefault(p => p.State.Equals(value: "open"));
                    string result = string.Empty;
                    if (firstOpen != null)
                    {
                        result = firstOpen.PortNumber.ToString();
                    }
                    return result;
                }
                catch (Exception)
                {
                    return string.Empty;
                }

            }

        }

        public static string CheckIfHostIsUp(string ipOrHostName)
        {
            string errorMessage = null;
            // Проверяваме дали изобщо може да достъпим адреса;
            using (HttpClient client = new HttpClient())
            {
                string encodedArgs0 = Uri.EscapeDataString($"{ipOrHostName} -sn -Pn -n");
                string url = $"http://ants-neu.cloudapp.net/home/exec?program=nmap&args=" + encodedArgs0;
                string res = client.GetStringAsync(url).Result;
                if (!res.Contains(value: "Host is up."))
                {
                    errorMessage = $"'{ipOrHostName}' is invalid address or host is down!";
                }
            }

            return errorMessage;
        }

        public static bool IsIPLocal(IPAddress ipaddress)
        {
            String[] straryIPAddress = ipaddress.ToString().Split(new String[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            int[] iaryIPAddress = new int[] { int.Parse(straryIPAddress[0]), int.Parse(straryIPAddress[1]), int.Parse(straryIPAddress[2]), int.Parse(straryIPAddress[3]) };
            if (iaryIPAddress[0] == 10 || (iaryIPAddress[0] == 192 && iaryIPAddress[1] == 168) || (iaryIPAddress[0] == 172 && (iaryIPAddress[1] >= 16 && iaryIPAddress[1] <= 31)))
            {
                return true;
            }
            else
            {
                // IP Address is "probably" public. This doesn't catch some VPN ranges like OpenVPN and Hamachi.
                return false;
            }
        }


        //        Starting Nmap 7.01 ( https://nmap.org ) at 2016-04-24 13:55 FLE Summer Time
        //Nmap scan report for data.bg (195.149.248.130)
        //Host is up(0.0040s latency).
        //Nmap done: 1 IP address(1 host up) scanned in 0.38 seconds
        public static double? ParseLatencyString(string latencyString)
        {
            if (!latencyString.ToUpperInvariant().Contains("latency".ToUpperInvariant()))
            {
                return null;
            }

            string[] lines = latencyString.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            string latencyLine = lines.FirstOrDefault(l => l.StartsWith(value: "Host is up"));
            string latency = latencyLine
                .Replace(oldValue: "Host is up", newValue: string.Empty)
                .Replace(oldValue: "(", newValue: string.Empty)
                .Replace(oldValue: ").", newValue: string.Empty)
                .Replace(oldValue: "s", newValue: string.Empty)
                .Replace(oldValue: "latency", newValue: string.Empty)
                .Replace(oldValue: " ", newValue: string.Empty);


            double result = 0;
            double.TryParse(latency, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
            result = result * 1000; // Calculate milliseconds
            return result;
        }


#pragma warning disable JustCode_CSharp_InnerTypeFileNameMismatch // Inner types not matching file names
        public static class GeoCodeCalc
#pragma warning restore JustCode_CSharp_InnerTypeFileNameMismatch // Inner types not matching file names
        {
            public const double EarthRadiusInMiles = 3956.0;
            public const double EarthRadiusInKilometers = 6367.0;

            public static double ToRadian(double val) { return val * (Math.PI / 180); }
            public static double DiffRadian(double val1, double val2) { return ToRadian(val2) - ToRadian(val1); }

            public static double CalcDistance(double lat1, double lng1, double lat2, double lng2)
            {
                return CalcDistance(lat1, lng1, lat2, lng2, GeoCodeCalcMeasurement.Kilometers);
            }

            public static double CalcDistance(double lat1, double lng1, double lat2, double lng2, GeoCodeCalcMeasurement m)
            {
                double radius = GeoCodeCalc.EarthRadiusInMiles;

                if (m == GeoCodeCalcMeasurement.Kilometers)
                {
                    radius = GeoCodeCalc.EarthRadiusInKilometers;
                }
                double result = radius * 2 * Math.Asin(Math.Min(val1: 1, val2: Math.Sqrt((Math.Pow(Math.Sin((DiffRadian(lat1, lat2)) / 2.0), y: 2.0) + Math.Cos(ToRadian(lat1)) * Math.Cos(ToRadian(lat2)) * Math.Pow(Math.Sin((DiffRadian(lng1, lng2)) / 2.0), y: 2.0)))));
                return result;
            }

        }

        public enum GeoCodeCalcMeasurement : int
        {
            Miles = 0,
            Kilometers = 1
        }
    }
}