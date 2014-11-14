(function() {
    "use strict";

    var $$routeBuilder = RouteBuilder,
        $$core = MapCore,
        waypoints = [],
        parsedData,
        finish,
        start;

    // static
    function Route() {}

    Route.getRouteData = function() {
        $.ajax({
            method: "GET",
            url: "/User/Race/RaceRoute/" + Common.getRaceId(),
            success: function (data) {
                parsedData = JSON.parse(data);

                console.log(parsedData);

                Route.parseMarkers();
                Route.timer();
                $$routeBuilder.generateRoute(start, finish, waypoints);
            },
            error: function () {
                console.log("An error occurred.");
            }
        });
    };

    Route.parseMarkers = function() {
        var markers = parsedData.Routepoints,
            current,
            point,
            i;

        for (i = 0; i < markers.length; i += 1) {
            current = markers[i];
            point = new google.maps.LatLng(current.Latitude, current.Longitude);

            if (i == 0) {
                start = point;
            } else if (i == markers.length - 1) {
                finish = point;
            } else {
                waypoints.push({
                    location: point,
                    stopover: true
                })
            }
        }
    };

    // Can be optimized a bit
    Route.timer = function () {
        if (parsedData.IsFinished || parsedData.IsCanceled) {
            return;
        }

        var startDate = new Date(parsedData.StartDate),
            endDate = new Date(parsedData.EndDate);

        console.log(startDate);
        console.log(endDate);

        var updateTimer = function () {
            var $countDown = $("#race-countdown"),
                currentDate = new Date(),
                text;

            if (currentDate < startDate) {
                text = Math.abs(startDate - currentDate);
            }
            else if (currentDate < endDate) {
                text = Math.abs(endDate - currentDate)
            }
            else {
                $countDown.text("--:--:--");
                return;
            }

            $countDown.text(Common.msecToTimer(text));
            
            setTimeout(updateTimer, 1000);
        }

        updateTimer();
    };

    $$core.init(function() {
        Route.getRouteData();
    }, true);
}());