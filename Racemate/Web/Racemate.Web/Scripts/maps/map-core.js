var MapCore = (function() {
    "use strict";

    var MAP_STYLE = [{ "featureType": "all", "elementType": "all", "stylers": [{ "invert_lightness": true }, { "saturation": 10 }, { "lightness": 30 }, { "gamma": 0.5 }, { "hue": "#435158" }] }],
        POLYLINE_STYLE = { strokeColor: "#ff9000", strokeOpacity: 0.6, strokeWeight: 8 };

    // static
    function Core() { }

    Core.MARKERS_PATH = "../../Images/markers/";

    Core._init = function() {
        var mapOptions;

        Core.geocoder = new google.maps.Geocoder();
        Core.directionsService = new google.maps.DirectionsService();
        Core.directionsRenderer = new google.maps.DirectionsRenderer({
            polylineOptions: POLYLINE_STYLE,
            suppressMarkers: true
        });

        mapOptions = {
            zoom: 15,
            styles: MAP_STYLE
        };

        Core.map = new google.maps.Map(document.getElementById("map-canvas"), mapOptions);
        Core.directionsRenderer.setMap(Core.map);

        Core.setCurrentPosition();
    };

    Core.setCurrentPosition = function() {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(function (position) {
                var pos = new google.maps.LatLng(position.coords.latitude, position.coords.longitude);
                Core.map.setCenter(pos);
            }, function () {
                handleNoGeolocation(true);
            });
        } else {
            handleNoGeolocation(false);
        }
    };

    Core.createMarker = function(title, icon) {
        var template = {
            position: Core.map.getCenter(),
            map: Core.map,
            draggable: true,
            animation: google.maps.Animation.DROP
        };

        if (title) {
            template.title = title;
        }

        if (icon) {
            template.icon = icon;
        }

        return new google.maps.Marker(template);
    };

    Core.init = function (onMapLoad) {
        google.maps.event.addDomListener(window, "load", function() {
            Core._init();

            if (onMapLoad) {
                onMapLoad();
            }
        });
    };

    return Core;
}());