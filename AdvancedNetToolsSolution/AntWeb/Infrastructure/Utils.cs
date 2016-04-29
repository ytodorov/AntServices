using AntDal;
using AntDal.Entities;
using AutoMapper;
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

namespace SmartAdminMvc.Infrastructure
{
    public static class Utils
    {

        static Utils()
        {
            RandomNumberGenerator = new Random();

            //TopSitesGlobal = Зареди тук сайтовете от файла Misc\TopSites като ги парснеш по подходящ начин. 
            // накрая в TopSitesGlobal трябва да има 1000 сайта
            TopSitesGlobal = new List<string>();

            string path = System.Web.Hosting.HostingEnvironment.MapPath(virtualPath: "~/Misc/TopSites.txt");

            if (!string.IsNullOrEmpty(path))
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(path))
                {
                    while (!sr.EndOfStream)
                    {
                        string splitMe = sr.ReadLine();
                        string[] rowsSplits = splitMe.Split(new char[] { ',' });
                        if (rowsSplits.Length == 2)
                        {
                            TopSitesGlobal.Add(rowsSplits[1]);
                        }
                    }
                }
            }

            GetDeployedServicesUrlAddresses = new List<string>()
            {
                "http://ants-ea.cloudapp.net", // "40.83.125.9",
                "http://ants-je.cloudapp.net", // "13.71.155.140"
                "http://ants-sea.cloudapp.net", // "13.76.100.42"
                "http://ant-cus.cloudapp.net", // "52.165.26.10"
                "http://ant-jw.cloudapp.net", // "40.74.137.95"
                "http://ants-eus.cloudapp.net", // "13.90.212.72"
                "http://ants-neu.cloudapp.net", // "13.74.188.161"
                "http://ants-scus.cloudapp.net", // "104.214.70.79"
                "http://ants-weu.cloudapp.net", // "104.46.38.89"
                "http://ants-wus.cloudapp.net" // "13.93.211.38"
            };

            // Това е защото гасим и палим виртуалните машини през нощта за да спестим някой лев
            HotstNameToIp = new Dictionary<string, string>();
            foreach (string url in GetDeployedServicesUrlAddresses)
            {
                string ip = Utils.GetIpAddressFromHostName(url.Replace(oldValue: "http://", newValue: string.Empty), locationOfDeployedService: "http://ants-neu.cloudapp.net");
                HotstNameToIp.Add(url, ip);
            }

