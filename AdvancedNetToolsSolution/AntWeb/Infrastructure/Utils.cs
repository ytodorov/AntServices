using Newtonsoft.Json;
using SmartAdminMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Helpers;

namespace SmartAdminMvc.Infrastructure
{
    public static class Utils
    {
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
            var random = new Random();
            var result = new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
            return result;
        }
    }
}