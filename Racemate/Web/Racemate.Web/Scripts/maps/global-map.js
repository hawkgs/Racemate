(function () {
    "use strict";

    var $$core = MapCore,
        MARKER_EXT = ".png";

    function Global() {}

    Global.createRaceTag = function (raceType, name, latitude, longitude) {
        var marker = {
            position: new google.maps.LatLng(latitude, longitude),
            map: $$core.map,
            title: name,
            icon: $$core.MARKERS_PATH + raceType.toLowerCase().replace(" ", "") + MARKER_EXT
        }

        return new google.maps.Marker(marker);
    }

    Global.getMapData = function () {
        $.ajax({
            method: "GET",
            url: "/User/Home/MapRaces",
            success: function (data) {
                if (data) {
                    data.forEach(function (item) {
                        Global.createRaceTag(item.Type, item.Name, item.StartLatitude, item.StartLongitude);
                    });
                }
            },
            error: function () {
                console.log("An error occurred while processing data.");
            }
        });
    };

    Global.bindEvents = function () {
        $("#enlarge-map").click(function () {
            $("#map-canvas").css({
                position: "fixed",
                width: "100%",
                height: "100%",
                top: $("#main-header").height(),
                left: "0"
            });

            $(this).css({
                position: "fixed",
                right: "5px",
                bottom: "10px"
            });

            google.maps.event.trigger($$core.map, "resize");
        });
       
    };

    Global.enlargeMap = function () {

    };

    // Initialization
    $$core.init(function () {
        Global.getMapData();
        Global.bindEvents();
    });
}());