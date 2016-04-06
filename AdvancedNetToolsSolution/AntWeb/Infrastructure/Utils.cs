using Newtonsoft.Json;
using SmartAdminMvc.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Helpers;

namespace SmartAdminMvc.Infrastructure
{
    public static class Utils
    {
        public static Random RandomNumberGenerator = new Random();

        public static IpLocationViewModel GetLocationDataForIp(string ipOrHostName)
        {
            using (HttpClient client = new HttpClient())
            {

                string apiKey = "45c5fd0e7b3a7f323010de71fbc4aae7943b9f139ef3578af1b38878d0d02e81";
                string url = $"http://api.ipinfodb.com/v3/ip-city/?key={apiKey}&ip={ipOrHostName}&format=json";
                string result = client.GetStringAsync(url).Result;

                var ipLocationViewModel = JsonConvert.DeserializeObject<IpLocationViewModel>(result);
                return ipLocationViewModel;
            }
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var result = new string(Enumerable.Repeat(chars, length).Select(s => s[RandomNumberGenerator.Next(s.Length)]).ToArray());
            return result;
        }

        public static string GetGoogleMapsString(IEnumerable<string> ips)
        {
            ips = ips.Where(m => !string.IsNullOrEmpty(m));//.OrderBy(m => m.Hop).ToArray();

            List<IpLocationViewModel> locations = new List<IpLocationViewModel>();
            List<string> locationNamesInMap = new List<string>();
            List<string> markerNamesInMap = new List<string>();

            for (int i = 0; i < ips.Count(); i++)
            {
                var currLocation = GetLocationDataForIp(ips.ElementAt(i));

                //var lastLocation = locations.LastOrDefault();

                //if (locations.Any(l => l.Latitude == currLocation.Latitude && l.Longitude == currLocation.Longitude))
                //{
                //    currLocation.Latitude += RandomNumberGenerator.NextDouble() / 100;
                //    currLocation.Longitude += RandomNumberGenerator.NextDouble() / 100;
                //}

                locations.Add(currLocation);
                locationNamesInMap.Add("latLng" + i);
                markerNamesInMap.Add("marker" + i);
            }


            StringBuilder sb = new StringBuilder();


            for (int i = 0; i < ips.Count(); i++)
            {
                IpLocationViewModel location = locations[i];
            }

            for (int i = 0; i < ips.Count(); i++)
            {
                sb.AppendLine($@"var {locationNamesInMap[i]} = {{ lat: {locations[i].Latitude.ToString(CultureInfo.InvariantCulture)}, lng: {locations[i].Longitude.ToString(CultureInfo.InvariantCulture)} }};");
            }

            sb.AppendLine($@"var map = new google.maps.Map(document.getElementById('map'), {{
                zoom: 3,
                center: {locationNamesInMap.LastOrDefault()}
            }});");

            for (int i = 0; i < ips.Count(); i++)
            {
                sb.AppendLine($@"var {markerNamesInMap[i]} = new google.maps.Marker({{
                position: {locationNamesInMap[i]},
                map: map,
                title: '{i}. {ips.ElementAt(i)} {locations[i].CityName} {locations[i].RegionName} {locations[i].CountryName}'
            }});");
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

            sb.AppendLine(" $('#map').width('100%');");
            string finalResult = sb.ToString();

            return finalResult;
        }

        public static List<string> GetDeployedServicesUrlAddresses()
        {
            List<string> address = new List<string>()
            {
                "http://antnortheu.cloudapp.net",
                "http://ants-ea.cloudapp.net",
                 "http://ants-je.cloudapp.net"
                //"http://ant-ne.azurewebsites.net"
            };
            return address;
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