        //    HotstNameToIp = new Dictionary<string, string>()
        //{
        //    {  "http://ants-ea.cloudapp.net", "40.83.125.9" },
        //    {  "http://ants-je.cloudapp.net", "13.71.155.140" },
        //    {  "http://ants-sea.cloudapp.net", "13.76.100.42" },
        //    {  "http://ant-cus.cloudapp.net", "52.165.26.10" },
        //    {  "http://ant-jw.cloudapp.net", "40.74.137.95" },
        //    {  "http://ants-eus.cloudapp.net", "13.90.212.72" },
        //    {  "http://ants-neu.cloudapp.net", "13.74.188.161" },
        //    {  "http://ants-scus.cloudapp.net", "104.214.70.79" },
        //    {  "http://ants-weu.cloudapp.net", "104.46.38.89" },
        //    {  "http://ants-wus.cloudapp.net" , "13.93.211.38"}
        //};
            HotstNameToAzureLocation = new Dictionary<string, string>()
            {
                
            { "http://ants-ea.cloudapp.net", "East Asia" },
            { "http://ants-sea.cloudapp.net", "South East Asia" },
            { "http://ants-je.cloudapp.net", "East Japan" },
            { "http://ant-jw.cloudapp.net", "West Japan" },
            { "http://ant-cus.cloudapp.net", "Central US" },
            { "http://ants-eus.cloudapp.net", "East US" },
            { "http://ants-neu.cloudapp.net", "North EU" },
            { "http://ants-scus.cloudapp.net", "South Central US" },
            { "http://ants-weu.cloudapp.net", "West EU" },
            { "http://ants-wus.cloudapp.net" , "West US"}

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

        public static IpLocationViewModel GetLocationDataForIp(string ipOrHostName)
        {
            //IPAddress ipString;
            //if(!IPAddress.TryParse(ipOrHostName, out ipString))
            //{

            //}
            using (AntDbContext context = new AntDbContext())
            {
                IpLocation ipLocation = context.IpLocations.Where(ip => ip.IpAddress == ipOrHostName).FirstOrDefault();
                if (ipLocation != null)
                {
                    IpLocationViewModel ipLocationViewModel = Mapper.Map<IpLocationViewModel>(ipLocation);
                    return ipLocationViewModel;
                }
                else
                {
                    using (HttpClient client = new HttpClient())
                    {

                        string apiKey = "45c5fd0e7b3a7f323010de71fbc4aae7943b9f139ef3578af1b38878d0d02e81";
                        string url = $"http://api.ipinfodb.com/v3/ip-city/?key={apiKey}&ip={ipOrHostName}&format=json";
                        string result = client.GetStringAsync(url).Result;

                        IpLocationViewModel ipLocationViewModel = JsonConvert.DeserializeObject<IpLocationViewModel>(result);
                        IpLocation ipLocationToSaveInDb = Mapper.Map<IpLocation>(ipLocationViewModel);
                        context.IpLocations.Add(ipLocationToSaveInDb);
                        context.SaveChanges();
                        return ipLocationViewModel;
                    }
                }
            }
        }

        public static string RandomString(int length)
        {
#pragma warning disable JustCode_NamingConventions // Naming conventions inconsistency
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
#pragma warning restore JustCode_NamingConventions // Naming conventions inconsistency
            var result = new string(Enumerable.Repeat(chars, length).Select(s => s[RandomNumberGenerator.Next(s.Length)]).ToArray());
            return result;
        }

        public static string GetGoogleMapsString(IEnumerable<string> ips, List<PingResponseSummary> pingSummaries = null,  bool starLine = true)
        {
            ips = ips.Where(m => !string.IsNullOrEmpty(m));

            var locations = new List<IpLocationViewModel>();
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
                IpLocationViewModel currLocation = GetLocationDataForIp(ips.ElementAt(i));
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

            List<IpLocationViewModel> sourceLocations = new List<IpLocationViewModel>();

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
                        double newLongitude = currLocation.Longitude.GetValueOrDefault() + 0.05;
                        currLocation.Longitude = newLongitude;
                    }
                }
            }


            var sb = new StringBuilder();


            for (int i = 0; i < ips.Count(); i++)
            {
                IpLocationViewModel location = locations[i];
            }

            for (int i = 0; i < ips.Count(); i++)
            {
                sb.AppendLine($@"var {locationNamesInMap[i]} = {{ lat: {locations[i].Latitude.GetValueOrDefault().ToString(CultureInfo.InvariantCulture)}, lng: {locations[i].Longitude.GetValueOrDefault().ToString(CultureInfo.InvariantCulture)} }};");
            }

            sb.AppendLine($@"var map = new google.maps.Map(document.getElementById('map'), {{
                zoom: 2,
                center: {locationNamesInMap.LastOrDefault()}
            }});");

            string fullRootUrl = HttpContext.Current?.Request.Url.OriginalString.Replace(HttpContext.Current?.Request.Url.LocalPath, string.Empty);
            if (!HttpContext.Current.Request.Url.ToString().Contains(value: "localhost"))
            {
                fullRootUrl = fullRootUrl.Replace(":" + HttpContext.Current?.Request.Url.Port, string.Empty);
            }
            string greenColorPin = @"
            var pinGreenColor = '00FF00';
            var pinGreenImage = new google.maps.MarkerImage('http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld=%E2%80%A2|' + pinGreenColor,
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

            string hostAddress = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.LocalPath, string.Empty);

            string redColorPing = $@"
            var pinRedColor = 'FF0000';
            var pinRedImage = new google.maps.MarkerImage('{hostAddress}/content/img/pinRedColor.png',
                new google.maps.Size(21, 34),
                new google.maps.Point(0, 0),
                new google.maps.Point(10, 34));";
            sb.AppendLine(redColorPing);

            for (int i = 0; i < ips.Count(); i++)
            {
               

                double distanceKm = 0;
                double distanceMiles = 0;
                if (i%2 == 0)
                {
                   distanceKm = GeoCodeCalc.CalcDistance(locations[i].Latitude.GetValueOrDefault(), locations[i].Longitude.GetValueOrDefault(),
                       locations[i + 1].Latitude.GetValueOrDefault(), locations[i + 1].Longitude.GetValueOrDefault());
                    distanceKm = Math.Round(distanceKm, digits: 0);
                    distanceMiles = GeoCodeCalc.CalcDistance(locations[i].Latitude.GetValueOrDefault(), locations[i].Longitude.GetValueOrDefault(),
                       locations[i + 1].Latitude.GetValueOrDefault(), locations[i + 1].Longitude.GetValueOrDefault(), GeoCodeCalcMeasurement.Miles);
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
                if (i % 2 == 0)
                {
                    timeAvg = @"<font size=""2"" color=""#057CBE"">TIME AVG:&nbsp;</font>" + Math.Round(pingSummaries[i / 2].AvgRtt.GetValueOrDefault(), digits: 0).ToString() + "ms.<br />";
                    distance = @"<font size=""2"" color=""#057CBE"">DISTANCE:&nbsp;</font>" + distanceKm.ToString() + "km./" + distanceMiles +"mi.<br />";
                }

                var sbMarkerWindowHtml = new StringBuilder();
                sbMarkerWindowHtml.Append($@"<font size=""2"" color=""#057CBE"">IP:&nbsp;</font> {ips.ElementAt(i)} <br />{timeAvg}{distance}<font size=""2"" color=""#057CBE"">CITY:&nbsp;</font> {locations[i].CityName.Replace(oldValue: "'", newValue: "&quot;")} <br /><font size=""2"" color=""#057CBE"">COUNTRY:&nbsp;</font> {locations[i].CountryName.Replace(oldValue: "'", newValue: "&quot;")}");

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
                //Note: Host seems down. If it is really up, but blocking our ping probes, try -Pn
                string encodedArgs0 = Uri.EscapeDataString($"-T4 --top-ports 1000 -Pn {ipOrHostName}");
                string url = "http://ants-neu.cloudapp.net/home/exec?program=nmap&args=" + encodedArgs0;
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
                double result = radius * 2 * Math.Asin(Math.Min(val1: 1, val2: Math.Sqrt((Math.Pow(Math.Sin((DiffRadian(lat1, lat2)) / 2.0), y: 2.0)+ Math.Cos(ToRadian(lat1)) * Math.Cos(ToRadian(lat2)) * Math.Pow(Math.Sin((DiffRadian(lng1, lng2)) / 2.0), y: 2.0)))));
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