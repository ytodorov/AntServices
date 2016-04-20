using AntDal;
using AntDal.Entities;
using AutoMapper;
using Newtonsoft.Json;
using SmartAdminMvc.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Helpers;

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

            using (System.IO.StreamReader sr = new System.IO.StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("~/Misc/TopSites.txt")))
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

            HotstNameToIp = new Dictionary<string, string>()
        {
            {  "http://ants-ea.cloudapp.net", "40.83.125.9" },
            {  "http://ants-je.cloudapp.net", "13.71.155.140" },
            {  "http://ants-sea.cloudapp.net", "13.76.100.42" },
            {  "http://ant-cus.cloudapp.net", "52.165.26.10" },
            {  "http://ant-jw.cloudapp.net", "40.74.137.95" },
            {  "http://ants-eus.cloudapp.net", "13.90.212.72" },
            {  "http://ants-neu.cloudapp.net", "13.74.188.161" },
            {  "http://ants-scus.cloudapp.net", "104.214.70.79" },
            {  "http://ants-weu.cloudapp.net", "104.46.38.89" },
            {  "http://ants-wus.cloudapp.net" , "13.93.211.38"}
        };
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
                var ipLocation = context.IpLocations.Where(ip => ip.IpAddress == ipOrHostName).FirstOrDefault();
                if (ipLocation != null)
                {
                    var IpLocationViewModel = Mapper.Map<IpLocationViewModel>(ipLocation);
                    return IpLocationViewModel;
                }
                else
                {
                    using (HttpClient client = new HttpClient())
                    {

                        string apiKey = "45c5fd0e7b3a7f323010de71fbc4aae7943b9f139ef3578af1b38878d0d02e81";
                        string url = $"http://api.ipinfodb.com/v3/ip-city/?key={apiKey}&ip={ipOrHostName}&format=json";
                        string result = client.GetStringAsync(url).Result;

                        var ipLocationViewModel = JsonConvert.DeserializeObject<IpLocationViewModel>(result);
                        var ipLocationToSaveInDb = Mapper.Map<IpLocation>(ipLocationViewModel);
                        context.IpLocations.Add(ipLocationToSaveInDb);
                        context.SaveChanges();
                        return ipLocationViewModel;
                    }
                }
            }
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var result = new string(Enumerable.Repeat(chars, length).Select(s => s[RandomNumberGenerator.Next(s.Length)]).ToArray());
            return result;
        }

        public static string GetGoogleMapsString(IEnumerable<string> ips, bool starLine = true)
        {
            ips = ips.Where(m => !string.IsNullOrEmpty(m));

            List<IpLocationViewModel> locations = new List<IpLocationViewModel>();
            List<string> locationNamesInMap = new List<string>();
            List<string> markerNamesInMap = new List<string>();
            List<string> infoWindowsNamesInMap = new List<string>();

            for (int i = 0; i < ips.Count(); i++)
            {
                var currLocation = GetLocationDataForIp(ips.ElementAt(i));

                locations.Add(currLocation);
                locationNamesInMap.Add("latLng" + i);
                markerNamesInMap.Add("marker" + i);
                infoWindowsNamesInMap.Add("infoWindow" + i);
            }


            StringBuilder sb = new StringBuilder();


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
            if (!HttpContext.Current.Request.Url.ToString().Contains("localhost"))
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


            string redColorPing = @"
            var pinRedColor = 'FF0000';
            var pinRedImage = new google.maps.MarkerImage('http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld=%E2%80%A2|' + pinRedColor,
                new google.maps.Size(21, 34),
                new google.maps.Point(0, 0),
                new google.maps.Point(10, 34));";
            sb.AppendLine(redColorPing);

            for (int i = 0; i < ips.Count(); i++)
            {
                if (i%2 == 0)
                {
                    sb.AppendLine($@"var {markerNamesInMap[i]} = new google.maps.Marker({{
                position: {locationNamesInMap[i]},
                map: map,
                icon: pinRedImage,
                title: '{i}. {ips.ElementAt(i)} {locations[i].CityName} {locations[i].RegionName} {locations[i].CountryName}'
                }});");
                }
                else
                {
                    sb.AppendLine($@"var {markerNamesInMap[i]} = new google.maps.Marker({{
                position: {locationNamesInMap[i]},
                map: map,
                icon: pinGreenImage,
                title: '{i}. {ips.ElementAt(i)} {locations[i].CityName} {locations[i].RegionName} {locations[i].CountryName}'
                }});");
                                        
                }

                var infowindow = $@" var {infoWindowsNamesInMap[i]} = new google.maps.InfoWindow({{
    content: '{i}. {ips.ElementAt(i)} {locations[i].CityName} {locations[i].RegionName} {locations[i].CountryName}',
    maxWidth: 200
  }});";
                sb.Append(infowindow);

                var markerWithInfoWindow = $@" {markerNamesInMap[i]}.addListener('click', function() {{
    {infoWindowsNamesInMap[i]}.open(map, {markerNamesInMap[i]});
  }});";

                sb.Append(markerWithInfoWindow);
            }

            StringBuilder locationStringsForPolyline = new StringBuilder();
            for (int i = 0; i < locationNamesInMap.Count; i++)
            {
                locationStringsForPolyline.Append(locationNamesInMap[i]);
                if (i != locationNamesInMap.Count - 1)
                {
                    locationStringsForPolyline.AppendLine(",");
                }
            }

            // Define a symbol using a predefined path (an arrow)
            // supplied by the Google Maps JavaScript API.
            sb.Append(@" var lineSymbol = {
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
        offset: '50%'
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

            sb.AppendLine(" $('#map').width('100%');");
            string finalResult = sb.ToString();

            return finalResult;
        }





        public static string GetIpAddressFromHostName(string hostName, string locationOfDeployedService)
        {
            using (HttpClient client = new HttpClient())
            {

                var encodedArgs0 = Uri.EscapeDataString($"{hostName} -n -sn -Pn");
                string url = $"{locationOfDeployedService}/home/exec?program=nmap&args=" + encodedArgs0;
                var res = client.GetStringAsync(url).Result;

                var lines = res.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                string ipLine = lines.FirstOrDefault(l => l.StartsWith("Nmap scan report"));
                if (ipLine != null)
                {
                    int lastIndexOfOpen = ipLine.LastIndexOf("(");
                    int lastIndexOfClose = ipLine.LastIndexOf(")");
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
            var isIpAddress = IPAddress.TryParse(ipOrUrlToTest, out dummy);
            if (isIpAddress)
            {
                return true;
            }
            Uri uriDummy;
            var isValidUri = Uri.TryCreate(ipOrUrlToTest, UriKind.Absolute, out uriDummy);
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
                var encodedArgs0 = Uri.EscapeDataString($"-T4 -F -Pn {ipOrHostName}");
                string url = "http://ants-neu.cloudapp.net/home/exec?program=nmap&args=" + encodedArgs0;
                var portSummary = client.GetStringAsync(url).Result;

                List<PortResponseSummaryViewModel> portViewModels = PortParser.ParseSummary(portSummary);
                var firstOpen = portViewModels.FirstOrDefault(p => p.State.Equals("open"));
                string result = string.Empty;
                if (firstOpen != null)
                {
                    result = firstOpen.PortNumber.ToString();
                }
                return result;


            }

        }

        public static class GeoCodeCalc
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
                var result = radius * 2 * Math.Asin(Math.Min(1, Math.Sqrt((Math.Pow(Math.Sin((DiffRadian(lat1, lat2)) / 2.0), 2.0) + Math.Cos(ToRadian(lat1)) * Math.Cos(ToRadian(lat2)) * Math.Pow(Math.Sin((DiffRadian(lng1, lng2)) / 2.0), 2.0)))));
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