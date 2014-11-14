var RouteBuilder = (function() {
    "use strict";

    var $$core = MapCore,
        KILOMETER = 1000;

    // static
    function Builder() {}

    Builder.markers = [];

    // TODO: Refactor
    Builder.drawRoute = function(distanceCallback, addressCallback) {
        var wayPointsLatLng = [],
            start = Builder.markers[0],
            finish = Builder.markers[Builder.markers.length - 1],
            waypoints = Builder.markers.slice(1, Builder.markers.length - 1),
            finishPointLatLng,
            startPointLatLng;

        finishPointLatLng = new google.maps.LatLng(start.getPosition().lat(), start.getPosition().lng());
        startPointLatLng = new google.maps.LatLng(finish.getPosition().lat(), finish.getPosition().lng());

        Builder.getMarkerAddress(startPointLatLng, addressCallback);

        waypoints.forEach(function(point) {
            wayPointsLatLng.push({
                location: new google.maps.LatLng(point.getPosition().lat(), point.getPosition().lng()),
                stopover: true
            });
        });

        Builder.generateRoute(startPointLatLng, finishPointLatLng, wayPointsLatLng, distanceCallback);
    };

    Builder.generateRoute = function(start, finish, waypoints, distanceCallback) {
        var request = {
            origin: start,
            destination: finish,
            waypoints: waypoints,
            optimizeWaypoints: true,
            travelMode: google.maps.TravelMode.DRIVING
        };

        $$core.directionsService.route(request, function (response, status) {
            if (status == google.maps.DirectionsStatus.OK) {
                var legs = response.routes[0].legs,
                    totalDistance = 0,
                    i;

                $$core.directionsRenderer.setDirections(response);

                for (i = 0; i < legs.length; i += 1) {
                    totalDistance += legs[i].distance.value;
                }

                Builder.distance = totalDistance / KILOMETER;

                if (distanceCallback) {
                    distanceCallback(Builder.distance);
                }
            }
        });
    };

    Builder.getMarkerAddress = function(pos, callback) {
        $$core.geocoder.geocode({
            latLng: pos
        }, function(responses) {
            if (responses && responses.length > 0) {
                Builder.address = responses[0].formatted_address;
            } else {
                Builder.address = "Unknown";
            }

            if (callback) {
                callback(Builder.address);
            }
        });
    };

    return Builder;
}());