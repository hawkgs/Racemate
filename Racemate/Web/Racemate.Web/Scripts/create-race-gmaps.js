(function() {
    "use strict";

    var MAP_STYLE = [{"featureType":"all","elementType":"all","stylers":[{"invert_lightness":true},{"saturation":10},{"lightness":30},{"gamma":0.5},{"hue":"#435158"}]}],
        POLYLINE_STYLE = { strokeColor: "#ff9000", strokeOpacity: 0.6, strokeWeight: 8 },
        MARKERS_PATH = "../../Images/markers/",
        KILOMETER = 1000,
        distance = 0,
        directionsRenderer,
        directionsService,
        geocoder,
        address,
        map,
        markers = [];

    function Route() {}

    Route.init = function() {
        var mapOptions;

        geocoder = new google.maps.Geocoder();
        directionsService = new google.maps.DirectionsService();
        directionsRenderer = new google.maps.DirectionsRenderer({
            polylineOptions: POLYLINE_STYLE,
            suppressMarkers: true
        });

        mapOptions = {
            zoom: 15,
            styles: MAP_STYLE
        };

        map = new google.maps.Map(document.getElementById("map-canvas"), mapOptions);
        directionsRenderer.setMap(map);

        Route.setCurrentPosition();
        Route.bindEvents();
    };

    Route.setCurrentPosition = function() {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(function (position) {
                var pos = new google.maps.LatLng(position.coords.latitude, position.coords.longitude);
                map.setCenter(pos);
            }, function () {
                handleNoGeolocation(true);
            });
        } else {
            handleNoGeolocation(false);
        }
    };

    // >>> Event-based functions

    Route.bindDragEventToMarker = function(marker) {
        google.maps.event.addListener(marker, "dragend", function() {
            Route.drawRoute();
        });
    };

    Route.bindEvents = function() {
        var $nodeMarker = $("#add-node-marker");

        $("#add-init-markers").on("click", function() {
            Route.addInitialMarkers();
            $(this).hide();
            $nodeMarker.fadeIn(250);
        });

        $nodeMarker.on("click", function() {
            Route.addMarker();
        });

        $("#create-race-btn").click(function() {
            Route.onFormSubmit();
        });
    };

    // >>> Form

    Route.onFormSubmit = function() {
        var dataObject = {
            routepoints: Route.convertMarkersToLatLng(),
            address: address,
            distance: distance,
            typeId: $("#race-type-select").val(),
            duration: $("#duration").val(),
            dateTimeOfRace: (new Date($("#date-time-race").val())).toISOString(),
            availableRacePositions: $("#racers").val(),
            name: $("#race-name").val(),
            description: $("#race-description").val(),
            moneyBet: $("#bet").val(),
            password: $("#race-password").val()
        };

        Route.postAjaxRequest(dataObject);
    };

    Route.postAjaxRequest = function(dataObject) {
        $.ajax({
            method: "POST",
            url: "Create",
            data: dataObject,
            success: function (data) {
                window.location = "User/Home";
            },
            error: function (response) {
                var $errorContainer = $("#error-container");
                $errorContainer.show();

                response.responseJSON.forEach(function (msg) {
                    var message = $("<div>")
                        .addClass("msg")
                        .addClass("error")
                        .text(msg);

                    $errorContainer.append(message);
                });

                $errorContainer.delay(5000).fadeOut(1000, function () {
                    $(this).html("");
                });
            }
        });
    };

    Route.convertMarkersToLatLng = function () {
        var waypoints = [];

        markers.forEach(function(point) {
            waypoints.push({
                latitude: point.getPosition().lat(), 
                longitude: point.getPosition().lng()
            });
        });

        return waypoints;
    };

    // >>> Route

    Route.drawRoute = function() {
        var wayPointsLatLng = [],
            start = markers[0],
            finish = markers[markers.length - 1],
            waypoints = markers.slice(1, markers.length - 1),
            finishPointLatLng,
            startPointLatLng,
            request;

        finishPointLatLng = new google.maps.LatLng(start.getPosition().lat(), start.getPosition().lng());
        startPointLatLng = new google.maps.LatLng(finish.getPosition().lat(), finish.getPosition().lng());

        Route.getMarkerAddress(startPointLatLng);

        waypoints.forEach(function(point) {
            wayPointsLatLng.push({
                location: new google.maps.LatLng(point.getPosition().lat(), point.getPosition().lng()),
                stopover: true
            });
        });

        request = {
            origin: startPointLatLng,
            destination: finishPointLatLng,
            waypoints: wayPointsLatLng,
            optimizeWaypoints: true,
            travelMode: google.maps.TravelMode.DRIVING
        };

        directionsService.route(request, function(response, status) {
            if (status == google.maps.DirectionsStatus.OK) {
                var legs = response.routes[0].legs,
                    totalDistance = 0,
                    i;

                directionsRenderer.setDirections(response);

                for (i = 0; i < legs.length; i+=1) {
                    totalDistance += legs[i].distance.value;
                }

                distance = totalDistance / KILOMETER;

                $("#distance-field").text(distance + " km");
            }
        });
    };

    // >>> Marker-based functions

    Route.addInitialMarkers = function() {
        var start = Route.createMarker("START", MARKERS_PATH + "start_marker.png"),
            finish = Route.createMarker("FINISH", MARKERS_PATH + "finish_marker.png");

        Route.bindDragEventToMarker(finish);
        markers.push(finish);

        Route.bindDragEventToMarker(start);
        markers.push(start);
    };

    Route.addMarker = function() {
        var point = Route.createMarker("Point", MARKERS_PATH + "route_point.png");

        Route.bindDragEventToMarker(point);
        markers.splice(markers.length - 1, 0, point);
    };

    Route.createMarker = function(title, icon) {
        var template = {
            position: map.getCenter(),
            map: map,
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

    Route.getMarkerAddress = function(pos) {
        geocoder.geocode({
            latLng: pos
        }, function(responses) {
            if (responses && responses.length > 0) {
                address = responses[0].formatted_address;
            } else {
                address = "Unknown";
            }

            $("#location-field").text(address);
        });
    };

    google.maps.event.addDomListener(window, "load", Route.init);
    $("#date-time-race").datetimepicker();
}());