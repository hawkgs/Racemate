$(function () {
    "use strict";

    $(".pop-up-btn").click(function () {
        var TOP_OFFSET = 100,
            windowId = $(this).data("window"),
            $winContainer = $("#pop-ups").find("[data-wid=" + windowId + "]"),
            $window = $winContainer.find(".window"),
            $close = $winContainer.find(".cl-action");

        $winContainer.show();

        $window.css({
            marginTop: (($(window).height() - $window.height()) / 2) - TOP_OFFSET, // !!!
            marginLeft: ($(window).width() - $window.width()) / 2
        });

        $close.click(function () {
            $winContainer.hide();
        });
    });
});