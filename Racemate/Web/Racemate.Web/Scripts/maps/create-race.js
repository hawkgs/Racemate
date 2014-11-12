(function() {
    "use strict";

    var $$routeBuilder = RouteBuilder,
        $$core = MapCore,
        SUCC_REDRCT_ROUTE = "/User/Home";

    // static
    function RaceCreator() {}

    // >>> Event-based functions

    RaceCreator.bindDragEventToMarker = function(marker) {
        google.maps.event.addListener(marker, "dragend", function() {
            $$routeBuilder.drawRoute(function(distance) {
                $("#distance-field").text(distance + " km");
            },
            function(address) {
                $("#location-field").text(address);
            });
        });
    };

    RaceCreator.bindEvents = function() {
        var $nodeMarker = $("#add-node-marker");

        $("#add-init-markers").on("click", function() {
            RaceCreator.addInitialMarkers();
            $(this).hide();
            $nodeMarker.fadeIn(250);
        });

        $nodeMarker.on("click", function() {
            RaceCreator.addMarker();
        });

        $("#create-race-btn").click(function() {
            RaceCreator.onFormSubmit();
        });
    };

    // >>> Form

    RaceCreator.onFormSubmit = function() {
        var dataObject = {
            routepoints: RaceCreator.convertMarkersToLatLng(),
            address: $$routeBuilder.address,
            distance: $$routeBuilder.distance,
            typeId: $("#race-type-select").val(),
            duration: $("#duration").val(),
            dateTimeOfRace: (new Date($("#date-time-race").val())).toISOString(),
            availableRacePositions: $("#racers").val(),
            name: $("#race-name").val(),
            description: $("#race-description").val(),
            moneyBet: $("#bet").val(),
            password: $("#race-password").val()
        };

        RaceCreator.postAjaxRequest(dataObject);
    };

    RaceCreator.postAjaxRequest = function(dataObject) {
        $.ajax({
            method: "POST",
            url: "Create",
            data: dataObject,
            success: function (data) {
                window.location = SUCC_REDRCT_ROUTE;
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

    RaceCreator.convertMarkersToLatLng = function () {
        var waypoints = [];

        $$routeBuilder.markers.forEach(function(point) {
            waypoints.push({
                latitude: point.getPosition().lat(), 
                longitude: point.getPosition().lng()
            });
        });

        return waypoints.reverse();
    };

    // >>> Marker-based functions

    RaceCreator.addInitialMarkers = function() {
        var start = $$core.createMarker("START", $$core.MARKERS_PATH + "start_marker.png"),
            finish = $$core.createMarker("FINISH", $$core.MARKERS_PATH + "finish_marker.png");

        RaceCreator.bindDragEventToMarker(finish);
        $$routeBuilder.markers.push(finish);

        RaceCreator.bindDragEventToMarker(start);
        $$routeBuilder.markers.push(start);
    };

    RaceCreator.addMarker = function() {
        var point = $$core.createMarker("Point", $$core.MARKERS_PATH + "route_point.png");

        RaceCreator.bindDragEventToMarker(point);
        $$routeBuilder.markers.splice($$routeBuilder.markers.length - 1, 0, point);
    };

    // Initialization
    $("#date-time-race").datetimepicker();
    RaceCreator.bindEvents();
    $$core.init();
}());