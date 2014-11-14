var Common = (function() {
    "use strict";

    var Common = {};

    Common.getRaceId = function() {
        var queryParams = window.location.pathname.split("/"),
            encryptedId = queryParams[queryParams.length - 1];

        return encryptedId;
    };

    Common.msecToTimer = function(milliseconds) {
        var temp = Math.floor(milliseconds / 1000),
            days = Math.floor((temp %= 31536000) / 86400),
            hours = Math.floor((temp %= 86400) / 3600),
            minutes = Math.floor((temp %= 3600) / 60),
            seconds = temp % 60,
            result = "";

        if (days > 0) {
            result += days + "d ";
        }

        result += Common.formatTimeUnit(hours) + ":";
        result += Common.formatTimeUnit(minutes) + ":";
        result += Common.formatTimeUnit(seconds);

        return result;
    };

    Common.formatTimeUnit = function(unit) {
        if (unit.toString().length === 1) {
            return "0" + unit;
        }

        return unit;
    };

    return Common;
}());