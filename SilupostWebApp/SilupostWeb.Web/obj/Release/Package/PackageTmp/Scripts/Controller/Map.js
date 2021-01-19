
var mapController = function() {
    var appSettings = {
        model:{},
        status:{ IsNew:false}
    };
    var init = function (obj) {
        // TO MAKE THE MAP APPEAR YOU MUST
        // ADD YOUR ACCESS TOKEN FROM
        // https://account.mapbox.com
        mapboxgl.accessToken = 'pk.eyJ1IjoiZXJ3aW5yYW1pcmV6MjIwIiwiYSI6ImNrZ3U1cHJzazAwYTAycm82MDRmdWNmczAifQ.TarlRjuzi62vw_hPR6uTGg';
        var map = new mapboxgl.Map({
            container: 'map', // container id
            style: 'mapbox://styles/mapbox/streets-v11',
            center: [-96, 37.8], // starting position
            zoom: 3 // starting zoom
        });

        // Add geolocate control to the map.


        var geolocate = new mapboxgl.GeolocateControl({
            positionOptions: {
                enableHighAccuracy: true
            },
            trackUserLocation: true
        });

        map.addControl(geolocate);

        geolocate.on('geolocate', function (e) {
            var lon = e.coords.longitude;
            var lat = e.coords.latitude
            var position = [lon, lat];
            $("#Latitude").val(lat);
            $("#Longitude").val(lon);

            console.log("Latitude " + $("#Latitude").val());
            console.log("Longitude " + $("#Longitude").val());
        });
    };

    //Function for clearing the textboxes
    return  {
        init: init
    };
}
var map = new mapController;
