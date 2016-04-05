using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Homer_MVC.Models
{
    public class IpLocationViewModel
    {
        public string StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public string IpAddress { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string RegionName { get; set; }
        public string CityName { get; set; }
        public string ZipCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string TimeZone { get; set; }

    }
}

/*
"statusCode" : "OK",
	"statusMessage" : "",
	"ipAddress" : "194.153.145.104",
	"countryCode" : "BG",
	"countryName" : "Bulgaria",
	"regionName" : "Grad Sofiya",
	"cityName" : "Sofia",
	"zipCode" : "1000",
	"latitude" : "42.6975",
	"longitude" : "23.3242",
	"timeZone" : "+02:00"
*/
