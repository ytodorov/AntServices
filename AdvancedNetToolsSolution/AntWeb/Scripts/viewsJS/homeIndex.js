$(window).ready(function test() {

    var dublinLatLng = { lat: 53.349443, lng: -6.260082 };
    var sofiaLatLng = { lat: 42.6975, lng: 23.3242 };
    var hongKongLatLng = { lat: 22.41102, lng: 114.09301 };
    var singaporeLatLng = { lat: 1.27842, lng: 103.84414 };
    var sanAntonioLatLng = { lat: 29.42412, lng: -98.49362 };
    var californiaLatLng = { lat: 36.77826, lng: -119.41793 };
    var virginiaLatLng = { lat: 37.43157, lng: -78.65689 };
    var chicagoLatLng = { lat: 41.87811, lng: -87.62979 };
    var amsterdamLatLng = { lat: 52.37021, lng: 4.89516 };
    var melbourneLatLng = { lat: -37.81421, lng: 144.96323 };
    var sydneyLatLng = { lat: -33.86748, lng: 151.20699 };
    var saitamaLatLng = { lat: 35.85699, lng: 139.64884 };
    var osakaLatLng = { lat: 34.69373, lng: 135.50216 };
    var saoPauloLatLng = { lat: -23.54317, lng: -46.62918 };
    var iowaLatLng = { lat: 41.878, lng: -93.0977 };

    //unused
    //var eastUSLatLng = { lat: 42.6975, lng: 23.3242 };
    //var westUSLatLng = { lat: 42.6975, lng: 23.3242 };

    //var japanLatLng = { lat: 42.6975, lng: 23.3242 };


    var map = new google.maps.Map(document.getElementById('map'), {
        zoom: 2,
        center: dublinLatLng
    });

    var marker = new google.maps.Marker({
        position: dublinLatLng,
        map: map,
        title: 'North Europe - Dublin, Ireland'
    });

    var marker = new google.maps.Marker({
        position: sofiaLatLng,
        map: map,
        title: 'Eastern Europe - Sofia, Bulgaria'
    });

    var marker = new google.maps.Marker({
        position: hongKongLatLng,
        map: map,
        title: 'East Asia - Hong Kong, China'
    });

    var marker = new google.maps.Marker({
        position: singaporeLatLng,
        map: map,
        title: 'South East Asia - Singapore'
    });

    var marker = new google.maps.Marker({
        position: sanAntonioLatLng,
        map: map,
        title: 'South-central US - San Antonio, TX'
    });

    var marker = new google.maps.Marker({
        position: californiaLatLng,
        map: map,
        title: 'West US - California'
    });

    var marker = new google.maps.Marker({
        position: virginiaLatLng,
        map: map,
        title: 'East US - Virginia'
    });

    var marker = new google.maps.Marker({
        position: chicagoLatLng,
        map: map,
        title: 'North-central US - Chicago, IL'
    });

    var marker = new google.maps.Marker({
        position: amsterdamLatLng,
        map: map,
        title: 'West Europe - Amsterdam, Netherlands'
    });

    var marker = new google.maps.Marker({
        position: melbourneLatLng,
        map: map,
        title: 'Australia Southeast - Melbourne'
    });

    var marker = new google.maps.Marker({
        position: sydneyLatLng,
        map: map,
        title: 'Australia East - Sydney'
    });

    var marker = new google.maps.Marker({
        position: saitamaLatLng,
        map: map,
        title: 'Japan East - Saitama'
    });

    var marker = new google.maps.Marker({
        position: osakaLatLng,
        map: map,
        title: 'Japan West - Osaka'
    });

    var marker = new google.maps.Marker({
        position: saoPauloLatLng,
        map: map,
        title: 'Brazil South - São Paulo'
    });

    var marker = new google.maps.Marker({
        position: iowaLatLng,
        map: map,
        title: 'Central US - Iowa'
    });

    var infowindow = new google.maps.InfoWindow({
        content: "test"
    });

    marker.addListener('click', function () {
        infowindow.open(map, marker);
    });


    var line = new google.maps.Polyline({
        path: [
            dublinLatLng,
            sofiaLatLng,
            hongKongLatLng
        ],
        strokeColor: "#FF0000",
        strokeOpacity: 1.0,
        strokeWeight: 1,
        map: map
    });

    $("#map").width("100%");


});