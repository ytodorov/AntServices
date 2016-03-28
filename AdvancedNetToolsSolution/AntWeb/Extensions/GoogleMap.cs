using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartAdminMvc.Extensions
{
    public static class GoogleMap
    {
        public static string GetMapWithLocaions()
        {
            string result = @"var myLatLng = { lat: 53.349443, lng: -6.260082 };
            var sofiaLatLng = { lat: 42.6975, lng: 23.3242 };

            var map = new google.maps.Map(document.getElementById('map'), {
                zoom: 2,
                center: myLatLng
            });

            var marker = new google.maps.Marker({
                position: myLatLng,
                map: map,
                title: 'Dublin'
            });

            var marker = new google.maps.Marker({
                position: sofiaLatLng,
                map: map,
                title: 'Sofia'
            });

            var line = new google.maps.Polyline({
                path: [
                   myLatLng,
                    sofiaLatLng
                ],
                strokeColor: '#FF0000',
                strokeOpacity: 1.0,
                strokeWeight: 1,
                map: map
            });

            $('#map').width('100%');";

            return result;
        }

    }
}